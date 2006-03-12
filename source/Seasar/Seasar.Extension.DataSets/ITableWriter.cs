using System.Data;

namespace Seasar.Extension.DataSets
{
	public interface ITableWriter
	{
		void Write(DataTable table);
	}
}
