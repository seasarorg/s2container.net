using System;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Container;

namespace Seasar.Quill
{
    /// <summary>
    /// S2Containerと連携する為の静的クラスです
    /// </summary>
    /// <remarks>
    /// <see cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory"/>
    /// で作成された<see cref="Seasar.Framework.Container.IS2Container"/>を扱います
    /// </remarks>
    public static class SingletonS2ContainerConnector
    {
        /// <summary>
        /// S2Containerのコンポーネントをコンポーネント名を指定して取得します
        /// </summary>
        /// <remarks>
        /// see cref="Seasar.Framework.Container.Factory.SingletonS2ContainerFactory"/>
        /// で作成された<see cref="Seasar.Framework.Container.IS2Container"/>
        /// からコンポーネントを取得します
        /// </remarks>
        /// <param name="componentName">コンポーネント名</param>
        /// <returns>コンポーネントのインスタンス</returns>
        public static object GetComponent(string componentName)
        {
            if (!SingletonS2ContainerFactory.HasContainer)
            {
                // S2Containerが作成されていない場合は例外をスローします
                throw new QuillApplicationException("EQLL0009");
            }

            // S2Containerを取得する
            IS2Container container = SingletonS2ContainerFactory.Container;

            if (!container.HasComponentDef(componentName))
            {
                // S2Containerにコンポーネントが登録されていない場合は例外をスローする
                throw new QuillApplicationException("EQLL0010", 
                    new string[] { componentName });
            }

            try
            {
                // S2Containerから取得したコンポーネントを返す
                return container.GetComponent(componentName);
            }
            catch(Exception ex)
            {
                // コンポーネントの取得で例外が発生した場合は例外をスローする
                throw new QuillApplicationException("EQLL0011", new string[] { }, ex);
            }
        }
    }
}
