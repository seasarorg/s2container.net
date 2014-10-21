
namespace Seasar.Quill.Custom
{
    public delegate object HandleSystemException(System.Exception ex);

    public interface ISystemExceptionHandler
    {
        object Handle(System.Exception ex);
    }
}
