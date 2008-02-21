using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Framework.Aop;
using Seasar.Extension.Tx;

namespace Seasar.Quill.Tx
{
    /// <summary>
    /// Quill用TransactionInterceptor
    /// やっていることはS2ContainerのTransactionInterceptorと
    /// 全く同じ
    /// </summary>
    public class QuillTransactionInterceptor : IMethodInterceptor
    {
        /// <summary>
        /// 本来のTransactionInterceptorのインスタンスを保持する
        /// </summary>
        private TransactionInterceptor s2daoTxInterceptor = null;

        private ITransactionHandler transactionhandler = null;
        private ITransactionStateHandler tansactionstatehandler = null;

        /// <summary>
        /// S2DaoのTransactionInterceptorを利用する
        /// （TransactionInterceptorのTransactionHandlerのreadonlyに
        /// こだわらなければ、TransactionInterceptorを使わない手もあり）
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public object Invoke(IMethodInvocation invocation)
        {
            //  S2DaoのTransactionInterceptorがなかったらインスタンス生成
            //  本クラスをQuillComponentとして扱う場合はsingleton
            //  内部で保持するTransactionInterceptorも実質的にsingletonとなる
            if ( s2daoTxInterceptor == null )
            {
                s2daoTxInterceptor = new TransactionInterceptor(TransactionHandler);
                s2daoTxInterceptor.TransactionStateHandler = TransactionStateHandler;
            }
            return s2daoTxInterceptor.Invoke(invocation);
        }

        public ITransactionHandler TransactionHandler
        {
            get { return transactionhandler; }
            set { transactionhandler = value; }
        }

        public ITransactionStateHandler TransactionStateHandler
        {
            get { return tansactionstatehandler; }
            set { tansactionstatehandler = value; }
        }
    }
}
