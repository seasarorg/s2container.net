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

        

        private IInterceptor[] GetInterceptors(Type target)
        {
            var aspectAttributes = new HashSet<AspectAttribute>();
            AddAspectAttributes(aspectAttributes, target);
            foreach (var member in target.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                AddAspectAttributes(aspectAttributes, member);
            }

            var interceptors = new Dictionary<Type, IInterceptor>();
            foreach(var aspectAttr in aspectAttributes)
            {
                if (!interceptors.ContainsKey(aspectAttr.InterceptorType))
                {
                    interceptors.Add(target, GetInterceptor(aspectAttr));
                }
            }
            return interceptors.Values.ToArray();
        }

        private static void AddAspectAttributes<ATTR>(ISet<ATTR> aspectAttributes, MemberInfo target) where ATTR : AspectAttribute
        {
            var attrs = target.GetCustomAttributes<ATTR>();
            if (attrs == null || attrs.Count() == 0)
            {
                return;
            }

            foreach (var attr in attrs)
            {
                if (!aspectAttributes.Contains(attr))
                {
                    aspectAttributes.Add(attr);
                }
            }
        }

        private IInterceptor GetInterceptor(AspectAttribute aspect)
        {
            var interceptorType = aspect.InterceptorType;
            if (!_interceptorMap.ContainsKey(interceptorType))
            {
                var interceptor = new CastleInterceptorAdapter(
                    (IMethodInterceptor)_injector.CreateInjectedInstance(interceptorType), aspect.Ordinal);
                _interceptorMap.AddOrUpdate(interceptorType, interceptor, (m, i) => interceptor);
            }
            return _interceptorMap[interceptorType];
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
            public int Ordinal { get { return _ordinal; } }

            private readonly IMethodInterceptor _interceptor;
            public IMethodInterceptor Interceptor { get { return _interceptor; } }

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
                var aspectAttrs = new HashSet<AspectAttribute>();
                AddAspectAttributes(aspectAttrs, type);
                AddAspectAttributes(aspectAttrs, method);
                // メソッドに関連づけられているInterceptorの型セットを取り出す
                var targetInterceptorTypes = aspectAttrs.Select(attr => attr.InterceptorType);

                CastleInterceptorAdapter[] adapters = (CastleInterceptorAdapter[])interceptors;
                // メソッドに適用するInterceptorを抽出、ソート
                var appliedInterceptors = adapters.Where(i => targetInterceptorTypes.Contains(i.Interceptor.GetType())).OrderBy(i => i.Ordinal);
                return appliedInterceptors.ToArray();
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
