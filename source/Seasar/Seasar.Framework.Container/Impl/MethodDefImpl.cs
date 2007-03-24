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

using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Impl
{
    public class MethodDefImpl : IMethodDef
    {
        private readonly string _methodName;
        private readonly ArgDefSupport _argDefSupport = new ArgDefSupport();
        private IS2Container _container;
        private string _expression;

        public MethodDefImpl()
        {
        }

        public MethodDefImpl(string methodName)
        {
            _methodName = methodName;
        }

        #region MethodDef ÉÅÉìÉo

        public string MethodName
        {
            get { return _methodName; }
        }

        public object[] Args
        {
            get
            {
                object[] args = new object[ArgDefSize];
                for (int i = 0; i < ArgDefSize; ++i)
                {
                    args[i] = GetArgDef(i).Value;
                }
                return args;
            }
        }

        public IS2Container Container
        {
            get { return _container; }
            set
            {
                _container = value;
                _argDefSupport.Container = value;
            }
        }

        public string Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        #endregion

        #region IArgDefAware ÉÅÉìÉo

        public void AddArgDef(IArgDef argDef)
        {
            _argDefSupport.AddArgDef(argDef);
        }

        public int ArgDefSize
        {
            get { return _argDefSupport.ArgDefSize; }
        }

        public IArgDef GetArgDef(int index)
        {
            return _argDefSupport.GetArgDef(index);
        }

        #endregion
    }
}
