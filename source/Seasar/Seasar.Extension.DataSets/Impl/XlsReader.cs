using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;

namespace Seasar.Extension.DataSets.Impl
{
	public class XlsReader : IDataReader
	{
		private const int DEFAULT_BUFFER_SIZE = 1024 * 4;

		private Regex illigalColumnNamePattern = new Regex("F[0-9]+$", RegexOptions.Compiled);

		private Regex workSheetPrefixPattern = new Regex(@"^#[0-9]+\s+.+$", RegexOptions.Compiled);

		private DataSet dataSet_;

		public XlsReader(string path)
		{
			string fullPath = Path.GetFullPath(path);
			if (!File.Exists(fullPath)) 
			{
				throw new FileNotFoundException("xls file not found.", fullPath);
			}

			dataSet_ = CreateDataSet(fullPath);
		}

		public XlsReader(Stream stream)
		{
			string tempPath = Path.GetTempFileName();
			using (Stream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write)) 
			{
				byte[] b = new byte[DEFAULT_BUFFER_SIZE];
				int n = 0;
				while (0 < (n = stream.Read(b, 0, b.Length))) 
				{
					fs.Write(b, 0, n);
				}
			}

			dataSet_ = CreateDataSet(tempPath);

			if (File.Exists(tempPath)) 
			{
				File.Delete(tempPath);
			}
		}

		#region IDataReader ƒƒ“ƒo

		public DataSet Read()
		{
			return dataSet_;
		}

		#endregion

		private DataSet CreateDataSet(string path) 
		{
			string connectonString = string.Format(
				"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"",
				path
				);

			DataSet ds = new DataSet();
			using (OleDbConnection con = new OleDbConnection(connectonString)) 
			{
				if (con.State != ConnectionState.Open) 
				{
					con.Open();
				}

				DataTable tableList = con.GetOleDbSchemaTable(
					OleDbSchemaGuid.Tables_Info,
					new object[] {null, null, null, "TABLE"}
					);

				foreach (DataRow row in tableList.Rows) 
				{
					string tableName = (string) row["TABLE_NAME"];
					if (!tableName.EndsWith("$") && !tableName.EndsWith("$'")) 
					{
						continue;
					}

					string sql = string.Format("select * from [{0}]", tableName);
					using (OleDbCommand cmd = new OleDbCommand(sql, con)) 
					{
						DbDataAdapter adapter = new OleDbDataAdapter(cmd);
						adapter.AcceptChangesDuringFill = false;
						DataTable table = new DataTable(tableName);
						adapter.Fill(table);
						SetupTable(table);
						ds.Tables.Add(table);
						//adapter.Fill(ds, tableName.Replace("$", string.Empty));
					}
				}
			}

			return ds;
		}

		private void SetupTable(DataTable table) 
		{
			string tableName = table.TableName.Trim();
			if (tableName.EndsWith("$")) 
			{
				tableName = tableName.Remove(tableName.Length - 1, 1);
			}
			if (tableName.EndsWith("$'")) 
			{
				tableName = tableName.Substring(1, tableName.Length - 3);
			}
			if (workSheetPrefixPattern.IsMatch(tableName))
			{
				tableName = tableName.Split(new char[] {' ','@'})[1];
			}
			table.TableName = tableName;

			int rowCount = table.Rows.Count;
			if (rowCount > 0) 
			{
				SetupColumns(table, table.Columns);
			}
		}

		private void SetupColumns(DataTable table, DataColumnCollection columns) 
		{
			for (int i = 0; i < columns.Count; i++)
			{
				DataColumn column = columns[i];
				string columnName = column.ColumnName.Trim();

				bool isRemove = false;

				if (string.Empty == columnName) 
				{
					isRemove = true;
				}

				if (illigalColumnNamePattern.IsMatch(columnName)) 
				{
					isRemove = true;
				}

				if (isRemove) 
				{
					columns.Remove(column);
					i--;
				}
			}
		}
	}
}
