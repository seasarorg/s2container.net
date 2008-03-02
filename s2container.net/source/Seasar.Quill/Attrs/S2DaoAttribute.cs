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
using Seasar.Quill.Dao;
using Seasar.Quill.Dao.Impl;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using Seasar.Quill.Xml;

namespace Seasar.Quill.Attrs
{
    /// <summary>
    /// 属性を使ってDaoInterceptorをかけるための属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface |
       AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class S2DaoAttribute : Attribute
    {
        private Type _daoSettingType = null;

        public virtual Type DaoSettingType
        {
            get
            {
                return _daoSettingType;
            }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// (S2DaoInterceptorを使います)
        /// </summary>
        public S2DaoAttribute()
        {
            QuillSection section = QuillSectionHandler.GetQuillSection();
            if (section == null || string.IsNullOrEmpty(section.DaoSetting))
            {
                //  設定がない場合は既定のDao設定を使う
                SetSettingType(typeof(TypicalDaoSetting));
            }
            else
            {
                string typeName = section.DaoSetting;
                if (TypeUtil.HasNamespace(typeName) == false)
                {
                    //  名前空間の指定がなければ既定の名前空間を使う
                    typeName = string.Format("{0}.{1}",
                        QuillConstants.NAMESPACE_DAOSETTING, typeName);
                }
                Type settingType = ClassUtil.ForName(typeName);
                SetSettingType(settingType);
            }
        }

        /// <summary>
        /// カスタムコンストラクタ
        /// (指定したDao設定クラスを使います)
        /// (IDaoSetting実装クラスではない場合実行時例外を投げます）
        /// </summary>
        /// <param name="handlerType"></param>
        public S2DaoAttribute(Type settingType)
        {
            SetSettingType(settingType);
        }

        /// <summary>
        /// 使用するTxHandlerの設定
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="">S2DaoInterceptorサブクラスでないとき</exception>
        protected virtual void SetSettingType(Type type)
        {
            if (typeof(IDaoSetting).IsAssignableFrom(type))
            {
                _daoSettingType = type;
            }
            else
            {
                throw new QuillApplicationException("EQLL0025", type.Name);
            }
        }
    }
}
