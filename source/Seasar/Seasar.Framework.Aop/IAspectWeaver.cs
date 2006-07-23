using System;
using Seasar.Framework.Container;
using System.Reflection;

namespace Seasar.Framework.Aop
{
    /// <summary>
    /// Aspectを織り込む処理を持つInterface
    /// </summary>
	public interface IAspectWeaver
	{
        /// <summary>
        /// Aspectを織り込む
        /// </summary>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        /// <param name="constructor">コンストラクタ</param>
        /// <param name="args">コンストラクタの引数</param>
        /// <returns>Aspectを織り込んだオブジェクト</returns>
        object WeaveAspect(IComponentDef componentDef, ConstructorInfo constructor, object[] args);
	}
}
