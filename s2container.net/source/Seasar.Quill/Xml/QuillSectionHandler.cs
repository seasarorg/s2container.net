﻿#region Copyright
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

using System.Configuration;
using System.Xml;

namespace Seasar.Quill.Xml
{
    /// <summary>
    /// Quillの構成セクションハンドラクラスです。
    /// </summary>
    public class QuillSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Quill設定情報を取得
        /// </summary>
        /// <returns>Quill設定情報(設定ファイルがなければnull)</returns>
        public static QuillSection GetQuillSection()
        {
            //  App.config上の設定を取得
            var quillSection = (QuillSection)ConfigurationManager.GetSection(QuillConstants.QUILL_CONFIG);
            return quillSection;
        }

        #region IConfigurationSectionHandler メンバ

        public object Create(object parent, object configContext, XmlNode section) 
            => QuillSectionLoader.CreateQuillSection(section);

        #endregion
    }
}
