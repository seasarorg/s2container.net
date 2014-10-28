using NUnit.Framework;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill
{
    public class QuillTestBase
    {
        [TearDown]
        public void TearDown()
        {
            SingletonInstances.Clear();
        }
    }
}
