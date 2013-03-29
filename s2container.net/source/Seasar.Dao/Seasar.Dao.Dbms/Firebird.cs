#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

namespace Seasar.Dao.Dbms
{
    public class Firebird : Standard
    {
        public override string Suffix
        {
            get { return "_firebird"; }
        }

        public override string GetSequenceNextValString(string sequenceName)
        {
            return "select gen_id( " + sequenceName + ", 1 ) from RDB$DATABASE";
        }

        public override KindOfDbms Dbms
        {
            get { return KindOfDbms.Firebird; }
        }
    }
}
