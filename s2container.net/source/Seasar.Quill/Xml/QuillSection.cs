#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Text;

namespace Seasar.Quill.Xml
{
    /// <summary>
    /// Quillの構成セクション
    /// </summary>
    public class QuillSection
    {
        private string _daoSetting = null;
        private string _transactionSetting = null;
        private IList _dataSources = new ArrayList();
        private IList _assemblys = new ArrayList();

        /// <summary>
        /// S2Dao関係設定クラス
        /// </summary>
        public string DaoSetting
        {
            set { _daoSetting = value; }
            get { return _daoSetting; }
        }

        /// <summary>
        /// トランザクション関係設定クラス
        /// </summary>
        public string TransactionSetting
        {
            set { _transactionSetting = value; }
            get { return _transactionSetting; }
        }

        /// <summary>
        /// データソースリスト
        /// </summary>
        public IList DataSources
        {
            set { _dataSources = value; }
            get { return _dataSources; }
        }

        /// <summary>
        /// アセンブリリスト
        /// </summary>
        public IList Assemblys
        {
            set { _assemblys = value; }
            get { return _assemblys; }
        }

        /// <summary>
        /// セクション情報を文字列で返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            //  Dao設定クラス
            builder.Append("DaoSetting=[");
            if (string.IsNullOrEmpty(DaoSetting))
            {
                builder.AppendLine("Nothing]");
            }
            else
            {
                builder.AppendFormat("{0}]\n", DaoSetting);
            }
            //  Transaction設定クラス
            builder.Append("TransactionSetting=[");
            if (string.IsNullOrEmpty(TransactionSetting))
            {
                builder.AppendLine("Nothing]");
            }
            else
            {
                builder.AppendFormat("{0}]\n", TransactionSetting);
            }

            //  データソース
            builder.Append("DataSources=[");
            if (DataSources == null || DataSources.Count == 0)
            {
                builder.AppendLine("Nothing]");
            }
            else
            {
                foreach (object ds in DataSources)
                {
                    builder.AppendFormat("{0},", ds);
                }
                builder.Replace(",", string.Empty, builder.Length - 1, 1);
                builder.AppendLine("]");
            }

            //  アセンブリ
            builder.Append("Assembly=[");
            if (Assemblys == null || Assemblys.Count == 0)
            {
                builder.AppendLine("Nothing]");
            }
            else
            {
                foreach (object assembly in Assemblys)
                {
                    builder.AppendFormat("{0},", assembly);
                }
                builder.Replace(",", string.Empty, builder.Length - 1, 1);
                builder.AppendLine("]");
            }

            return builder.ToString();
        }
    }
}
