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

using System;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Extension.DataSets.Impl;

namespace Seasar.Examples.Reference.S2Unit
{
    public class Db2ExcelClient2
    {
        private const string PATH = "Seasar.Examples/Reference/S2Unit/Db2ExcelClient2.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            container.Init();
            try
            {
                SqlReader reader = (SqlReader) container.GetComponent(typeof(SqlReader));
                XlsWriter writer = (XlsWriter) container.GetComponent(typeof(XlsWriter));
                writer.Write(reader.Read());
                Console.Out.WriteLine("output Excel File : {0}", writer.FullPath);
            }
            catch (ApplicationException e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }
    }
}
