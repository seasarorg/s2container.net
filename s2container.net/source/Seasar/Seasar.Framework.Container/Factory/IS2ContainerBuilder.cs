#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container.Factory
{
    /// <summary>
    /// IS2Containerを構築します。
    /// </summary>
    public interface IS2ContainerBuilder
    {
        /// <summary>
        /// 指定されたパスからS2Containerを構築します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>構築したS2Container</returns>
        IS2Container Build(string path);

        /// <summary>
        /// 指定されたパスからS2Containerを構築し、
        /// 親S2Containerにincludeします。
        /// </summary>
        /// <param name="parent">親S2Container</param>
        /// <param name="path">パス</param>
        /// <returns>構築したS2Container</returns>
        IS2Container Include(IS2Container parent, string path);
    }
}
