using NUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Attr;
using Seasar.Quill.Preset.FieldSelect;
using System.Collections.Generic;
using System.Linq;

namespace Seasar.Tests.Quill.Preset.FieldSelect
{
    [TestFixture]
    public class ImplementationFieldSelectorTest : QuillTestBase
    {
        [Test]
        public void TestSelectField()
        {
            // Arrange
            var selector = new ImplementationFieldSelector();
            var expectedFieldNames = new List<string>();
            expectedFieldNames.Add("_selectedInstance");
            expectedFieldNames.Add("<SelectedProp>k__BackingField");
            expectedFieldNames.Add("_selectedPublicValue");
            var target = new SelectTestClass();
            
            // Act
            var result = selector.Select(target, new QuillInjectionContext());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedFieldNames.Count(), result.Count());
            foreach(var actual in result)
            {
                Assert.IsTrue(expectedFieldNames.Contains(actual.Name));
            }
        }


        private class SelectTestClass
        {
            private string _notSelectedInstance = "";
            private SelectTargetClass _selectedInstance = new SelectTargetClass();

            public string NotSelectedProp { get; set; }
            public SelectTargetClass SelectedProp { get; set; }

            private static SelectTargetClass _notSelectedPrivateStaticValue = new SelectTargetClass();
            public static SelectTargetClass _notSelectedPublicStaticValue = new SelectTargetClass();
            public SelectTargetClass _selectedPublicValue = new SelectTargetClass();

            public SelectTestClass()
            {
                // 警告を出さないようにするために適当に参照
                _notSelectedInstance.Trim();
                _notSelectedPrivateStaticValue.GetType();
                _notSelectedPublicStaticValue.GetType();
                _selectedPublicValue.GetType();
            }
        }

        /// <summary>
        /// インジェクション対象条件を満たすクラス
        /// </summary>
        [Implementation]
        private class SelectTargetClass
        {
        }
    }
}
