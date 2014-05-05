using Seasar.Quill.Exception;
using System;
using System.Reflection;

namespace Seasar.Quill.Container.Impl.ComponentCreator
{
    /// <summary>
    /// インスタンス生成クラス
    /// </summary>
    public class NewInstanceCreator : IComponentCreater
    {
        public bool IsTarget(Type t)
        {
            // デフォルトのインスタンス生成クラスとして機能
            return true;
        }

        public object Create(Type t)
        {
            return CreateInstance(t);
        }

        public object Create(Type i, Type impl)
        {
            return CreateInstance(impl);
        }

        private object CreateInstance(Type t)
        {
            // 引数を持たないコンストラクタの取得
            ConstructorInfo constructor = t.GetConstructor(new Type[] { });

            if (constructor == null)
            {
                // 引数なしのコンストラクタを持たないクラスは対象外。例外をスロー
                // TODO 例外スロー
            }

            try
            {
                return Activator.CreateInstance(t);
            }
            catch(System.Exception ex)
            {
                // TODO 例外スロー
                throw new QuillApplicationException("", ex);
            }
        }
    }
}
