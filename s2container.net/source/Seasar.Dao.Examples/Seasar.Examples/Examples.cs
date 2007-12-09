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
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using log4net;
using log4net.Config;
using log4net.Util;

using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Examples
{
    /// <summary>
    /// Examplesをブートストラップするクラスです。
    /// </summary>
    public class Examples
    {
        public Examples()
        {
        }

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main() 
        {
            TextWriter Out = Console.Out;
            TextWriter Err = Console.Error;
            try 
            {
                // log4netの初期化
                FileInfo info = new FileInfo(SystemInfo.AssemblyShortName(
                    Assembly.GetExecutingAssembly()) + ".exe.config");
                XmlConfigurator.Configure(LogManager.GetRepository(), info);
                
                SingletonS2ContainerFactory.Init();
                IS2Container container = SingletonS2ContainerFactory.Container;
                ExamplesExplorer de = container.GetComponent(typeof(ExamplesExplorer)) as ExamplesExplorer;
                if(de != null) 
                {
                    Application.Run(de);
                }
            } 
            catch(Exception e) 
            {
                Out.WriteLine(e.Message);
                Console.ReadLine();
            } 
            finally 
            {
                SingletonS2ContainerFactory.Destroy();
            }
        }
    }
}
