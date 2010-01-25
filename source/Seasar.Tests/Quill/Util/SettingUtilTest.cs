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

using System.Diagnostics;
using System.IO;
using MbUnit.Framework;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill.Util
{
    /// <summary>
    /// 設定ユーティリティテストクラス
    /// </summary>
    [TestFixture]
    public class SettingUtilTest
    {
        /// <summary>
        /// アセンブリが置いてあるパスの取得テスト
        /// </summary>
        [Test]
        public void TestGetAssemblyDirectoryPath()
        {
            //  QuillConfigTestで使っているファイルパスの取得
            string path = SettingUtil.GetAssemblyDirectoryPath() + 
                "Quill\\ResourcesForQuillConfig\\QuillConfigTest_Empty.config";
            Trace.WriteLine(path);
            Assert.IsTrue(File.Exists(path), "正しくパスが取得できていればtrueが返ってくるはず");
        }

        /// <summary>
        /// Quill設定ファイルパス取得テスト
        /// </summary>
        [Test]
        public void TestGetDefaultQuillConfigPath()
        {
            string path = SettingUtil.GetDefaultQuillConfigPath();
            Trace.WriteLine(path);
            Assert.IsTrue(File.Exists(path), "テスト用のQuill設定ファイルを見つけられるか");
        }
    }
}
