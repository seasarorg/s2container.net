#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Quill.Util;
using Seasar.Quill.Xml;
using Seasar.Quill.Exception;

namespace Seasar.Quill.Attrs
{
    /// <summary>
    /// トランザクション境界を指定するための属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface |
       AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class TransactionAttribute : Attribute
    {
        private Type _transactionSettingType = null;

        public virtual Type TransactionSettingType
        {
            get
            {
                return _transactionSettingType;
            }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// (QuillLocalRequiredTxInterceptorを使います)
        /// </summary>
        public TransactionAttribute()
        {
            QuillSection section = QuillSectionHandler.GetQuillSection();
            if (section == null || string.IsNullOrEmpty(section.TransactionSetting))
            {
                //  属性引数による指定もapp.configにも設定がなければ
                //  デフォルトのトランザクション設定を使う
                SetSettingType(typeof(TypicalTransactionSetting));
            }
            else
            {
                string typeName = section.TransactionSetting;
                if (TypeUtil.HasNamespace(typeName) == false)
                {
                    //  名前空間なしの場合は既定の名前空間から
                    typeName = string.Format("{0}.{1}",
                        QuillConstants.NAMESPACE_TXSETTING, typeName);
                }
                Type settingType = ClassUtil.ForName(typeName);
                    SetSettingType(settingType);
            }
        }

        /// <summary>
        /// カスタムコンストラクタ
        /// (指定したInterceptorを使います)
        /// (AbstractQuillTransactionInterceptorサブクラスではない場合実行時例外を投げます）
        /// </summary>
        /// <param name="handlerType"></param>
        public TransactionAttribute(Type settingType)
        {
            SetSettingType(settingType);
        }

        /// <summary>
        /// 使用するトランザクション設定クラスの設定
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="">ITransactionSetting実装クラスでないとき</exception>
        protected virtual void SetSettingType(Type type)
        {
            if(typeof(ITransactionSetting).IsAssignableFrom(type))
            {
                _transactionSettingType = type;
            }
            else
            {
                throw new QuillApplicationException("EQLL0026", type.Name);
            }
        }
    }
}
