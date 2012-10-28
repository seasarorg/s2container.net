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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MbUnit.Framework;
using Seasar.Dao;

namespace Seasar.Tests.Dao
{
    /// <summary>
    /// 例外クラステスト
    /// </summary>
    [TestFixture]
    public class ExceptionTest
    {
        /// <summary>
        /// NotSingleRowUpdatedRuntimeExceptionのデシリアライズテスト
        /// </summary>
        [Test]
        public void TestDesirializeNotSingleRowUpdatedException()
        {
            const string TEST_BEAN = "Bean";
            const int TEST_ROW = 99;
            var target = new NotSingleRowUpdatedRuntimeException(TEST_BEAN, TEST_ROW);

            string path = GetFileName(target);
            // シリアル化
            Serialize(path, target);

            // 逆シリアル化
            object result = Desirialize(path);

            // ============================================================================
            //                                                                      Assert
            //                                                                     ========
            Assert.IsNotNull(result, "Nullチェック");
            Assert.IsTrue(result is NotSingleRowUpdatedRuntimeException, "型チェック");
            var resultEx = (NotSingleRowUpdatedRuntimeException)result;
            Assert.AreEqual(TEST_BEAN, resultEx.Bean, "値チェック１");
            Assert.AreEqual(TEST_ROW, resultEx.Rows, "値チェック２");
        }

        /// <summary>
        /// NotSingleRowUpdatedRuntimeExceptionのデシリアライズテスト
        /// </summary>
        [Test]
        public void TestDesirializeUpdateFailureRuntimeException()
        {
            const string TEST_BEAN = "Bean";
            const int TEST_ROW = 99;
            var target = new UpdateFailureRuntimeException(TEST_BEAN, TEST_ROW);

            string path = GetFileName(target);
            // シリアル化
            Serialize(path, target);

            // 逆シリアル化
            object result = Desirialize(path);

            // ============================================================================
            //                                                                      Assert
            //                                                                     ========
            Assert.IsNotNull(result, "Nullチェック");
            Assert.IsTrue(result is UpdateFailureRuntimeException, "型チェック");
            var resultEx = (UpdateFailureRuntimeException)result;
            Assert.AreEqual(TEST_BEAN, resultEx.Bean, "値チェック１");
            Assert.AreEqual(TEST_ROW, resultEx.Rows, "値チェック２");
        }

        private string GetFileName(object data)
        {
            return string.Format("{0}_serizlizeTest.data", data.GetType().Name);
        }

        private void Serialize(string path, object data)
        {
            string TEST_FILE = data.GetType().Name + "_serializeTest.dat";
            // シリアル化
            using (var fs = new FileStream(path, FileMode.Create))
            {
                var sr = new BinaryFormatter();
                using (var writer = new StreamWriter(fs))
                {
                    sr.Serialize(fs, data);
                }
            }
        }

        private object Desirialize(string path)
        {
            object result = null;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var dsr = new BinaryFormatter();
                using (var reader = new StreamReader(fs))
                {
                    result = dsr.Deserialize(fs);
                }
            }
            return result;
        }
    }
}
