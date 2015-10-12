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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using Seasar.Quill.Exception;

namespace Seasar.Quill
{
    /// <summary>
    /// QuillContainer�Ɋi�[����R���|�[�l���g��`�N���X
    /// </summary>
    /// <remarks>
    /// �R���X�g���N�^�ŏ��������s���ۂɃR���|�[�l���g�̃I�u�W�F�N�g
    /// ���C���X�^���X������
    /// </remarks>
    public class QuillComponent : IDisposable
    {
        /// <summary>
        /// �R���|�[�l���g��Type
        /// </summary>
        protected Type componentType;

        /// <summary>
        /// �R���|�[�l���g���󂯎��t�B�[���h��Type
        /// </summary>
        protected Type receiptType;

        /// <summary>
        /// �R���|�[�l���g��Object���i�[����R���N�V����
        /// </summary>
        /// <remarks>
        /// Aspect���K�p�����ꍇ�ɂ̓v���L�V�I�u�W�F�N�g���i�[����
        /// </remarks>
        protected Dictionary<Type, object> componentObjects = 
            new Dictionary<Type, object>(2);

        #region �v���p�e�B

        /// <summary>
        /// �R���|�[�l���g��Type���擾����
        /// </summary>
        /// <value>�R���|�[�l���g��Type</value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// �R���|�[�l���g���󂯎��t�B�[���h��Type���擾����
        /// </summary>
        /// <value>�R���|�[�l���g���󂯎��t�B�[���h��Type</value>
        public Type ReceiptType
        {
            get { return receiptType; }
        }

        #endregion

        #region �R���X�g���N�^

        /// <summary>
        /// QuillComponent������������R���X�g���N�^
        /// </summary>
        /// <param name="componentType">�R���|�[�l���g��Type</param>
        /// <param name="receiptType">�R���|�[�l���g���󂯎��t�B�[���h��Type</param>
        /// <param name="aspects">Aspect��`�̔z��</param>
        public QuillComponent(Type componentType, Type receiptType, IAspect[] aspects)
        {
            // �R���|�[�l���g��Type���t�B�[���h�ɃZ�b�g����
            this.componentType = componentType;

            // �R���|�[�l���g���󂯎��t�B�[���h��Type���t�B�[���h�ɃZ�b�g����
            this.receiptType = receiptType;

            if (aspects.Length > 0)
            {
                // Aspect��������`����Ă���ꍇ��ProxyObject���쐬����
                CreateProxyObject(componentType, receiptType, aspects);
            }
            else
            {
                // Aspect��������`����Ă��Ȃ��ꍇ�͎����N���X�̃C���X�^���X���쐬����
                CreateObject(componentType, receiptType);
            }
        }

        #endregion

        #region public ���\�b�h

        /// <summary>
        /// �R���|�[�l���g�̃I�u�W�F�N�g���擾����
        /// </summary>
        /// <param name="type">�R���|�[�l���g���󂯎��t�B�[���h��Type</param>
        /// <returns>
        /// �R���|�[�l���g�̃I�u�W�F�N�g
        /// <para>type�ɑΉ�����I�u�W�F�N�g���Ȃ��ꍇ��null��Ԃ�</para>
        /// </returns>
        public virtual object GetComponentObject(Type type)
        {
            if (componentObjects == null)
            {
                // Destroy����Ă���ꍇ�͗�O�𔭐�����
                throw new QuillApplicationException("EQLL0018");
            }

            if (componentObjects.ContainsKey(type))
            {
                // �Ή�����I�u�W�F�N�g���i�[���Ă���ꍇ�͕Ԃ�
                return componentObjects[type];
            }
            else
            {
                // �Ή�����I�u�W�F�N�g���i�[���Ă��Ȃ��ꍇ��null��Ԃ�
                return null;
            }
        }

        /// <summary>
        /// QuillComponent�����Q�Ƃ�j������
        /// </summary>
        public virtual void Destroy()
        {
            if (componentObjects == null)
            {
                return;
            }

            // �A���}�l�[�W���\�[�X���������
            Dispose();

            componentObjects[componentType] = null;

            if (componentType != null && !componentType.Equals(receiptType))
            {
                // �󂯑��̌^���قȂ�ꍇ�͎󂯑��̌^�Ŋi�[����Ă���I�u�W�F�N�g�ւ̎Q�Ƃ��j������
                componentObjects[receiptType] = null;
            }

            componentObjects = null;
            componentType = null;
            receiptType = null;
        }

