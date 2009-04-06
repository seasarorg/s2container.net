namespace Seasar.Tests.Dxo
{
    public class NestedEmployee
    {
        private string _code;

        private Employee _emp;

        public string Name
        {
            get { return _code; }
            set { _code = value; }
        }

        public Employee Emp
        {
            get { return _emp; }
            set { _emp = value; }
        }

    }
}