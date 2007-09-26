/*
 * Created by: H.Fujii
 * Created: 2007”N6ŒŽ21“ú
 */

using Seasar.Dxo.Annotation;

namespace Seasar.Tests.Dxo 
{
    public interface IEmployeeDxo
    {
        EmployeePage ConvertToEmployeePage(Employee emp);

        void ConvertToEmpPage(Employee emp, EmployeePage page);

        [ConversionRule(PropertyName = "EName", TargetPropertyName = "DName")]
        [ConversionRule("DName","EName")]
        void ConvertToSpecialEmpPage(Employee emp, EmployeePage page);

        void ConvertPageToEmp(EmployeePage page, Employee emp);
    }
}