using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Extension.DataSets.States
{
	public interface RowState
	{
		void Update(IDataSource dataSource, DataRow row);
	}
}
