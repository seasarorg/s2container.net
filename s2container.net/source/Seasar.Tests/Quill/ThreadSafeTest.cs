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

using System.Threading;
using MbUnit.Framework;
using Seasar.Quill;

namespace Seasar.Tests.Quill
{
    /// <summary>
    /// スレッドセーフを検証するためのテスト
    /// </summary>
    [TestFixture]
    public class ThreadSafeTest
    {
        QuillContainer _container = new QuillContainer();

        [SetUp]
        public void SetupTest()
        {
            //  MbUnitはstatic変数を保持するのでここでリセット
            HeavyClass.Result = 0;
        }

        [Test]
        public void TestThreadSafe()
        {
            Thread t1 = new Thread(MyGetComponent);
            Thread t2 = new Thread(MyGetComponent);
            Thread t3 = new Thread(MyGetComponent);

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();

            Assert.AreEqual(1, HeavyClass.Result, 
                "コンストラクタが１回しか呼ばれていなけりゃ１ですよね");
        }

        private void MyGetComponent()
        {
            _container.GetComponent(typeof(HeavyClass));
        }
    }

    public class HeavyClass
    {
        public static int Result = 0;
        public HeavyClass()
        {
            //  とりあえず500ms待ってみる
            //  待機中に別スレッドがGetComponentしても
            //  インスタンスが二重三重に作られないか
            //  チェックするため
            Result++;
            Thread.Sleep(500);
        }
    }
}
