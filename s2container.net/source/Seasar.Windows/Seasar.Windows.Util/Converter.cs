#region Copyright

/*
 * Copyright 2006 the Seasar Foundation and the Others.
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
 * 
 */

#endregion

using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace Seasar.Windows.Utils
{
    /// <summary>
    /// 変換用ユーティリティクラス
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Converter()
        {
            ;
        }

        /// <summary>
        /// PONOをDataSetに変換するクラス
        /// </summary>
        /// <param name="type">PONOクラス</param>
        /// <param name="list"></param>
        /// <returns>生成されたDataSet</returns>
        /// <remarks>DataSetに含まれるDataTableの名称はPONOクラス名</remarks>
        public static DataSet ConvertPONOToDataSet(Type type, IList list)
        {
            if ( type == null )
                throw new ArgumentNullException("type");
            if ( list == null )
                throw new ArgumentNullException("list");

            DataTable dt = new DataTable(type.Name);

            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                dt.Columns.Add(pi.Name, pi.GetType());
            }

            foreach (object bean in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo pi in pis)
                {
                    PropertyInfo p = bean.GetType().GetProperty(pi.Name);
                    row[pi.Name] = p.GetValue(bean, null);
                }

                dt.Rows.Add(row);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            return ds;
        }
    }
}