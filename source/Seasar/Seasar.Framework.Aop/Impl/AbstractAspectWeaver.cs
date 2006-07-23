using System;
using Seasar.Framework.Container;
using System.Reflection;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// Aspectを織り込む処理を持つ抽象クラス
    /// </summary>
	public abstract class AbstractAspectWeaver : IAspectWeaver
	{
        /// <summary>
        /// Aspectを織り込む
        /// </summary>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        public abstract object WeaveAspect(IComponentDef componentDef, 
            ConstructorInfo constructor, object[] args);

        /// <summary>
        /// コンポーネント定義に設定されているAspectを取得する
        /// </summary>
        /// <param name="componentDef">コンポーネント定義</param>
        /// <returns>Aspectの配列</returns>
        protected IAspect[] GetAspects(IComponentDef componentDef)
        {
            int size = componentDef.AspectDefSize;
            IAspect[] aspects = new IAspect[size];
            for (int i = 0; i < size; ++i)
            {
                aspects[i] = componentDef.GetAspectDef(i).Aspect;
            }
            return aspects;
        }

	}
}
