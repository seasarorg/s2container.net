#region Copyright
/*
* Copyright 2005-2015 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using Seasar.Framework.Aop.Impl;

#endregion

namespace Seasar.Framework.Aop.Proxy
{
#if NET_4_0    
    /// <summary>
    /// Castle.DynamicProxy���g�p�����AAspect���s�̂��߂̃v���L�V�N���X
    /// </summary>
    [Serializable]
    public class DynamicAopProxy
#else
    #region NET2.0
    /// <summary>
    /// Castle.DynamicProxy���g�p�����AAspect���s�̂��߂̃v���L�V�N���X
    /// </summary>
    /// <author>Kazz
    /// </author>
    /// <remarks>edited Kazuya Sugimoto</remarks>
    /// <version>1.7.2 2006/07/24</version>
    ///
    [Serializable]
    public class DynamicAopProxy : IInterceptor
#endregion
#endif
    {
        #region fields
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

#if NET_4_0
        private readonly Type _componentType;
        private readonly IInterceptor[] _interceptors;
#else
#region NET2.0
        private readonly IAspect[] _aspects;
        private readonly Hashtable _interceptors = new Hashtable();
        private readonly Type _type;
        private readonly Type _enhancedType;
        private readonly Hashtable _parameters;
#endregion
#endif
        #endregion

        #region constructors

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="type">Aspect���K�p�����^</param>
        public DynamicAopProxy(Type type)
            : this(type, null)
        {
        }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="type">Aspect���K�p�����^</param>
        /// <param name="aspects">�K�p����Aspect�̔z��</param>
        public DynamicAopProxy(Type type, IAspect[] aspects)
            : this(type, aspects, null)
        {
        }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="type">Aspect���K�p�����^</param>
        /// <param name="aspects">�K�p����Aspect�̔z��</param>
        /// <param name="parameters">�p�����[�^</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters)
            : this(type, aspects, parameters, null)
        {
        }

#if NET_4_0
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="componentType">Aspect���K�p�����^</param>
        /// <param name="aspects">�K�p����Aspect�̔z��</param>
        /// <param name="parameters">�p�����[�^</param>
        /// <param name="target">Aspect���K�p�����^�[�Q�b�g</param>
        public DynamicAopProxy(Type componentType, IAspect[] aspects, Hashtable parameters, object target)
        {
            _componentType = componentType;

            var interceptorMap = CreateMethodInterceptors(componentType, aspects);
            _interceptors = new IInterceptor[] { new InterceptorAdapter(
                interceptorMap, componentType, parameters) };
        }
#else
#region NET2.0
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="type">Aspect���K�p�����^</param>
        /// <param name="aspects">�K�p����Aspect�̔z��</param>
        /// <param name="parameters">�p�����[�^</param>
        /// <param name="target">Aspect���K�p�����^�[�Q�b�g</param>
        public DynamicAopProxy(Type type, IAspect[] aspects, Hashtable parameters, object target)
        {
            _type = type;
            _aspects = aspects;
            _parameters = parameters;
            _generator = new ProxyGenerator();

            if (_type.IsInterface)
            {
                if (target == null)
                {
                    target = new object();
                }
                _enhancedType = _generator.ProxyBuilder.CreateInterfaceProxy(new Type[] { _type }, target.GetType());
            }
            else
            {
                _enhancedType = _generator.ProxyBuilder.CreateClassProxy(_type);
            }
            SetUpAspects();
        }
#endregion
#endif

        #endregion

        #region public method

#if NET_4_0
        /// <summary>
        /// �v���L�V�I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        public object Create()
        {
            if (_componentType.IsInterface)
            {
                return _generator.CreateInterfaceProxyWithoutTarget(_componentType, _interceptors);
            }
            else
            {
                return _generator.CreateClassProxy(_componentType, _interceptors);
            }
        }

        public object Create(Type receiptType, object target)
        {
            if (receiptType.IsInterface && target.GetType() == typeof(object))
            {
                return _generator.CreateInterfaceProxyWithoutTarget(receiptType, _interceptors);
            }
            return _generator.CreateClassProxy(_componentType, _interceptors);
        }

        public object Create(Type[] types, object[] args)
        {            
            var interceptorTypes = new List<Type>();
            foreach(var interceptor in _interceptors)
            {
                interceptorTypes.Add(interceptor.GetType());
            }

            Type enhancedType;
            if (_componentType.IsInterface)
            {
                enhancedType = _generator.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(_componentType, interceptorTypes.ToArray(), ProxyGenerationOptions.Default);
            }
            else
            {
                enhancedType = _generator.ProxyBuilder.CreateClassProxyType(_componentType, interceptorTypes.ToArray(), ProxyGenerationOptions.Default);
            }

            var newArgs = new ArrayList();
            newArgs.Add(_interceptors);
            newArgs.AddRange(args);
            return Activator.CreateInstance(enhancedType, newArgs.ToArray());
        }

        public T Create<T>(object target)
        {
            return (T)Create(typeof(T), target);
        }
#else
#region NET2.0
        /// <summary>
        /// �v���L�V�I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        public object Create()
        {
            if (_type.IsInterface)
            {
                return Create(new object());
            }
            else
            {
                return Activator.CreateInstance(_enhancedType, new object[] { this });
            }
        }

        /// <summary>
        /// �v���L�V�I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        public object Create(object target)
        {
            return Create(_type, target);
        }

        public object Create(Type type, object target)
        {
            ArrayList args = new ArrayList();
            args.Add(this);
            if (_type.IsInterface)
            {
                args.AddRange(new object[] { null });
            }
            if (type.IsInterface && target.GetType() != typeof(object))
            {
                return _generator.CreateProxy(type, this, target);
            }
            else
            {
                return Activator.CreateInstance(_enhancedType, args.ToArray());
            }
        }

        /// <summary>
        /// �v���L�V�I�u�W�F�N�g�𐶐����܂�    
        /// </summary>
        /// <param name="argTypes">�p�����^�^�̔z��</param>
        /// <param name="args">�������̃p�����^�̔z��</param>
        public object Create(Type[] argTypes, object[] args)
        {
            ArrayList newArgs = new ArrayList();
            newArgs.Add(this);
            newArgs.AddRange(args);
            return Activator.CreateInstance(_enhancedType, newArgs.ToArray());
        }

        /// <summary>
        /// �v���L�V�I�u�W�F�N�g�𐶐����܂�
        /// </summary>
        /// <param name="argTypes">�p�����^�^�̔z��</param>
        /// <param name="args">�������̃p�����^�̔z��</param>
        /// <param name="targetType">�^�[�Q�b�g�̌^</param>
        public object Create(Type[] argTypes, object[] args, Type targetType)
        {
            if (_type.IsInterface)
            {
                return _generator.CreateProxy(targetType, this, args);
            }
            else
            {
                return _generator.CreateClassProxy(targetType, this, args);
            }
        }
#endregion
#endif

        #endregion

#if NET_4_0
        // �����Ȃ�
#else
#region NET2.0
        #region IInterceptor member

        public object Intercept(IInvocation invocation, params object[] args)
        {
            object ret;
            if ((invocation.Proxy == invocation.InvocationTarget ||
                !(invocation.Method.IsVirtual && !invocation.Method.IsFinal)) &&
                _interceptors.ContainsKey(invocation.Method))
            {
                IMethodInterceptor[] interceptors = _interceptors[invocation.Method] as IMethodInterceptor[];
                IMethodInvocation mehotdInvocation =
                   new DynamicProxyMethodInvocation(invocation.InvocationTarget, _type, invocation, args, interceptors, _parameters);
                ret = interceptors[0].Invoke(mehotdInvocation);

            }
            else
            {
                ret = invocation.Proceed(args);
            }
            return ret;
        }

        #endregion
#endregion
#endif

        #region private methods
#if NET_4_0
        /// <summary>
        /// �A�X�y�N�g���Z�b�g�A�b�v���܂�
        /// </summary>
        /// <param name="type"></param>
        /// <param name="aspects"></param>
        private IDictionary<MethodInfo, IMethodInterceptor[]> CreateMethodInterceptors(
            Type type, IAspect[] aspects)
        {
            var interceptorMap = new Dictionary<MethodInfo, IMethodInterceptor[]>();
            if (aspects != null)
            {
                var methodInfos = type.GetMethods();
                foreach (var method in methodInfos)
                {
                    // DynamicProxy��K�p�����邽�߂ɂ̓C���^�[�t�F�[�X��virtual���\�b�h
                    // �łȂ���΂Ȃ�Ȃ�
                    if (method.IsVirtual || type.IsInterface)
                    {
                        var interceptorList = new List<IMethodInterceptor>();
                        foreach (var aspect in aspects)
                        {
                            var pointcut = aspect.Pointcut;
                            if (pointcut == null || pointcut.IsApplied(method))
                            {
                                interceptorList.Add(aspect.MethodInterceptor);
                            }
                        }
                        if (interceptorList.Count > 0)
                        {
                            IMethodInterceptor[] interceptors = interceptorList.ToArray();
                            interceptorMap[method] = interceptors;
                        }
                    }
                }
            }
            
            return interceptorMap;
        }
#else
#region NET2.0
        /// <summary>
        /// �A�X�y�N�g���Z�b�g�A�b�v���܂�
        /// </summary>
        private void SetUpAspects()
        {
            if (_aspects != null)
            {
                MethodInfo[] methodInfos = _type.GetMethods();
                foreach (MethodInfo method in methodInfos)
                {
                    if (method.IsVirtual || _type.IsInterface)
                    {
                        ArrayList interceptorList = new ArrayList();
                        foreach (IAspect aspect in _aspects)
                        {
                            IPointcut pointcut = aspect.Pointcut;
                            if (pointcut == null || pointcut.IsApplied(method))
                            {
                                interceptorList.Add(aspect.MethodInterceptor);
                            }
                        }
                        if (interceptorList.Count > 0)
                        {
                            IMethodInterceptor[] interceptors = (IMethodInterceptor[])
                            interceptorList.ToArray(typeof(IMethodInterceptor));
                            _interceptors.Add(method, interceptors);
                        }
                    }
                }
            }
        }
#endregion
#endif
        #endregion
    }
}
