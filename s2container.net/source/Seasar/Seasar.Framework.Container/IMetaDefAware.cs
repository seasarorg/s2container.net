#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container
{
    /// <summary>
    /// IMetaDefの設定が可能になります。
    /// </summary>
    public interface IMetaDefAware
    {
        /// <summary>
        /// IMetaDefを追加します。
        /// </summary>
        /// <param name="metaDef">IMetaDef</param>
        void AddMetaDef(IMetaDef metaDef);

        /// <summary>
        /// IMetaDefの数
        /// </summary>
        int MetaDefSize { get;}

        /// <summary>
        /// 番号を指定してIMetaDefを取得します。
        /// </summary>
        /// <param name="index">IMetaDefの番号</param>
        /// <returns>IMetaDef</returns>
        IMetaDef GetMetaDef(int index);

        /// <summary>
        /// 名前を指定してIMetaDefを取得します。
        /// </summary>
        /// <param name="name">IMetaDefの名前</param>
        /// <returns>IMetaDef</returns>
        IMetaDef GetMetaDef(string name);

        /// <summary>
        /// 名前を指定してIMetaDefの配列を取得します。
        /// </summary>
        /// <param name="name">IMetaDefの名前</param>
        /// <returns>IMetaDefの配列</returns>
        IMetaDef[] GetMetaDefs(string name);
    }
}
