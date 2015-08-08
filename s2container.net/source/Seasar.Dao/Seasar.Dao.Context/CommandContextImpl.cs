#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using System.Reflection;
using System.Text;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Context
{
    public class CommandContextImpl : ICommandContext
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

#if NET_1_1
        readonly Hashtable _args = new Hashtable( new CaseInsensitiveHashCodeProvider(), 
            new CaseInsensitiveComparer() );
        readonly Hashtable _argTypes = new Hashtable( new CaseInsensitiveHashCodeProvider(),
            new CaseInsensitiveComparer() );
        readonly Hashtable _argNames = new Hashtable( new CaseInsensitiveHashCodeProvider(), 
            new CaseInsensitiveComparer() );
#else
        readonly Hashtable _args = new Hashtable(StringComparer.OrdinalIgnoreCase);
        readonly Hashtable _argTypes = new Hashtable(StringComparer.OrdinalIgnoreCase);
        readonly Hashtable _argNames = new Hashtable(StringComparer.OrdinalIgnoreCase);
#endif

        private readonly StringBuilder _sqlBuf = new StringBuilder(100);
        private readonly IList _bindVariables = new ArrayList();
        private readonly IList _bindVariableTypes = new ArrayList();
        private readonly IList _bindVariableNames = new ArrayList();
        private readonly ICommandContext _parent;

        public CommandContextImpl()
        {
        }

        public CommandContextImpl(ICommandContext parent)
        {
            _parent = parent;
            IsEnabled = false;
        }

        public object GetArg(string name)
        {
            if (_args.ContainsKey(name))
            {
                return _args[name];
            }
            else if (_parent != null)
            {
                return _parent.GetArg(name);
            }
            else
            {
                var names = name.Split('.');
                var value = _args[names[0]];
                var type = GetArgType(names[0]);

                for (var pos = 1; pos < names.Length; pos++)
                {
                    if (value == null || type == null) break;
                    var pi = type.GetProperty(names[pos]);
                    if (pi == null)
                    {
                        return null;
                    }
//                    value = pi.GetValue(value, null);
                    value = PropertyUtil.GetValue(value, value.GetExType(), pi.Name);
                    type = pi.PropertyType;
                }
                return value;
            }
        }

        public Type GetArgType(string name)
        {
            if (_argTypes.ContainsKey(name))
            {
                return (Type) _argTypes[name];
            }
            else if (_parent != null)
            {
                return _parent.GetArgType(name);
            }
            else
            {
                _logger.Log("WDAO0001", new object[] { name });
                return null;
            }
        }

        public void AddArg(string name, object arg, Type argType)
        {
            if (_args.ContainsKey(name))
            {
                _args.Remove(name);
            }
            _args.Add(name, arg);

            if (_argTypes.ContainsKey(name))
            {
                _argTypes.Remove(name);
            }
            _argTypes.Add(name, argType);

            if (_argNames.ContainsKey(name))
            {
                _argNames.Remove(name);
            }
            _argNames.Add(name, name);
        }

        public string Sql => _sqlBuf.ToString();

        public object[] BindVariables
        {
            get
            {
                var variables = new object[_bindVariables.Count];
                _bindVariables.CopyTo(variables, 0);
                return variables;
            }
        }

        public Type[] BindVariableTypes
        {
            get
            {
                var variables = new Type[_bindVariableTypes.Count];
                _bindVariableTypes.CopyTo(variables, 0);
                return variables;
            }
        }

        public string[] BindVariableNames
        {
            get
            {
                var variableNames = new string[_bindVariableNames.Count];
                _bindVariableNames.CopyTo(variableNames, 0);
                return variableNames;
            }
        }

        public ICommandContext AddSql(string sql)
        {
            _sqlBuf.Append(sql);
            return this;
        }

        public ICommandContext AddSql(string sql, object bindVariable,
            Type bindVariableType, string bindVariableName)
        {

            _sqlBuf.Append(sql);
            _bindVariables.Add(bindVariable);
            _bindVariableTypes.Add(bindVariableType);
            _bindVariableNames.Add(bindVariableName);
            return this;
        }

        public ICommandContext AddSql(object bindVariable, Type bindVariableType, string bindVariableName)
        {
            AddSql("?", bindVariable, bindVariableType, bindVariableName);
            return this;
        }

        public ICommandContext AddSql(string sql, object[] bindVariables,
            Type[] bindVariableTypes, string[] bindVariableNames)
        {

            _sqlBuf.Append(sql);
            for (var i = 0; i < bindVariables.Length; ++i)
            {
                _bindVariables.Add(bindVariables[i]);
                _bindVariableTypes.Add(bindVariableTypes[i]);
                _bindVariableNames.Add(bindVariableNames[i]);
            }
            return this;
        }

        public ICommandContext AppendSql(object bindVariable, Type bindVariableType, string bindVariableName)
        {
            AddSql(", ?", bindVariable, bindVariableType, bindVariableName);
            return this;
        }

        public bool IsEnabled { get; set; } = true;
    }
}
