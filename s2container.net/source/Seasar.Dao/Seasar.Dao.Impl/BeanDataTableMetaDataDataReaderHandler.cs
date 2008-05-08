using System;
using System.Text;
using System.Data;
using Seasar.Framework.Util;
using System.Collections;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class BeanDataTableMetaDataDataReaderHandler : IDataReaderHandler
    {
        private Type _returnType;

        public BeanDataTableMetaDataDataReaderHandler(Type returnType)
        {
            _returnType = returnType;
        }

        public virtual object Handle(IDataReader dataReader)
        {
            DataTable table = (DataTable)Activator.CreateInstance(_returnType);
            table.Load(dataReader);
            return table;
        }
    }
}
