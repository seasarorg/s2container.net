/*
 * Created by: 
 * Created: 2007”N6Œ21“ú
 */

namespace Seasar.Tests.Dxo 
{
    public class Department
    {
        private int? _id;
        private string _dname;


        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string DName
        {
            get { return _dname; }
            set { _dname = value; }
        }
    }
}