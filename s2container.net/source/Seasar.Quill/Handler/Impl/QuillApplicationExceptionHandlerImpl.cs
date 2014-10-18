
namespace Seasar.Quill.Handler.Impl
{
    public class QuillApplicationExceptionHandlerImpl : IQuillApplicationExceptionHandler
    {
        public void Handle(Exception.QuillApplicationException ex)
        {
            throw ex;
        }
    }
}
