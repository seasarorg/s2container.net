#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;

namespace Seasar.Extension.ADO
{
	/// <summary>
	/// DataProvider ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class DataProvider
	{
		private string connectionType_;
		private string commandType_;
		private string parameterType_;
		private string dataAdapterType_;

		public DataProvider()
		{
		}

		public string ConnectionType
		{
			set { connectionType_ = value; }
			get { return connectionType_; }
		}

		public string CommandType
		{
			set { commandType_ = value; }
			get { return commandType_; }
		}

		public string ParameterType
		{
			set { parameterType_ = value; }
			get { return parameterType_; }
		}

		public string DataAdapterType
		{
			set { dataAdapterType_ = value; }
			get { return dataAdapterType_; }
		}
	}
}
