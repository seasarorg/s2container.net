using Seasar.Quill.Database.Tx.Impl;

namespace Seasar.Tests.Quill
{
    public class S2ContainerTxSetting : AbstractTransactionSetting
    {
        protected override void SetupTransaction(Seasar.Extension.ADO.IDataSource dataSource)
        {
            //  使用されることはないので空実装
            return;
        }

        public override bool IsNeedSetup()
        {
            //  固定でtrueを返す
            return false;
        }
    }
}
