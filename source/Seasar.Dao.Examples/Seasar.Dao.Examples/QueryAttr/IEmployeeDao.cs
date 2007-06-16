#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using System.Collections;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples.QueryAttr
{
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        /// <summary>
        /// ]‹Æˆõ–¼‚Ì¸‡‚É•À‚Ñ‘Ö‚¦‚½‘S‚Ä‚Ì]‹Æˆõ‚ğæ“¾‚µ‚Ü‚·B
        /// </summary>
        /// <returns>]‹Æˆõ‚ÌƒŠƒXƒg</returns>
        [Query("order by ename asc")]
        IList GetAllListEnameAsc();

        /// <summary>
        /// ]‹Æˆõ–¼‚©‚ç]‹Æˆõ‚ğæ“¾‚µ‚Ü‚·B
        /// </summary>
        /// <param name="ename">]‹Æˆõ–¼</param>
        /// <returns>]‹Æˆõ</returns>
        [Query("ename=/*ename*/")]
        Employee GetEmployeeByEname(string ename);
    }
}
