#region Copyright
/*
* Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Reflection;
using Castle.DynamicProxy;
using Seasar.Framework.Aop.Impl;

#endregion

namespace Seasar.Framework.Aop.Proxy
{
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
    {
        #region fields

        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private readonly IAspect[] _aspects;
        private readonly Hashtable _interceptors = new Hashtable();
        private readonly Type _type;
        private readonly Type _enhancedType;
        private readonly Hashtable _parameters;

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

        #region properties

        /// <summary>
        /// Proxy�ɂ��g�����ꂽ�^���擾����v���p�e�B
        /// </summary>
        /// <value>Type �g�����ꂽ�^</value>
        public Type EnhancedType
        {
            get { return _enhancedType; }
        }

        #endregion

        #region public method

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

        #region private methods

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
    }
}
