using System;
using Seasar.Quill.Attrs;
using Seasar.Quill.Examples.Entity;
using Seasar.Dao.Attrs;

namespace Seasar.Quill.Examples.Dao
{
    [Implementation]
    [Aspect("DaoInterceptor")]
    [Bean(typeof(Employee))]
    public interface IEmployeeDao
    {
        Employee GetByEmpNo(int empNo);
    }
}
