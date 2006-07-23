using System;
using System.Collections;
using Seasar.Framework.Container;
using Seasar.Framework.Aop.Proxy;
using System.Reflection;
using Seasar.Framework.Util;

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
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        public override object WeaveAspect(IComponentDef componentDef, ConstructorInfo constructor, object[] args)
        {
            object target;
            
            if (componentDef.ComponentType.IsInterface)
            {
                target = new object();
            }
            else
            {
                target = ConstructorUtil.NewInstance(constructor, args);
            }

            if (componentDef.AspectDefSize == 0)
            {
                return target;
            }

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

            return target;
        }
	}
}
