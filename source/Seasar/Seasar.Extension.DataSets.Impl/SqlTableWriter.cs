using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Extension.DataSets.States;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Impl
{
	public class SqlTableWriter : ITableWriter
	{
		private IDataSource dataSource_;

		public SqlTableWriter(IDataSource dataSource)
		{
			dataSource_ = dataSource;
			ValueTypes.Init(dataSource);
		}

		public IDataSource DataSource 
		{
			get { return dataSource_; }
		}

		#region ITableWriter メンバ

		public void Write(DataTable table)
		{
			/// TODO メタデータを取得済みであるか判断するロジックを追加すること。
			//      DbDataAdapter#TableMappings から判断するしかないかなあ…。
			SetupMetaData(table);

			DoWrite(table);
		}

		#endregion

		protected virtual void DoWrite(DataTable table) 
		{
			foreach (DataRow row in table.Rows) 
			{
				RowState state = RowStateFactory.GetRowState(row.RowState);
				state.Update(DataSource, row);
			}
			table.AcceptChanges();
		}

		private void SetupMetaData(DataTable table) 
		{
			IDbConnection con = DataSourceUtil.GetConnection(DataSource);
			try 
			{
				IDatabaseMetaData dbMetaData = new DatabaseMetaDataImpl(DataSource);
				DataTableUtil.SetupMetaData(dbMetaData, table);
			} 
			finally 
			{
				DataSourceUtil.CloseConnection(DataSource, con);
			}
		}
	}
}
