using System;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Extension.DataSets.States
{
	public abstract class AbstractRowState : RowState
	{
		protected AbstractRowState()
		{
		}

		#region RowState ÉÅÉìÉo

		public void Update(IDataSource dataSource, DataRow row)
		{
			DataTable table = row.Table;
			string sql = GetSql(table);
			string[] argNames = GetArgNames(table);
			object[] args = GetArgs(row);
			IUpdateHandler handler = new BasicUpdateHandler(dataSource, sql);
			Execute(handler, args, argNames);
		}

		#endregion

		protected abstract string GetSql(DataTable table);

		protected abstract string[] GetArgNames(DataTable table);

		protected abstract object[] GetArgs(DataRow row);

		protected void Execute(IUpdateHandler handler, object[] args, string[] argNames) 
		{
			handler.Execute(args, Type.GetTypeArray(args), argNames);
		}
	}
}
