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

            Type[] interfaces = componentDef.ComponentType.GetInterfaces();

            foreach (Type interfaceType in interfaces)
            {
                this.AddProxy(ref target, componentDef, interfaceType, parameters);
            }
            if (!componentDef.ComponentType.IsInterface)
            {
                this.AddProxy(ref target, componentDef, componentDef.ComponentType, parameters);
            }
        }

        /// <summary>
        /// コンポーネント定義にProxyを追加する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="type">コンポーネント定義に追加するProxyのType</param>
        /// <param name="parameters">パラメータ</param>
        protected void AddProxy(ref object target, IComponentDef componentDef, Type type, Hashtable parameters)
        {
            DynamicAopProxy aopProxy = new DynamicAopProxy(type,
                    GetAspects(componentDef), parameters, target);
            componentDef.AddProxy(type, aopProxy.Create());
        }
	}
}
