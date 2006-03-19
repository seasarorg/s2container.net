using System;
using System.Collections;
using System.Data;
using MbUnit.Core.Exceptions;
using MbUnit.Framework;

namespace Seasar.Extension.Unit
{
	public sealed class S2Assert
	{
		private S2Assert()
		{
		}

		#region AreEqual

		/// <summary>
		/// DataSet同士を比較します。
		/// 
		/// カラムの並び順は比較に影響しません。 
		/// 数値は全てBigDecimalとして比較します。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		public static void AreEqual(DataSet expected, DataSet actual) 
		{
			AreEqual(expected, actual, null);
		}

		/// <summary>
		/// DataSet同士を比較します。
		/// 
		/// カラムの並び順は比較に影響しません。 
		/// 数値は全てBigDecimalとして比較します。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		public static void AreEqual(DataSet expected, DataSet actual, string message) 
		{
			message = message == null ? string.Empty : message;

			Assert.AreEqual(
				expected.Tables.Count,
				actual.Tables.Count,
				message + ":TableSize"
				);
			for (int i = 0; i < expected.Tables.Count; ++i) 
			{
				AreEqual(expected.Tables[i], actual.Tables[i], message);
			}
		}

		/// <summary>
		/// DataTable同士を比較します。
		/// 
		/// カラムの並び順は比較に影響しません。 
		/// 数値は全てBigDecimalとして比較します。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		public static void AreEqual(DataTable expected, DataTable actual) 
		{
			AreEqual(expected, actual, null);
		}

		/// <summary>
		/// DataTable同士を比較します。
		/// 
		/// カラムの並び順は比較に影響しません。 
		/// 数値は全てBigDecimalとして比較します。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		public static void AreEqual(DataTable expected, DataTable actual, string message) 
		{
			message = message == null ? string.Empty : message;
			message = message + ":TableName=" + expected.TableName;
			Assert.AreEqual(expected.Rows.Count, actual.Rows.Count, message + ":RowSize");
			for (int i = 0; i < expected.Rows.Count; ++i) 
			{
				DataRow expectedRow = expected.Rows[i];
				DataRow actualRow = actual.Rows[i];
				IList errorMessages = new ArrayList();
				for (int j = 0; j < expected.Columns.Count; ++j) 
				{
					try 
					{
						string columnName = expected.Columns[j].ColumnName;
						object expectedValue = expectedRow[columnName];
						object actualValue = actualRow[columnName];
						// TOOD ColumnType ct = ColumnTypes.getColumnType(expectedValue); impl
						if (!expectedValue.Equals(actualValue)) 
						{
							Assert.AreEqual(
								expectedValue,
								actualValue,
								message + ":Row=" + i + ":columnName=" + columnName
								);
						}
					} 
					catch (AssertionException e) 
					{
						errorMessages.Add(e.Message);
					}
				}
				if (errorMessages.Count != 0) 
				{
					Assert.Fail(message + errorMessages);
				}
			}
		}

		/// <summary>
		/// オブジェクトをDataSetと比較します。
		/// 
		/// オブジェクトは、Bean、Hashtable、BeanのIList、HashtableのIListのいずれか でなければなりません。
		/// 
		/// Beanの場合はプロパティ名を、Mapの場合はキーをカラム名として 比較します。
		/// カラムの並び順は比較に影響しません。 
		/// 数値は全てBigDecimalとして比較します。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		public static void AreEqual(DataSet expected, object actual) 
		{
			AreEqual(expected, actual, null);
		}

		/// <summary>
		/// オブジェクトをDataSetと比較します。
		/// 
		/// オブジェクトは、object、IDictionary、objectのIList、IDictionaryのIListのいずれか でなければなりません。
		/// 
		/// objectの場合はプロパティ名を、IDictionaryの場合はキーをカラム名として 比較します。
		/// カラムの並び順は比較に影響しません。 
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="actual">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		public static void AreEqual(DataSet expected, object actual, string message) 
		{
			if (expected == null || actual == null) 
			{
				Assert.AreEqual(expected, actual, message);
				return;
			}

			if (actual is IList) 
			{
				IList actualList = (IList) actual;
				Assert.IsTrue(actualList.Count != 0);
				object actualItem = actualList[0];
				if (actualItem is IDictionary) 
				{
					AreDictionaryListEqual(expected, actualList, message);
				} 
				else 
				{
					AreBeanListEqual(expected, actualList, message);
				}
			} 
			else if (actual is object[]) 
			{
				AreEqual(expected, new ArrayList((object[]) actual), message);
			} 
			else 
			{
				if (actual is IDictionary) 
				{
					AreDictionaryEqual(expected, (IDictionary) actual, message);
				} 
				else 
				{
					AreBeanEqual(expected, actual, message);
				}
			}
		}

		/// <summary>
		/// IDictionaryをDataSetと比較します。
		/// 
		/// IDictionaryのキーをカラム名として比較します。
		/// カラムの並び順は比較に影響しません。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="dictionary">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		private static void AreDictionaryEqual(DataSet expected, IDictionary dictionary, string message) 
		{
			DictionaryReader reader = new DictionaryReader(dictionary);
			AreEqual(expected, reader.Read(), message);
		}

		/// <summary>
		/// IDictionaryのIListをDataSetと比較します。
		/// 
		/// IDictionaryのキーをカラム名として比較します。
		/// カラムの並び順は比較に影響しません。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="list">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		private static void AreDictionaryListEqual(DataSet expected, IList list, string message) 
		{
			DictionaryListReader reader = new DictionaryListReader(list);
			AreEqual(expected, reader.Read(), message);
		}

		/// <summary>
		/// objectをDataSetと比較します。
		/// 
		/// objectのプロパティ名をカラム名として比較します。
		/// カラムの並び順は比較に影響しません。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="bean">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		private static void AreBeanEqual(DataSet expected, object bean, string message) 
		{
			BeanReader reader = new BeanReader(bean);
			AreEqual(expected, reader.Read(), message);
		}

		/// <summary>
		/// objectのIListをDataSetと比較します。
		/// 
		/// objectのプロパティ名をカラム名として比較します。
		/// カラムの並び順は比較に影響しません。
		/// </summary>
		/// <param name="expected">予測値</param>
		/// <param name="list">実際値</param>
		/// <param name="message">assert失敗時のメッセージ</param>
		private static void AreBeanListEqual(DataSet expected, IList list, string message) 
		{
			BeanListReader reader = new BeanListReader(list);
			AreEqual(expected, reader.Read(), message);
		}

		#endregion
	}
}
