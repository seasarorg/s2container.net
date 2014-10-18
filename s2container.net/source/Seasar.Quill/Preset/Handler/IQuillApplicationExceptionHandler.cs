using Seasar.Quill.Exception;

namespace Seasar.Quill.Preset.Handler
{
    public delegate void HandleQuillApplicationException(QuillApplicationException ex);

    public interface IQuillApplicationExceptionHandler
    {
        void Handle(QuillApplicationException ex);
    }
}
