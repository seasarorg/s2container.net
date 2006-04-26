using System;
using System.Collections;
using Seasar.Framework.Container;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// AopProxyを用いてAspectを織り込む処理を持つクラス
    /// </summary>
	public class AopProxyAspectWeaver : AbstractAspectWeaver
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

            Type[] interfaces = componentDef.ComponentType.GetInterfaces();
            if (componentDef.ComponentType.IsMarshalByRef)
            {
                AopProxy aopProxy = new AopProxy(componentDef.ComponentType,
                    GetAspects(componentDef), parameters, target);
                componentDef.AddProxy(componentDef.ComponentType, aopProxy.Create());
            }
            else if (componentDef.ComponentType.IsInterface)
            {
                AopProxy aopProxy = new AopProxy(componentDef.ComponentType,
                    GetAspects(componentDef), parameters, target);
                target = aopProxy.Create();
            }
            foreach (Type interfaceType in interfaces)
            {
                AopProxy aopProxy = new AopProxy(interfaceType,
                    GetAspects(componentDef), parameters, target);
                componentDef.AddProxy(interfaceType, aopProxy.Create());
            }
        }
	}
}
