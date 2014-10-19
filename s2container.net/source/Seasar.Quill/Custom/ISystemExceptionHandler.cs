
namespace Seasar.Quill.Custom
{
    public delegate void HandleSystemException(System.Exception ex);

    public interface ISystemExceptionHandler
    {
        void Handle(System.Exception ex);
    }
}
