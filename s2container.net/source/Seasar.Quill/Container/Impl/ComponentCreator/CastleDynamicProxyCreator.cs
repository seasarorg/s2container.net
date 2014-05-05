using Castle.DynamicProxy;
using Seasar.Quill.AOP;
using Seasar.Quill.Attr;
using Seasar.Quill.Config;
using Seasar.Quill.Extensiion;
using Seasar.Quill.Injection;
using Seasar.Quill.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Seasar.Quill.Container.Impl.ComponentCreator
{
    /// <summary>
    /// Castle.DynamicProxyを使用したProxyオブジェクト生成クラス
    /// </summary>
    public class CastleDynamicProxyCreator : IComponentCreater
    {
        private readonly ConcurrentDictionary<Type, IInterceptor> _interceptorMap
            = new ConcurrentDictionary<Type, IInterceptor>();
        
        private readonly IQuillInjector _injector;

        private readonly ProxyGenerationHookImpl _proxyGenerationHook = new ProxyGenerationHookImpl();
        private readonly ProxyGenerationOptions _options;

        public CastleDynamicProxyCreator(IQuillInjector injector)
        {
            _injector = injector;
            
            var selector = new InterceptorSelectorImpl();
            _proxyGenerationHook = new ProxyGenerationHookImpl();
            _options = new ProxyGenerationOptions(_proxyGenerationHook) { Selector = selector };
        }

        public object Create(Type t)
        {
            _proxyGenerationHook.TargetType = t;
            var interceptors = GetInterceptors(t);
            var generator = new ProxyGenerator();
            if (t.IsInterface)
            {
                return generator.CreateInterfaceProxyWithoutTarget(t, _options, interceptors);
            }

            return generator.CreateClassProxy(t, _options, interceptors);
        }

        #region 内部メソッド
        private IInterceptor[] GetInterceptors(Type t)
        {
            var interceptors = new Dictionary<Type, IInterceptor>();

            var attrs = t.GetCustomAttributes<AspectAttribute>(false);
            RegisterInterceptors(interceptors, attrs);

            var members = t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (members != null && members.Count() > 0)
            {
                foreach (var member in members)
                {
                    var memberAttrs = member.GetCustomAttributes<AspectAttribute>(false);
                    RegisterInterceptors(interceptors, memberAttrs);
                }
            }
            return interceptors.Values.ToArray();
        }

        private void RegisterInterceptors(IDictionary<Type, IInterceptor> interceptors, IEnumerable<AspectAttribute> aspects)
        {
            foreach(var aspect in aspects)
            {
                Type interceptorType = aspect.InterceptorType;
                if (!interceptors.ContainsKey(interceptorType))
                {
                    if (!_interceptorMap.ContainsKey(interceptorType))
                    {
                        var interceptor = new CastleInterceptorAdapter(
                            (IMethodInterceptor)_injector.CreateInjectedInstance(interceptorType), aspect.Ordinal);
                        _interceptorMap.AddOrUpdate(interceptorType, interceptor, (m, i) => interceptor);
                    }

                    interceptors.Add(interceptorType, _interceptorMap[interceptorType]);
                }
            }
        }
        #endregion

        #region 内部クラス

        private class CastleInterceptorAdapter : IInterceptor
        {
            private readonly int _ordinal;
            private readonly IMethodInterceptor _interceptor;

            public CastleInterceptorAdapter(IMethodInterceptor interceptor, int ordinal)
            {
                _ordinal = ordinal;
                _interceptor = interceptor;
            }

            public void Intercept(IInvocation invocation)
            {
                invocation.ReturnValue = _interceptor.Invoke(new CastleInvocationAdapter(invocation));
            }
        }

        /// <summary>
        /// SeasarのIMethodInvocationとCastle.DynamicProxyのIInvocationを繋ぐAdapterクラス
        /// </summary>
        private class CastleInvocationAdapter : IMethodInvocation
        {
            private readonly IInvocation _invocation;

            public CastleInvocationAdapter(IInvocation invocation)
            {
                _invocation = invocation;
            }

            public MethodBase Method
            {
                get { return _invocation.Method; }
            }

            public object Target
            {
                get { return _invocation.InvocationTarget; }
            }

            public object[] Arguments
            {
                get { return _invocation.Arguments; }
            }

            public object Proceed()
            {
                _invocation.Proceed();
                return _invocation.ReturnValue;
            }
        }

        /// <summary>
        /// 適用するInterceptorを選択するクラス
        /// </summary>
        private class InterceptorSelectorImpl : IInterceptorSelector
        {
            public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Proxy生成時に特定のイベントをフックするクラス
        /// </summary>
        private class ProxyGenerationHookImpl : IProxyGenerationHook
        {
            private static readonly ConcurrentDictionary<MethodInfo, bool> _shouldInterceptMethodMap
                = new ConcurrentDictionary<MethodInfo, bool>();
            private readonly ILogger _logger = QuillConfig.GetInstance().LoggerFactory.GetLogger(typeof(CastleDynamicProxyCreator));
            public Type TargetType { get; set; }

            public void MethodsInspected()
            {
                // TODO メッセージ化
                _logger.Debug(string.Format("Interceptorの適用が完了しました。：{0}", TargetType.FullName));
            }

            public void NonProxyableMemberNotification(Type type, System.Reflection.MemberInfo memberInfo)
            {
                // TODO メッセージ化
                _logger.Debug(string.Format("[virual]ではないため、Interceptorを適用できません。：{0}#{1}",
                    type.FullName, memberInfo.Name));
            }

            public bool ShouldInterceptMethod(Type type, System.Reflection.MethodInfo methodInfo)
            {
                if (_shouldInterceptMethodMap.ContainsKey(methodInfo))
                {
                    bool hasAttr = methodInfo.HasAttribute<AspectAttribute>();
                    _shouldInterceptMethodMap.AddOrUpdate(methodInfo, hasAttr, (m, f) => hasAttr);
                }
                return _shouldInterceptMethodMap[methodInfo];
            }
        }
        #endregion
    }
}
