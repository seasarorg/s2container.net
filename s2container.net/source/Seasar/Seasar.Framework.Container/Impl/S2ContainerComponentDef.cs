#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Container.Impl
{
    public class S2ContainerComponentDef : SimpleComponentDef
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">S2Container</param>
        /// <param name="name"></param>
        public S2ContainerComponentDef(IS2Container container, string name)
            : base(container, name)
        {
        }

        /// <summary>
        /// S2Container
        /// </summary>
        public new IS2Container Container
        {
            get { return (IS2Container) base.GetComponent(); }
        }

        /// <summary>
        /// コンポーネント
        /// </summary>
        public override object GetComponent()
        {
            return Container;
        }

        public new void Init()
        {
            Container.Init();
        }

        public new void Destroy()
        {
            Container.Destroy();
        }
    }
}
