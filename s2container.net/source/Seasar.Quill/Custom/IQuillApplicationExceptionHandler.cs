using Seasar.Quill.Exception;

namespace Seasar.Quill.Custom
{
    public delegate object HandleQuillApplicationException(QuillApplicationException ex);

    public interface IQuillApplicationExceptionHandler
    {
        object Handle(QuillApplicationException ex);
    }
}
