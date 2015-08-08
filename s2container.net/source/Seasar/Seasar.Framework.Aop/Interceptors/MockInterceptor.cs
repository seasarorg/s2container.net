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

using System;
using System.Collections;

namespace Seasar.Framework.Aop.Interceptors
{
    /// <summary>
    /// Intercept�̑ΏۂƂȂ郁�\�b�h�ƃC���X�^���X��Mock�ɂ��܂�
    /// </summary>
    public class MockInterceptor : AbstractInterceptor
    {
        /// <summary>
        /// Mock�̖߂�l
        /// </summary>
        private readonly Hashtable _returnValues = new Hashtable();

        /// <summary>
        /// Mock�̗�O
        /// </summary>
        private readonly Hashtable _exceptions = new Hashtable();

        /// <summary>
        /// ���\�b�h���Ăяo���ς݂��ǂ��������t���O
        /// </summary>
        private readonly Hashtable _invokedMethods = new Hashtable();

        /// <summary>
        /// ���\�b�h��Ăяo�����Ƃ��̈���
        /// </summary>
        private readonly Hashtable _invokedMethodArgs = new Hashtable();

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public MockInterceptor()
        {
        }

        #region IMethodInterceptor �N���X

        /// <summary>
        /// ���\�b�h��Intercept�����ꍇ�A���̃��\�b�h���Ăяo����܂�
        /// </summary>
        /// <param name="invocation">IMethodInvocation</param>
        /// <returns>Intercept����郁�\�b�h�̖߂�l</returns>
        public override object Invoke(IMethodInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            _invokedMethods[methodName] = true;
            _invokedMethodArgs[methodName] = invocation.Arguments;

            if (_exceptions.ContainsKey(methodName))
            {
                throw (Exception) _exceptions[methodName];
            }
            else if (_exceptions.ContainsKey(string.Empty))
            {
                throw (Exception) _exceptions[string.Empty];
            }
            else if (_returnValues.ContainsKey(methodName))
            {
                return _returnValues[methodName];
            }
            else
            {
                return _returnValues[string.Empty];
            }
        }

        #endregion

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="value">Mock�̖߂�l</param>
        public MockInterceptor(object value)
        {
            SetReturnValue(value);
        }

        /// <summary>
        /// �߂�l�̐ݒ�
        /// </summary>
        /// <remarks>
        /// Mock�̂��ׂẴ��\�b�h�̖߂�l��ݒ肵�܂��B
        /// </remarks>
        /// <param name="returnValue">�߂�l</param>
        public void SetReturnValue(object returnValue)
        {
            SetReturnValue(string.Empty, returnValue);
        }

        /// <summary>
        /// �߂�l�̐ݒ�
        /// </summary>
        /// <remarks>
        /// Mock�̎w�肳�ꂽ���O�̃��\�b�h�̖߂�l��ݒ肵�܂��B
        /// </remarks>
        /// <param name="methodName">�߂�l��ݒ肷�郁�\�b�h��</param>
        /// <param name="returnValue">�߂�l</param>
        public void SetReturnValue(string methodName, object returnValue)
        {
            _returnValues[methodName] = returnValue;
        }

        /// <summary>
        /// ��O�̐ݒ�
        /// </summary>
        /// <remarks>
        /// Mock�̂��ׂẴ��\�b�h�ɗ�O��ݒ肵�܂��B
        /// </remarks>
        /// <param name="exception">��O</param>
        public void SetThrowable(Exception exception)
        {
            SetThrowable(string.Empty, exception);
        }

        /// <summary>
        /// ��O�̐ݒ�
        /// </summary>
        /// <remarks>
        /// Mock�̂��ׂẴ��\�b�h�ɗ�O��ݒ肵�܂��B
        /// </remarks>
        /// <param name="methodName">��O��ݒ肷�郁�\�b�h��</param>
        /// <param name="exception">��O</param>
        public void SetThrowable(string methodName, Exception exception)
        {
            _exceptions[methodName] = exception;
        }

        /// <summary>
        /// Mock�̃��\�b�h�����ɌĂяo����Ă��邩���肵�܂�
        /// </summary>
        /// <param name="methodName">�Ăяo����Ă��邩�ǂ������肷�郁�\�b�h��</param>
        /// <returns>Mock�̃��\�b�h�����ɌĂяo����Ă��邩</returns>
        public bool IsInvoked(string methodName) => _invokedMethods.ContainsKey(methodName);

        /// <summary>
        /// Mock�̃��\�b�h���Ă΂ꂽ�Ƃ��̈�����擾���܂�
        /// </summary>
        /// <param name="methodName">������擾���郁�\�b�h��</param>
        /// <returns>�����̃��X�g</returns>
        public object[] GetArgs(string methodName) => (object[]) _invokedMethodArgs[methodName];
    }
}
