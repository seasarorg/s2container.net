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
using Seasar.Quill.Util;

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
        /// (標準の設定を使います)
        /// </summary>
        public TransactionAttribute()
        {
            _transactionSettingType = SettingUtil.GetTransationSettingType();
            SettingUtil.ValidateTransactionSettingType(_transactionSettingType);
        }

        /// <summary>
        /// カスタムコンストラクタ
        /// (指定した設定を使います)
        /// (ITransactionSettingクラスではない場合実行時例外を投げます）
        /// </summary>
        /// <param name="handlerType"></param>
        public TransactionAttribute(Type settingType)
        {
            SettingUtil.ValidateTransactionSettingType(settingType);
            _transactionSettingType = settingType;
        }
    }
}
