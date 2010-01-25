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

using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Framework.Container;
using Seasar.Quill;
using System.Reflection;
using Seasar.Framework.Log;

namespace Seasar.Tests.Quill
{
    [TestFixture]
    public class InjectionMapTest
    {
        private readonly Logger _log = Logger.GetLogger(typeof (InjectionMapTest));

        [SetUp]
        public void InitializeMap()
        {    
            InjectionMap.GetInstance().Clear();
            _log.Debug("Initialized");
            Console.WriteLine("Initialized");
        }

        [Test]
        public void TestAdd_InterfaceAndImpl()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");
            
            //  ## Act ##
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            map.Add(typeof(IInjectionMapTarget2), typeof(InjectionMapTargetImpl2));

            //  ## Assert ##
            IDictionary<Type, Type> actual = GetInnerInjectionMap(map);
            Assert.IsTrue(actual.ContainsKey(typeof(IInjectionMapTarget1)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1), actual[typeof(IInjectionMapTarget1)]);

            Assert.IsTrue(actual.ContainsKey(typeof(IInjectionMapTarget2)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl2), actual[typeof(IInjectionMapTarget2)]);

            Assert.IsFalse(actual.ContainsKey(typeof(NoAdditional)));
        }

        [Test]
        [ExpectedException(typeof(TooManyRegistrationRuntimeException))]
        public void TestAdd_InterfaceAndImpl_TooMany()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act ##
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
        }

        [Test]
        public void TestAdd_Component()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act ##
            map.Add(typeof(InjectionMapTargetImpl1));
            map.Add(typeof(InjectionMapTargetImpl2));

            //  ## Assert ##
            IDictionary<Type, Type> actual = GetInnerInjectionMap(map);
            Assert.IsTrue(actual.ContainsKey(typeof(InjectionMapTargetImpl1)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1), actual[typeof(InjectionMapTargetImpl1)]);

            Assert.IsTrue(actual.ContainsKey(typeof(InjectionMapTargetImpl2)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl2), actual[typeof(InjectionMapTargetImpl2)]);

            Assert.IsFalse(actual.ContainsKey(typeof(NoAdditional)));
        }

        [Test]
        [ExpectedException(typeof(TooManyRegistrationRuntimeException))]
        public void TestAdd_Component_TooMany()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act ##
            map.Add(typeof(InjectionMapTargetImpl1));
            map.Add(typeof(InjectionMapTargetImpl1));
        }

        [Test]
        public void TestAdd_IDictionary()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            IDictionary<Type, Type> newMap = new Dictionary<Type, Type>();
            newMap.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            newMap.Add(typeof(IInjectionMapTarget2), typeof(InjectionMapTargetImpl2));

            //  ## Act ##
            map.Add(newMap);

            //  ## Assert ##
            IDictionary<Type, Type> actual = GetInnerInjectionMap(map);
            Assert.IsTrue(actual.ContainsKey(typeof(IInjectionMapTarget1)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1), actual[typeof(IInjectionMapTarget1)]);

            Assert.IsTrue(actual.ContainsKey(typeof(IInjectionMapTarget2)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl2), actual[typeof(IInjectionMapTarget2)]);

            Assert.IsFalse(actual.ContainsKey(typeof(NoAdditional)));
        }

        [Test]
        [ExpectedException(typeof(TooManyRegistrationRuntimeException))]
        public void TestAdd_IDictionary_TooMany()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            IDictionary<Type, Type> newMap = new Dictionary<Type, Type>();
            newMap.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            newMap.Add(typeof(IInjectionMapTarget2), typeof(InjectionMapTargetImpl2));

            //  ## Act ##
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            map.Add(newMap);
        }

        [Test]
        public void TestAdd_List()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            IList<Type> newList = new List<Type>();
            newList.Add(typeof(InjectionMapTargetImpl1));
            newList.Add(typeof(InjectionMapTargetImpl2));

            //  ## Act ##
            map.Add(newList);

            //  ## Assert ##
            IDictionary<Type, Type> actual = GetInnerInjectionMap(map);
            Assert.IsTrue(actual.ContainsKey(typeof(InjectionMapTargetImpl1)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1), actual[typeof(InjectionMapTargetImpl1)]);

            Assert.IsTrue(actual.ContainsKey(typeof(InjectionMapTargetImpl2)));
            Assert.AreEqual(typeof(InjectionMapTargetImpl2), actual[typeof(InjectionMapTargetImpl2)]);

            Assert.IsFalse(actual.ContainsKey(typeof(NoAdditional)));
        }

        [Test]
        [ExpectedException(typeof(TooManyRegistrationRuntimeException))]
        public void TestAdd_List_TooMany()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            IList<Type> newList = new List<Type>();
            newList.Add(typeof(InjectionMapTargetImpl1));
            newList.Add(typeof(InjectionMapTargetImpl2));

            //  ## Act ##
            map.Add(typeof(InjectionMapTargetImpl1));
            map.Add(newList);
        }

        [Test]
        public void TestGetComponentType_InterfaceAndImpl()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1), 
                map.GetComponentType(typeof(IInjectionMapTarget1)));
        }

        [Test]
        public void TestGetComponentType_Component()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            map.Add(typeof(InjectionMapTargetImpl1));
            Assert.AreEqual(typeof(InjectionMapTargetImpl1),
                map.GetComponentType(typeof(InjectionMapTargetImpl1)));
        }

        [Test]
        public void TestGetComponentType_Dictionary()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            IDictionary<Type, Type> newMap = new Dictionary<Type, Type>();
            newMap.Add(typeof(IInjectionMapTarget2), typeof(InjectionMapTargetImpl2));
            map.Add(newMap);
            Assert.AreEqual(typeof(InjectionMapTargetImpl2),
                map.GetComponentType(typeof(IInjectionMapTarget2)));
        }

        [Test]
        public void TestGetComponentType_List()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            IList<Type> newList = new List<Type>();
            newList.Add(typeof(InjectionMapTargetImpl2));
            map.Add(newList);
            Assert.AreEqual(typeof(InjectionMapTargetImpl2),
                map.GetComponentType(typeof(InjectionMapTargetImpl2)));
        }

        [Test]
        [ExpectedException(typeof(ComponentNotFoundRuntimeException))]
        public void TestGetComponent_NotFound()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            map.GetComponentType(typeof (IInjectionMapTarget1));
        }

        [Test]
        public void TestHasComponentType_InterfaceAndImpl()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            map.Add(typeof(IInjectionMapTarget1), typeof(InjectionMapTargetImpl1));
            Assert.IsTrue(map.HasComponentType(typeof(IInjectionMapTarget1)));
        }

        [Test]
        public void TestHasComponentType_Component()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            map.Add(typeof(InjectionMapTargetImpl1));
            Assert.IsTrue(map.HasComponentType(typeof(InjectionMapTargetImpl1)));
        }

        [Test]
        public void TestHasComponentType_Dictionary()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            IDictionary<Type, Type> newMap = new Dictionary<Type, Type>();
            newMap.Add(typeof(IInjectionMapTarget2), typeof(InjectionMapTargetImpl2));
            map.Add(newMap);
            Assert.IsTrue(map.HasComponentType(typeof(IInjectionMapTarget2)));
        }

        [Test]
        public void TestHasComponentType_List()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            IList<Type> newList = new List<Type>();
            newList.Add(typeof(InjectionMapTargetImpl2));
            map.Add(newList);
            Assert.IsTrue(map.HasComponentType(typeof(InjectionMapTargetImpl2)));
        }

        [Test]
        public void TestHasComponentType_NotFound()
        {
            //  ## Arrange ##
            InjectionMap map = InjectionMap.GetInstance();
            IDictionary<Type, Type> beforeInnerMap = GetInnerInjectionMap(map);
            Assert.AreEqual(0, beforeInnerMap.Count, "テスト開始時点は要素をもっていない");

            //  ## Act + Assert ##
            Assert.IsFalse(map.HasComponentType(typeof(InjectionMapTargetImpl2)));
        }


        #region テスト用補助メソッド

        /// <summary>
        /// InjectionMap内部のIDictionaryを取り出す
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static IDictionary<Type, Type> GetInnerInjectionMap(InjectionMap map)
        {
            FieldInfo field = typeof(InjectionMap).GetField(
                "_injectionMap", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field);
            object fieldValue = field.GetValue(map);
            Assert.IsTrue(fieldValue is IDictionary<Type, Type>);
            return (IDictionary<Type, Type>) fieldValue;
        }

        #endregion

        #region テスト用クラス

        private interface IInjectionMapTarget1
        {
        }

        private class InjectionMapTargetImpl1 : IInjectionMapTarget1
        {
        }

        private interface IInjectionMapTarget2
        {
        }

        private class InjectionMapTargetImpl2 : IInjectionMapTarget2
        {
        }

        private class NoAdditional
        {
        }

        #endregion
    }
}
