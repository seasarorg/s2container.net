
namespace Seasar.Quill.Preset.Handler.Impl
{
    public class SystemExceptionHandlerImpl : ISystemExceptionHandler
    {
        public void Handle(System.Exception ex)
        {
            throw ex;
        }
    }
}
