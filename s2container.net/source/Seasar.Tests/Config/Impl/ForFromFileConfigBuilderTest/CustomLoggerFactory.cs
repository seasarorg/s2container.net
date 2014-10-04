using Seasar.Quill.Log;
using System;

namespace Seasar.Tests.Config.Impl.ForFromFileConfigBuilderTest
{
    public class CustomLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(Type t)
        {
            throw new NotImplementedException();
        }
    }
}
