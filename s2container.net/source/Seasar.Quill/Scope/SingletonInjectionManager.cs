using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Scope
{
    /// <summary>
    /// コンテナのインスタンスがsingletonであることを前提にしたインジェクション管理クラス
    /// </summary>
    public class SingletonInjectionManager
    {
        //private readonly ConcurrentDictionary<Type, Type> _injectedMap = new ConcurrentDictionary<Type, Type>();

        //public virtual bool IsInject(object target, Type memberType, Seasar.Quill.QuillInjector.InjectionParameter parameter)
        //{
        //    if (_injectedMap.ContainsKey(memberType))
        //    {
        //        return false;
        //    }
        //    _injectedMap[memberType] = memberType;
        //    return false;
        //}

        //public virtual void Clear()
        //{
        //    _injectedMap.Clear();
        //}
    }
}
