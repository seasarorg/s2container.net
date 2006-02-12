using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace Seasar.Extension.Unit
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public sealed class S2Attribute : DecoratorPatternAttribute
	{
		private Tx tx;

		public S2Attribute()
		{
			this.tx = Tx.NotSupported;
		}

		public S2Attribute(Tx tx)
		{
			this.tx = tx;
		}

		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new S2TestCaseRunInvoker(invoker, tx);
		}
	}

	public enum Tx
	{
		Rollback,
		Commit,
		NotSupported
	}
}