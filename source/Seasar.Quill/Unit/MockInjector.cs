#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

using System.Reflection;
using Seasar.Quill.Attrs;
using Seasar.Quill.Util;

namespace Seasar.Quill.Unit
{
    /// <summary>
    /// MockをInjectするためのクラス
    /// </summary>
    public class MockInjector : QuillInjector
    {
        // MockInjectorのインスタンス
        private static MockInjector mockInjector;

        /// <summary>
        /// MockInjectorを初期化するコンストラクタ
        /// </summary>
        /// <remarks>
        /// <see cref="GetInstance"/>からインスタンスを生成する
        /// </remarks>
        /// <seealso cref="Seasar.Quill.Unit.MockInjector.GetInstance"/>
        protected MockInjector()
        {
        }

        /// <summary>
        /// MockInjectorのインスタンスを取得する
        /// </summary>
        /// <remarks>
        /// <para>
        /// MockInjectorのコンストラクタのアクセス修飾子はprotectedに設定されている為、
        /// 直接MockInjectorのインスタンスを生成することはできない。
        /// </para>
        /// <para>
        /// MockInjectorのインスタンスを取得する場合は当メソッドを使用する。
        /// </para>
        /// <para>
        /// 基本的に同じインスタンスを返すが、DestroyメソッドによってQuillが持つ
        /// 参照が破棄されている場合は新しいMockInjectorのインスタンスを作成する。
        /// </para>
        /// </remarks>
        /// <returns>QuillInjectorのインスタンス</returns>
        public new static MockInjector GetInstance()
        {
            if (mockInjector == null)
            {
                mockInjector = new MockInjector();
            }

            // MockInjectorのインスタンスを返す
            return mockInjector;
        }

        /// <summary>
        /// QuillInjectorが持つ参照を破棄する
        /// </summary>
        public override void Destroy()
        {
            if (container == null)
            {
                return;
            }

            // QuillContainerが持つ参照を破棄する
            container.Destroy();

            container = null;
            mockInjector = null;
        }

        /// <summary>
        /// 指定するフィールドにDIする。
        /// </summary>
        /// <remarks>Mock属性が指定されている場合はMock属性で指定されているMockクラスを
        /// 優先してInjectする。</remarks>
        /// <param name="target">DIが行われるオブジェクト</param>
        /// <param name="field">DIが行われるフィールド情報</param>
        protected override void InjectField(object target, FieldInfo field)
        {
            // フィールドの型に設定されているMock属性を取得する
            MockAttribute mockAttr = AttributeUtil.GetMockAttr(field.FieldType);

            if (mockAttr != null)
            {
                // MockをInjectする
                InjectField(target, field, mockAttr.MockType);
            }
            else
            {
                // Mock属性が指定されていない場合は通常の処理を行う
                base.InjectField(target, field);
            }
        }
    }
}
