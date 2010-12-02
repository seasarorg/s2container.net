using Seasar.Unit.Core;

namespace Seasar.Quill.Unit
{
    public class QuillAttribute : S2MbUnitAttributeBase
    {
        public QuillAttribute() : base()
        {
        }

        public QuillAttribute(Seasar.Extension.Unit.Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override S2TestCaseRunnerBase CreateRunner(Seasar.Extension.Unit.Tx txTreatment)
        {
            return new QuillTestCaseRunner(txTreatment);
        }
    }
}
