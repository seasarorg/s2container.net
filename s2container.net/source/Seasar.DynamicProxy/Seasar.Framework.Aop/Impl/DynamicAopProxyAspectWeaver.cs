using System;
using System.Collections;
using System.Collections.Generic;
using Seasar.Framework.Container;
using Seasar.Framework.Aop.Proxy;
using System.Reflection;
using Seasar.Framework.Util;

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
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        public override object WeaveAspect(IComponentDef componentDef, ConstructorInfo constructor, object[] args)
        {
            object target = null;
            if (componentDef.AspectDefSize == 0)
            {
                target = ConstructorUtil.NewInstance(constructor, args);
                return target;
            }

            if (!componentDef.ComponentType.IsInterface)
            {
                DynamicAopProxy aopProxy = GetAopProxy(target, componentDef);
                target = aopProxy.Create(Type.GetTypeArray(args), args);

            }
            else
            {
                target = new object();
                this.AddProxy(target, componentDef, componentDef.ComponentType);
            }

            Type[] interfaces = componentDef.ComponentType.GetInterfaces();

            foreach (Type interfaceType in interfaces)
            {
                this.AddProxy(target, componentDef, interfaceType);
            }

            return target;
        }

        /// <summary>
        /// コンポーネント定義にProxyを追加する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="type">コンポーネント定義に追加するProxyのType</param>
        protected void AddProxy(object target, IComponentDef componentDef, Type type)
        {
            DynamicAopProxy aopProxy = GetAopProxy(target, componentDef);

            componentDef.AddProxy(type, aopProxy.Create(type, target));
        }

        /// <summary>
        /// Proxyを作成する
        /// </summary>
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <returns>Proxy</returns>
        protected DynamicAopProxy GetAopProxy(object target, IComponentDef componentDef)
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
    }
}
