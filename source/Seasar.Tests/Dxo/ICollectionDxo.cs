/*
 * Created by: 
 * Created: 2007”N7ŒŽ2“ú
 */

using System.Collections;
using System.Collections.Generic;

namespace Seasar.Tests.Dxo 
{
    public interface ICollectionDxo
    {
        void ConvertFromArrayToArray(Employee[] listData, EmployeePage[]targetData);
        EmployeePage[] ConvertToArray(Employee[] listData);

        void ConvertListToList(IList<Employee> listData, IList<EmployeePage> targetData);

        List<EmployeePage> ConvertToList(IList<Employee> listData);

        void ConvertToDictinary(Employee emp, IDictionary dict);

        Hashtable ConvertToHashtable(Employee emp);

        void ConvertToPONO(IDictionary dict, Employee emp);
    }
}