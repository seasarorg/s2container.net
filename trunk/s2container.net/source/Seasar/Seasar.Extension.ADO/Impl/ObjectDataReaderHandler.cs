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

using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class ObjectDataReaderHandler : IDataReaderHandler
    {
        #region IDataReaderHandler メンバ

        public object Handle(IDataReader dataReader)
        {
            // 【固定でreturn null;としている理由】
            // BasicSelectHandler#Execute()において
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // if (_dataReaderHandler is ObjectDataReaderHandler) {
            //     return CommandFactory.ExecuteScalar(DataSource, cmd);
            // }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // という処理を行っているためである。
            // ObjectDataReaderHandlerというクラス自体は単なるマーカークラスであり、
            // その実装自体に特に意味を持っていない。
            // 
            return null;
        }

        #endregion
    }
}
