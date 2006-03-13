using System;
using System.Collections;
using System.Data;
using System.Text;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.States
{
	public class RemovedState : AbstractRowState
	{
		private static Hashtable sqlCache_ = new Hashtable();

		private static Hashtable argNamesCache_ = new Hashtable();

		public override string ToString()
		{
			return DataRowState.Deleted.ToString();
		}

		protected override string GetSql(DataTable table) 
		{
			string sql = null;
			WeakReference reference = (WeakReference) sqlCache_[table];
			if (reference == null || !reference.IsAlive) 
			{
				sql = CreateSql(table);
				sqlCache_.Add(table, new WeakReference(sql));
			} 
			else 
			{
				sql = (string) reference.Target;
			}
			return sql;
		}

		private static string CreateSql(DataTable table) 
		{
			StringBuilder buf = new StringBuilder(100);
			buf.Append("DELETE FROM ");
			buf.Append(table.TableName);
			buf.Append(" WHERE ");
			foreach (DataColumn column in table.Columns) 
			{
				if (DataTableUtil.IsPrimaryKey(table, column)) 
				{
					buf.Append(column.ColumnName);
					buf.AppendFormat(" = @{0} AND ", column.ColumnName);
				}
			}
			buf.Length -= 5;

			return buf.ToString();
		}

		protected override string[] GetArgNames(DataTable table) 
		{
			string[] argNames = null;
			WeakReference reference = (WeakReference) argNamesCache_[table];
			if (reference == null || !reference.IsAlive) 
			{
				argNames = CreateArgNames(table);
				argNamesCache_.Add(table, new WeakReference(argNames));
			} 
			else 
			{
				argNames = (string[]) reference.Target;
			}
			return argNames;
		}

		private static string[] CreateArgNames(DataTable table) 
		{
			ArrayList argNames = new ArrayList();
			for (int i = 0; i < table.Columns.Count; ++i) 
			{
				DataColumn column = table.Columns[i];
				if (DataTableUtil.IsPrimaryKey(table, column))
				{
					argNames.Add(column.ColumnName);
				}
			}
			return (string[]) argNames.ToArray(typeof(string));
		}

		protected override object[] GetArgs(DataRow row) 
		{
			DataTable table = row.Table;
			ArrayList bindVariables = new ArrayList();
			for (int i = 0; i < table.Columns.Count; ++i) 
			{
				DataColumn column = table.Columns[i];
				if (DataTableUtil.IsPrimaryKey(table, column))
				{
					if (row.RowState == DataRowState.Detached) 
					{
						bindVariables.Add(row[i, DataRowVersion.Proposed]);
					} 
					else if (row.RowState == DataRowState.Deleted) 
					{
						bindVariables.Add(row[i, DataRowVersion.Original]);
					}
					else 
					{
						bindVariables.Add(row[i, DataRowVersion.Current]);
					}
				}
			}
			return bindVariables.ToArray();
		}
	}
}
