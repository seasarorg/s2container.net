using System.Data;

namespace Seasar.Extension.DataSets.States
{
	public sealed class RowStateFactory
	{
		private RowStateFactory()
		{
		}

		public static RowState GetRowState(DataRowState rowState) 
		{
			RowState result = null;
			switch (rowState) 
			{
				case DataRowState.Added:
					result = new CreatedState();
					break;
				case DataRowState.Modified:
					result = new ModifiedState();
					break;
				case DataRowState.Deleted:
					result = new RemovedState();
					break;
				case DataRowState.Unchanged:
					result = new UnchangedState();
					break;
				case DataRowState.Detached:
					result = new DetachedState();
					break;
				default:
					break;
			}
			return result;
		}
	}
}
