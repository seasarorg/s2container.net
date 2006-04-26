using System;
using System.Collections;
using Seasar.Framework.Container;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// DynamicAopProxyを用いてAspectを織り込む処理を持つクラス
    /// </summary>
	public class DynamicAopProxyAspectWeaver : AbstractAspectWeaver
	{
        /// <summary>
        /// AopProxyを用いてAspectを織り込む
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        public override void WeaveAspect(ref object target, Seasar.Framework.Container.IComponentDef componentDef)
        {
            if (componentDef.AspectDefSize == 0) return;
            Hashtable parameters = new Hashtable();
            parameters[ContainerConstants.COMPONENT_DEF_NAME] = componentDef;

            DynamicAopProxy aopProxy = new DynamicAopProxy(componentDef.ComponentType,
                GetAspects(componentDef), parameters, target);
            target = aopProxy.Create();
            componentDef.AddProxy(componentDef.ComponentType, target);
        }
	}
}
