using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Container;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// S2Containerを使用してデータソースを返す
    /// </summary>
	public class SelectableDataSourceProxyWithContainer : AbstractSelectableDataSourceProxy
	{
        private IS2Container _container = null;

        public IS2Container Container
        {
            set
            {
                _container = value;
            }
            get
            {
                return _container;
            }
        }

        public override IDataSource GetDataSource(string dataSourceName)
        {
            if ( Container.HasComponentDef(dataSourceName) )
            {
                return (IDataSource)Container.GetComponent(dataSourceName);
            }
            else
            {
                throw new ComponentNotFoundRuntimeException(dataSourceName);
            }
        }
    }
}
