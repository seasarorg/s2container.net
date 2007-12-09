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

using System;

namespace Seasar.Dao.Attrs
{
    /// <summary>
    /// PROCEDURE属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ProcedureAttribute : Attribute
    {
        /// <summary>
        /// プロシージャ名
        /// </summary>
        private readonly string _procedureName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="procedureName">プロシージャ名</param>
        public ProcedureAttribute(string procedureName)
        {
            _procedureName = procedureName;
        }

        /// <summary>
        /// プロシージャ名
        /// </summary>
        public string ProcedureName
        {
            get { return _procedureName; }
        }
    }
}