        #region IDisposable �����o

        /// <summary>
        /// �R���|�[�l���g��IDisposable���������Ă���ꍇ��Dispose���Ăяo��
        /// </summary>
        public virtual void Dispose()
        {
            if (componentObjects == null)
            {
                // Destroy����Ă���ꍇ�͗�O�𔭐�����
                throw new QuillApplicationException("EQLL0018");
            }

            // IDisposable���������Ă���ꍇ�̓L���X�g����(�������Ă��Ȃ��ꍇ��null)
            IDisposable disposable = componentObjects[componentType] as IDisposable;

            if (disposable != null)
            {
                // IDisposable���������Ă���ꍇ��Dispose���Ăяo��
                disposable.Dispose();
            }
        }

        #endregion

        #endregion

        #region protected ���\�b�h

        /// <summary>
        /// �����N���X�̃C���X�^���X���쐬����
        /// </summary>
        /// <remarks>
        /// �C���X�^���X���쐬����N���X�ɂ̓p�����[�^(����)������public�ł���
        /// �R���X�g���N�^���K�v�Ƃ���
        /// </remarks>
        /// <param name="componentType">�R���|�[�l���g��Type</param>
        protected virtual void CreateObject(Type componentType, Type receiptType)
        {
            // �R���X�g���N�^���擾����
            ConstructorInfo constructor = componentType.GetConstructor(new Type[] { });

            if (constructor == null)
            {
                // �p�����[�^������public�ł���R���X�g���N�^���Ȃ��ꍇ�͗�O���X���[����
                throw new QuillApplicationException(
                    "EQLL0017", new object[] { componentType.FullName });
            }

            // �C���X�^���X���쐬����
            object obj;
            try
            {
                obj = Activator.CreateInstance(componentType);
            }
            catch (System.Exception ex)
            {
                throw new QuillApplicationException(
                    "EQLL0036", new object[] {componentType.Name}, ex); 
            }

            // �����N���X�̌^�Ŋi�[����
            componentObjects[componentType] = obj;

            if (!componentType.Equals(receiptType))
            {
                // �󂯑��̌^���قȂ�ꍇ�͎󂯑��̌^�Ŋi�[����
                componentObjects[receiptType] = obj;
            }
        }

        /// <summary>
        /// �v���L�V�I�u�W�F�N�g���쐬����
        /// </summary>
        /// <param name="componentType">�R���|�[�l���g��Type</param>
        /// <param name="receiptType">�R���|�[�l���g���󂯎��t�B�[���h��Type</param>
        /// <param name="aspects">�K�p����Aspect�̔z��</param>
        /// <returns>�쐬���ꂽ�v���L�V�I�u�W�F�N�g</returns>
        protected virtual void CreateProxyObject(
            Type componentType, Type receiptType, IAspect[] aspects)
        {
            // S2Container.NET�̃R���|�[�l���g��`���쐬����
            // �����S2Dao.NET��IComponentDef����Dao��Type���擾���Ă���̂�
            // �Ή�����ׁB(����A�����ƓK�؂ȕ��@�őΉ�����K�v����)
            IComponentDef componentDef = new ComponentDefImpl(componentType);

            // DynamicAopProxy�ɓn���ׂ̃p�����[�^
            Hashtable parameters = new Hashtable();

            // �R���|�[�l���g��`���p�����[�^�Ƃ��ăZ�b�g����
            parameters[ContainerConstants.COMPONENT_DEF_NAME] = componentDef;

            // DynamicAopProxy���쐬����
            DynamicAopProxy aopProxy;
            try
            {
                aopProxy = new DynamicAopProxy(componentType, aspects, parameters);

                // ProxyObject���쐬����
                componentObjects[componentType] = aopProxy.Create();
            }
            catch (System.Exception ex)
            {
                throw new QuillApplicationException(
                    "EQLL0037", new object[] { componentType.Name }, ex);
            }

            if (!componentType.Equals(receiptType))
            {
                // �R���|�[�l���g�̌^�ƃR���|�[�l���g���󂯎��t�B�[���h�̌^��
                // �قȂ�ꍇ�͎󂯎��ۂ̌^�ɑΉ�����ProxyObject��ݒ肷��
                componentObjects[receiptType] = componentObjects[componentType];
            }
        }

        #endregion
    }
}
