using System.Data;

namespace Seasar.Extension.DataSets
{
	public interface IDataWriter
	{
		void Write(DataSet dataSet);
	}
}
