#region Copyright

/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Collections;
using System.IO;
using System.Text;
using Seasar.WindowsExample.Logics.Dto;

namespace Seasar.WindowsExample.Logics.Dao.Impl
{
    /// <summary>
    /// CSV出力用DAO実装クラス
    /// </summary>
    public class OutputCSVDaoImpl : IOutputCSVDao
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputCSVDaoImpl()
        {
            ;
        }

        /// <summary>
        /// 社員データを出力する
        /// </summary>
        /// <param name="path">出力先パス</param>
        /// <param name="list">社員データ</param>
        /// <returns>出力件数</returns>
        public int OutputEmployeeList(string path, IList list)
        {
            if ( path == "" )
                throw new ArgumentNullException("path");
            if ( list == null )
                throw new ArgumentNullException("list");

            int ret = 0;
            using ( FileStream fs = File.Open(path, FileMode.Create) )
            {
                StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding(932));
                StringBuilder builder;
                try
                {
                    foreach (EmployeeCsvDto dto in list)
                    {
                        builder = new StringBuilder();

                        builder.Append(String.Format("\"{0}\",", dto.Code));
                        builder.Append(String.Format("\"{0}\",", dto.Name));
                        builder.Append(String.Format("{0},", dto.Gender));
                        if ( dto.EntryDay.HasValue )
                            builder.Append(String.Format("\"{0:yyyy/M/d}\",", dto.EntryDay.Value));
                        else
                            builder.Append("\"\",");
                        builder.Append(String.Format("\"{0}\",", dto.DeptCode));
                        builder.Append(String.Format("\"{0}\"", dto.DeptName));

                        writer.WriteLine(builder.ToString());
                        ret++;
                    }
                    writer.Close();
                }
                catch ( Exception ex )
                {
                    if ( writer != null ) writer.Close();
                    throw ex;
                }
            }

            return ret;
        }
    }
}