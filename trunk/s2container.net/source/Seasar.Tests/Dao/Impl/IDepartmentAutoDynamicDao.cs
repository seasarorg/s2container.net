using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Bean(typeof(Department))]
    public interface IDepartmentAutoDynamicDao
    {
        [Query("deptno=/*deptno*/")]
        Department GetDepartment(int deptno);

        void UpdateUnlessNull(Department employee);

        void UpdateModifiedOnly(Department employee);
    }
}
