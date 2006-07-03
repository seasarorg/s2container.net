using System;
using System.Collections;
using System.Collections.Generic;
using Seasar.Framework.Container;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// DynamicAopProxyを用いてAspectを織り込む処理を持つクラス
    /// </summary>
    public class DynamicAopProxyAspectWeaver : AbstractAspectWeaver
    {
        private IDictionary<IComponentDef, DynamicAopProxy> aopProxies =
            new Dictionary<IComponentDef, DynamicAopProxy>();

        /// <summary>
        /// AopProxyを用いてAspectを織り込む
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        public override void WeaveAspect(ref object target, IComponentDef componentDef)
        {
            if (componentDef.AspectDefSize == 0) return;

            Type[] interfaces = componentDef.ComponentType.GetInterfaces();

            if (!componentDef.ComponentType.IsInterface)
            {
                DynamicAopProxy aopProxy = GetAopProxy(ref target, componentDef);
                target = aopProxy.Create();

            }
            else
            {
                this.AddProxy(ref target, componentDef, componentDef.ComponentType);
            }

            foreach (Type interfaceType in interfaces)
            {
                this.AddProxy(ref target, componentDef, interfaceType);
            }

        }

        /// <summary>
        /// Proxyを作成する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <returns>Proxy</returns>
        protected DynamicAopProxy GetAopProxy(ref object target, IComponentDef componentDef)
        {
            DynamicAopProxy aopProxy = null;

            if (aopProxies.ContainsKey(componentDef))
            {
                aopProxy = aopProxies[componentDef];
            }
            else
            {
                Hashtable parameters = new Hashtable();
                parameters[ContainerConstants.COMPONENT_DEF_NAME] = componentDef;
                aopProxy = new DynamicAopProxy(componentDef.ComponentType,
                    GetAspects(componentDef), parameters, target);
                aopProxies[componentDef] = aopProxy;
            }

            return aopProxy;
        }

        /// <summary>
        /// コンポーネント定義にProxyを追加する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="type">コンポーネント定義に追加するProxyのType</param>
        protected void AddProxy(ref object target, IComponentDef componentDef, Type type)
        {
            DynamicAopProxy aopProxy = GetAopProxy(ref target, componentDef);

            componentDef.AddProxy(type, aopProxy.Create(type, target));
        }
    }
}
