#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

using System;
using Seasar.Unit.Core;

namespace Seasar.Quill.Unit
{
    /// <summary>
    /// Quillを使用したテスト補助クラス
    /// </summary>
    /// <remarks>
    /// 継承して使用
    /// S2Containerへの登録やコンポーネントの取得等をショートカット
    /// </remarks>
    public class QuillTestCase : S2TestCaseBase
    {
        public QuillContainer QContainer
        {
            get { return QuillInjector.GetInstance().Container; }
        }

        public QuillInjector Injector
        {
            get { return QuillInjector.GetInstance(); }
        }

        public object GetQuillComponent(Type componentClass)
        {
            var component = QContainer.GetComponent(componentClass);
            return component.GetComponentObject(componentClass);
        }

        public object GetQuillComponent(Type interfaceType, Type implType)
        {
            var component = QContainer.GetComponent(interfaceType, implType);
            return component.GetComponentObject(component.ReceiptType);
        }

        public virtual void Inject(object target)
        {
            Injector.Inject(target);
        }
    }
}
