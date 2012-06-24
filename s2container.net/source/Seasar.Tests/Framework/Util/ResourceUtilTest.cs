#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Framework.Util;
using System.Resources;
using System.Reflection.Emit;
using System.Threading;

namespace Seasar.Tests.Framework.Util
{
    [TestFixture]
    public class ResourceUtilTest
    {
        [Test]
        public void TestGetExtension()
        {
            Assert.AreEqual("xml", ResourceUtil.GetExtension("aaa.bbb.xml"));
            Assert.AreEqual(null, ResourceUtil.GetExtension("aaa"));
        }

        [Test]
        public void TestGetResourceAsStream()
        {
            StreamReader stream = ResourceUtil.GetResourceAsStreamReader(
                "Seasar.Tests.Framework.Util.test1.xml", Assembly.GetExecutingAssembly());
            Trace.WriteLine(stream.ReadToEnd());
            stream.Close();
        }

        [Test]
        public void TestResourceNotFound()
        {
            StreamReader stream = null;
            try
            {
                stream = ResourceUtil.GetResourceAsStreamReader(
                    "Seasar.Tests.Framework.Util.test2.xml", Assembly.GetExecutingAssembly());
                Assert.Fail();
            }
            catch (ResourceNotFoundRuntimeException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        [Test]
        public void TestGetResourceNoException_リソースが存在する場合()
        {
            using (Stream stream = ResourceUtil.GetResourceNoException(
                "Seasar.Tests.Framework.Util.test1.xml", Assembly.GetExecutingAssembly()))
            {
                Assert.IsNotNull(stream);
            }
        }

        [Test]
        public void TestGetResourceNoException_リソースが存在しない場合()
        {
            using (Stream stream = ResourceUtil.GetResourceNoException(
                "Seasar.Tests.Framework.Util.test2.xml", Assembly.GetExecutingAssembly()))
            {
                Assert.IsNull(stream);
            }
        }

        //[Test]
        //public void TestGetResourceNoException_動的アセンブリ()
        //{

        //    AssemblyBuilder dynamicAssembly;
        //    IResourceWriter resourceWriter;
        //    dynamicAssembly = (AssemblyBuilder) CreateAssembly(Thread.GetDomain()).Assembly;

        //    resourceWriter = dynamicAssembly.DefineResource("myResourceFile",
        //       "A sample Resource File", "MyEmitAssembly.MyResource.resources");

        //    resourceWriter.AddResource("AddResource 1", "First added resource");
        //    resourceWriter.AddResource("AddResource 2", "Second added resource");
        //    resourceWriter.AddResource("AddResource 3", "Third added resource");

        //    using (Stream stream = ResourceUtil.GetResourceNoException(
        //        "MyEmitAssembly.MyResource.resources", dynamicAssembly))
        //    {
        //        Assert.IsNull(stream);
        //    }

        //}

        ///// <summary>
        ///// テスト用の動的アセンブリを作成する
        ///// </summary>
        ///// <param name="appDomain">アプリケーションドメイン</param>
        ///// <returns>テスト用の動的アセンブリ</returns>
        //private Type CreateAssembly(AppDomain appDomain)
        //{
        //    AssemblyName assemblyName = new AssemblyName();
        //    assemblyName.Name = "testAssembly";

        //    AssemblyBuilder dynamicAssembly = appDomain.DefineDynamicAssembly(assemblyName,
        //       AssemblyBuilderAccess.Save);

        //    ModuleBuilder testModule = dynamicAssembly.DefineDynamicModule("EmittedModule",
        //       "EmittedModule.mod");

        //    TypeBuilder helloWorldClass =
        //       testModule.DefineType("HelloWorld", TypeAttributes.Public);

        //    MethodBuilder myMethod = helloWorldClass.DefineMethod("Display",
        //       MethodAttributes.Public, typeof(String), null);

        //    ILGenerator methodIL = myMethod.GetILGenerator();
        //    methodIL.Emit(OpCodes.Ldstr, "Display _method get called.");
        //    methodIL.Emit(OpCodes.Ret);

        //    return (helloWorldClass.CreateType());
        //}

    }
}
