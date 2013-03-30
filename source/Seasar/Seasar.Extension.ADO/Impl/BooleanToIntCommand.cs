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

using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class BooleanToIntCommand : DbCommandWrapper
    {
        private readonly BooleanToIntParameterCollection _parameters = new BooleanToIntParameterCollection();

        public BooleanToIntCommand(IDbCommand original)
            : base(original)
        {
        }

        public override IDataReader ExecuteReader(CommandBehavior behavior)
        {
            SetParameters();
            return base.ExecuteReader(behavior);
        }

        public override IDataReader ExecuteReader()
        {
            SetParameters();
            return base.ExecuteReader();
        }

        public override object ExecuteScalar()
        {
            SetParameters();
            return base.ExecuteScalar();
        }

        public override int ExecuteNonQuery()
        {
            SetParameters();
            return base.ExecuteNonQuery();
        }

        public override IDataParameterCollection Parameters
        {
            get { return _parameters; }
        }

        public override IDbDataParameter CreateParameter()
        {
            return new BooleanToIntParameter(base.CreateParameter());
        }

        public override void Dispose()
        {
            base.Dispose();
            _parameters.Clear();
        }

        private void SetParameters()
        {
            base.Parameters.Clear();
            foreach (BooleanToIntParameter p in _parameters)
            {
                base.Parameters.Add(p.Original);
            }
        }
    }
}
