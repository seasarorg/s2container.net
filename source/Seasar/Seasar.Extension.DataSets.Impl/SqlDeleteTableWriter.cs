using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.DataSets.States;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlDeleteTableWriter : SqlTableWriter
	{
		public SqlDeleteTableWriter(IDataSource dataSource) : base(dataSource)
		{
		}

		protected override void DoWrite(DataTable table) 
		{
			foreach (DataRow row in table.Rows) 
			{
				RowState state = RowStateFactory.GetRowState(DataRowState.Deleted);
				state.Update(DataSource, row);
			}
		}
	}
}
