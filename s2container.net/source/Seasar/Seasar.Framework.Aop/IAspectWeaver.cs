using System;
using Seasar.Framework.Container;

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
        /// <param name="target">Aspectを織り込む対象のオブジェクト</param>
        /// <param name="componentDef">Aspectを織り込む対象のコンポーネント定義</param>
        void WeaveAspect(ref object target, IComponentDef componentDef);
	}
}
