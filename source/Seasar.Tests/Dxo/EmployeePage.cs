/*
 * Created by: H.Fujii
 * Created: 2007”N6ŒŽ21“ú
 */

namespace Seasar.Tests.Dxo 
{
    public class EmployeePage
    {
        private string _dname;
        private string _ename;
        private int? _id;
        private string _name;


        public string DName
        {
            get { return _dname; }
            set { _dname = value; }
        }

        public string EName
        {
            get { return _ename; }
            set { _ename = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}