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
using System.Collections.Generic;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;
using Seasar.Framework.Log;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// �R���|�[�l���g���i�[����R���e�i�N���X
    /// </summary>
    /// <remarks>
    /// <para>
    /// �i�[����R���|�[�l���g�̃C���X�^���X��1�x����������
    /// �������̂��g�p�����(singleton)</para>
    /// </remarks>
    public class QuillContainer : IDisposable
    {
        /// <summary>
        /// ���O
        /// </summary>
        private static readonly Logger _log = Logger.GetLogger(typeof(QuillContainer));

        // �쐬�ς݂ɃR���|�[�l���g���i�[����
        protected IDictionary<Type, QuillComponent> components =
            new Dictionary<Type, QuillComponent>();

        // Aspect���\�z����Builder
        protected AspectBuilder aspectBuilder;

        /// <summary>
        /// QuillContainer�̏��������s���R���X�g���N�^
        /// </summary>
        public QuillContainer()
        {
            // QuillContainer���Ŏg�p����AspectBuilder���쐬����
            aspectBuilder = new AspectBuilder(this);

            //  Quill�ݒ���̏�����
            QuillConfig.InitializeQuillConfig(this);
            QuillConfig config = QuillConfig.GetInstance();
            LogUtil.Output(_log, "IQLL0003", config.HasQuillConfig());
            if (config.HasQuillConfig())
            {
                //  �ݒ��񂪂���ꍇ�̓A�Z���u���A�f�[�^�\�[�X��o�^
                config.RegisterAssembly();
                RegistDataSource(
                    config.CreateDataSources(),
                    config.GetTransationSettingType());
            }
        }

        /// <summary>
        /// Quill�R���|�[�l���g���擾����
        /// </summary>
        /// <remarks>
        /// <para>�C���X�^���X�̎󂯑���Type�ƃC���X�^���X��Type�������ꍇ��
        /// QuillComponent���擾����B</para>
        /// <para>Quill�R���|�[�l���g�������ς݂̏ꍇ�͐����ς݂�
        /// Quill�R���|�[�l���g��Ԃ��B</para>        
        /// </remarks>
        /// <param name="type">�C���X�^���X�̎󂯑���Type</param>
        /// <returns>Quill�R���|�[�l���g</returns>
        public virtual QuillComponent GetComponent(Type type)
        {
            // Quill�R���|�[�l���g���擾���ĕԂ�
            return GetComponent(type, type);
        }

        /// <summary>
        /// Quill�R���|�[�l���g���擾����
        /// </summary>
        /// <remarks>
        /// Quill�R���|�[�l���g�������ς݂̏ꍇ�͐����ς݂�
        /// Quill�R���|�[�l���g��Ԃ��B
        /// </remarks>
        /// <param name="type">�C���X�^���X�̎󂯑���Type</param>
        /// <param name="implType">�C���X�^���X��Type</param>
        /// <returns>Quill�R���|�[�l���g</returns>
        public virtual QuillComponent GetComponent(Type type, Type implType)
        {
            if (components == null)
            {
                // Destroy����Ă���ꍇ�͗�O�𔭐�����
                throw new QuillApplicationException("EQLL0018");
            }

            lock (components)
            {
                // ���ɍ쐬�ς݂̃C���X�^���X�ł��邩�m�F����
                if (components.ContainsKey(type))
                {
                    // ���ɍ쐬�ς݂ł���΍쐬�ς݂̃C���X�^���X��Ԃ�
                    return components[type];
                }

                if (components.ContainsKey(implType))
                {
                    // ���ɍ쐬�ς݂ł���΍쐬�ς݂̃C���X�^���X��Ԃ�
                    components[type] = components[implType];
                    return components[implType];
                }

                // Aspect���쐬����iAspect�������w�肳��Ă��Ȃ���΃T�C�Y0�ƂȂ�)
                IAspect[] aspects = aspectBuilder.CreateAspects(implType);

                if (implType.IsInterface && aspects.Length == 0)
                {
                    // Interface��Aspect����`����Ă��Ȃ��ꍇ�͗�O���X���[����
                    throw new QuillApplicationException("EQLL0008",
                        new string[] { implType.FullName });
                }

                // Quill�R���|�[�l���g���쐬����
                QuillComponent component = new QuillComponent(implType, type, aspects);

                // �쐬�ς݂�Quill�R���|�[�l���g��ۑ�����
                components[type] = component;
                if (type.GetType().FullName != implType.GetType().FullName)
                {
                    components[implType] = component;
                }

                // Quill�R���|�[�l���g��Ԃ�
                return component;
            }
        }

        /// <summary>
        /// QuillContainer�����Q�Ƃ�j������
        /// </summary>
        public virtual void Destroy()
        {
            if (components == null)
            {
                return;
            }

            // �ێ����Ă���QuillComponent�𔽕���������ׂ̗񋓎q���擾����
            IEnumerator<QuillComponent> componentValues =
                components.Values.GetEnumerator();

            while (componentValues.MoveNext())
            {
                // QuillComponent��Destroy���Ăяo��
                componentValues.Current.Destroy();
            }

            components = null;
            aspectBuilder = null;
        }

        /// <summary>
        /// �f�[�^�\�[�X��o�^
        /// </summary>
        public void RegistDataSource(IDictionary<string, IDataSource> dataSources,
            Type defaultTxSettingType)
        {
            // �f�[�^�\�[�X�̒�`���Ȃ���ΈȌ�̏����͍s��Ȃ�
            if (dataSources.Count == 0)
            {
                return;
            }
            //  Quill�p�f�[�^�\�[�X�̐���
            SelectableDataSourceProxyWithDictionary dataSourceProxy =
                (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                this, typeof(SelectableDataSourceProxyWithDictionary));
            //  �f�[�^�\�[�X�̒�`������Γo�^
            foreach (KeyValuePair<string, IDataSource> dataSourcePair in dataSources)
            {
                dataSourceProxy.RegistDataSource(dataSourcePair.Key, dataSourcePair.Value);
            }

            ITransactionSetting defaultTxSetting = (ITransactionSetting)ComponentUtil.GetComponent(
                this, defaultTxSettingType);
            //  �g�����U�N�V�����̃f�t�H���g�ݒ���s��
            if (defaultTxSetting != null && defaultTxSetting.IsNeedSetup())
            {
                defaultTxSetting.Setup(dataSourceProxy);
            }
        }
        
        #region IDisposable �����o

        /// <summary>
        /// �ێ����Ă���QuillComponent��Dispose���Ăяo��
        /// </summary>
        public virtual void Dispose()
        {
            if (components == null)
            {
                // Destroy����Ă���ꍇ�͗�O�𔭐�����
                throw new QuillApplicationException("EQLL0018");
            }

            // �ێ����Ă���QuillComponent�𔽕���������ׂ̗񋓎q���擾����
            IEnumerator<QuillComponent> componentValues = 
                components.Values.GetEnumerator();

            while (componentValues.MoveNext())
            {
                // QuillComponent��Dispose���Ăяo��
                componentValues.Current.Dispose();
            }
        }

        #endregion
    }
}
