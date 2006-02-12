using System;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Unit;
using Seasar.Framework.Util;

namespace Seasar.Extension.Unit
{
	/// <summary>
	/// S2TestCase ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class S2TestCase : S2FrameworkTestCaseBase
	{
   
		private IDataSource dataSource;

		private IDbConnection connection;

		//private DatabaseMetaData dbMetaData;

		public S2TestCase()
		{

		}
		public IDataSource DataSource
		{
			get
			{
				if (dataSource == null)
					throw new EmptyRuntimeException("dataSource");
				return dataSource;
			}
		}

		public IDbConnection Connection
		{
			get
			{
				if (connection != null)
					return connection;
				connection = DataSourceUtil.GetConnection(dataSource);
				return connection;
			}
		}

		internal void SetConnection(IDbConnection connection)
		{
			this.connection = connection;
		}

		internal void SetDataSource(IDataSource dataSource)
		{
			this.dataSource = dataSource;
		}
	}
}
