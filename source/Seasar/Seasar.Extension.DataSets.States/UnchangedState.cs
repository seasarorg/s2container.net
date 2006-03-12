using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.States
{
	public class UnchangedState : RowState
	{
		public override string ToString()
		{
			return DataRowState.Unchanged.ToString();
		}

		#region RowState ƒƒ“ƒo

		public void Update(IDataSource dataSource, DataRow row)
		{
		}

		#endregion
	}
}
