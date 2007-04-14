using System;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// このインターフェースは、 コンポーネントの状態に対するアクセスタイプ定義を表します。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 与えられたコンポーネントに対し、 アクセスタイプ定義に基づいて、
    /// S2コンテナ上のコンポーネントをインジェクションする機能も提供します。
    /// </para>
    /// <para>アクセスタイプ定義には、 以下のものがあります。</para>
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.Assembler.IAccessTypePropertyDef">property</see></term>
    /// <description>プロパティによるアクセスを表します。</description>
    /// </item>
    /// <item>
    /// <term><see cref="Seasar.Framework.Container.Assembler.IAccessTypeFieldDef">field</see></term>
    /// <description>フィールドへの直接アクセスを表します。</description>
    /// </item>
    /// </list>
    /// <para>
    /// アクセスタイプ定義は、
    /// <see cref="Seasar.Framework.Container.Assembler.IAccessTypeDefFactory">ファクトリ</see>
    /// 経由で取得します。
    /// </para>
    /// <remarks>
    public interface IAccessTypeDef
    {
        /// <summary>
        /// アクセスタイプ定義名を返します。
        /// </summary>
        /// <value>アクセスタイプ定義名</value>
        /// <seealso cref="AccessTypeDefConstants.PROPERTY_NAME"/>
        /// <seealso cref="AccessTypeDefConstants.FIELD_NAME"/>
        string Name { get; }

        /// <summary>
        /// アクセスタイプ定義に基づいて、 <code>component</code>のプロパティ
        /// またはフィールドにS2コンテナ上のコンポーネントをインジェクションします。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <param name="propertyDef">プロパティ定義</param>
        /// <param name="component">コンポーネント</param>
        void Bind(IComponentDef componentDef, IPropertyDef propertyDef,
            object component);

        /// <summary>
        /// アクセスタイプ定義に基づいて、 <code>component</code>のプロパティ
        /// またはフィールドにS2コンテナ上のコンポーネントをインジェクションします。
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <param name="propertyDef">プロパティ定義</param>
        /// <param name="bindingTypeDef">バインディングタイプ定義</param>
        /// <param name="component">コンポーネント</param>
        void Bind(IComponentDef componentDef, IPropertyDef propertyDef,
            IBindingTypeDef bindingTypeDef, object component);
    }
}
