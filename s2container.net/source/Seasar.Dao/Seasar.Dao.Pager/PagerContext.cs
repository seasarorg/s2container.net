#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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

using System;
using System.Collections;
using System.Threading;

namespace Seasar.Dao.Pager
{
    public class PagerContext
    {
        private static LocalDataStoreSlot _slot;
        private readonly Stack _argsStack = new Stack();

        static PagerContext()
        {
            _slot = Thread.AllocateDataSlot();
        }

        private PagerContext()
        {
        }

        /// <summary>
        /// 現在のスレッドに結びついたPagerContextを取得します。
        /// </summary>
        /// <returns></returns>
        public static PagerContext GetContext()
        {
            object o = Thread.GetData(_slot);
            if (o == null)
            {
                PagerContext context = new PagerContext();
                Thread.SetData(_slot, context);
                o = context;
            }
            return o as PagerContext;
        }

        public void PushArgs(IPagerCondition condition)
        {
            _argsStack.Push(condition);
        }

        public IPagerCondition PopArgs()
        {
            return (IPagerCondition) _argsStack.Pop();
        }

        public IPagerCondition PeekArgs()
        {
            if (_argsStack.Count == 0)
            {
                return null;
            }
            else
            {
                return (IPagerCondition) _argsStack.Peek();
            }
        }
    }
}
