using Seasar.Unit.Core;

namespace Seasar.Extension.Unit
{
    public class S2Attribute : S2MbUnitAttributeBase
    {
        public S2Attribute()
            : base()
        {
        }

        public S2Attribute(Seasar.Extension.Unit.Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override S2TestCaseRunnerBase CreateRunner(Seasar.Extension.Unit.Tx txTreatment)
        {
            return new S2TestCaseRunner(txTreatment);
        }
    }
}
