﻿#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;
using Seasar.Quill.Exception;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// QuillComponentに関するユーティリティクラス
    /// </summary>
    public class ComponentUtil
    {
        /// <summary>
        /// QuillContainerから指定した型のコンポーネントを取り出す(NotNull)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="componentType"></param>
        /// <returns></returns>
        /// <exception>
        /// コンポーネントが取得できなかった場合
        ///     <cref>Seasar.Quill.Exception.QuillComponentInvalidCastException</cref>
        /// </exception>
        public static object GetComponent(QuillContainer container, Type componentType)
        {
            var qc = container.GetComponent(componentType);
            var retComponent = qc.GetComponentObject(componentType);
            //  実質的にありえないが念のためチェック
            if (retComponent == null)
            {
                throw new QuillInvalidClassException(componentType);
            }
            else if(componentType.IsAssignableFrom(retComponent.GetExType()) == false)
            {
                throw new QuillInvalidClassException(retComponent.GetExType(), componentType);
            }
            return retComponent;
        }

        /// <summary>
        /// QuillContainerから指定した型のコンポーネントを取り出す(NotNull)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="interfaceType"></param>
        /// <param name="implType"></param>
        /// <returns></returns>
        /// <exception cref="Seasar.Quill.Exception.QuillInvalidClassException">
        /// コンポーネントが取得できなかった場合
        /// </exception>
        public static object GetComponent(QuillContainer container, Type interfaceType, Type implType)
        {
            var qc = container.GetComponent(interfaceType, implType);
            var retComponent = qc.GetComponentObject(interfaceType);
            //  実質的にありえないが念のためチェック
            if (retComponent == null)
            {
                throw new QuillInvalidClassException(interfaceType);
            }
            else if (interfaceType.IsAssignableFrom(retComponent.GetExType()) == false)
            {
                throw new QuillInvalidClassException(retComponent.GetExType(), interfaceType);
            }
            return retComponent;
        }
    }
}
