using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Quill.Container.Impl.ComponentCreator
{
    /// <summary>
    /// Castle.DynamicProxyを使用したProxyオブジェクト生成クラス
    /// </summary>
    public class CastleDynamicProxyCreator : IComponentCreater
    {
        public bool IsTarget(Type t)
        {
            throw new NotImplementedException();
        }

        public object Create(Type t)
        {
            ProxyGenerator generator = new ProxyGenerator();
            throw new NotImplementedException();
        }

        public object Create(Type i, Type impl)
        {
            throw new NotImplementedException();
        }
    }
}
