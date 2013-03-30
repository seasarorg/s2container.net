using Seasar.Quill.Attrs;
using Seasar.Quill.Examples.Entity;

namespace Seasar.Quill.Examples.Logic
{
    [Implementation(typeof(EmployeeLogic))]
    public interface IEmployeeLogic
    {
        Employee GetEmployeeByEmpNo(int empNo);
    }
}
