
namespace Seasar.Quill.Handler.Impl
{
    public class SystemExceptionHandlerImpl : ISystemExceptionHandler
    {
        public void Handle(System.Exception ex)
        {
            throw ex;
        }
    }
}
