/*
 * Created by: H.Fujii
 * Created: 2007”N6Œ21“ú
 */

namespace Seasar.Tests.Dxo 
{
    public class Employee
    {
        private Department _department;
        private string _ename;

        public string EName
        {
            get { return _ename; }
            set { _ename = value; }
        }

        public Department Department
        {
            get { return _department; }
            set { _department = value; }
        }
    }
}