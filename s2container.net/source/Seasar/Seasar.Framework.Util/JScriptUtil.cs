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

using Microsoft.JScript;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.CodeDom.Compiler;
using Seasar.Framework.Exceptions;
using System.Configuration;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// CodeDomÇ≈JScript.NETÇàµÇ¶ÇÈÇÊÇ§Ç…ÇµÇ‹Ç∑ÅB
    /// </summary>
    public sealed class JScriptUtil
    {
        private static readonly CodeDomProvider _provider = new JScriptCodeProvider();
        private static readonly Type _evaluateType;

        private const string EVAL_SOURCE = @"
            package Seasar.Framework.Util.JScript
            {
                class Evaluator
                {
                    public static function Eval(expr : String,unsafe : boolean,
                        self : Object,out : Object,err : Object, container : Object,
                        appSettings : Object, connectionStrings : Object) : Object 
                    {
                        if (unsafe)
                        {
                            return eval(expr,'unsafe');
                        }
                        else
                        {
                            return eval(expr); 
                        }
                    }
                }
            }";

        private JScriptUtil()
        {
        }

        static JScriptUtil()
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

#if NET_1_1
            ICodeCompiler compiler = _provider.CreateCompiler();
            CompilerResults results = compiler.CompileAssemblyFromSource(parameters,EVAL_SOURCE);
#else
            CompilerResults results = _provider.CompileAssemblyFromSource(parameters, EVAL_SOURCE);
#endif

            Assembly assembly = results.CompiledAssembly;
            _evaluateType = assembly.GetType("Seasar.Framework.Util.JScript.Evaluator");
        }

        public static object Evaluate(string exp, Hashtable ctx, object root)
        {
#if NET_1_1
            exp = exp.Replace("\r", "\\r");
            exp = exp.Replace("\n", "\\n");
            
            NameValueCollection appSettings = ConfigurationSettings.AppSettings;
#else
            if (exp.Contains("\r"))
            {
                exp = exp.Replace("\r", "\\r");
            }
            if (exp.Contains("\n"))
            {
                exp = exp.Replace("\n", "\\n");
            }

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            ConnectionStringSettingsCollection connectionStrings = 
                ConfigurationManager.ConnectionStrings;
            
#endif

            try
            {
                return _evaluateType.InvokeMember("Eval", BindingFlags.InvokeMethod,
                    null, null, new object[] {exp,true, ctx["self"], ctx["out"], ctx["err"], root,
                    appSettings, connectionStrings});
            }
            catch (Exception ex)
            {
                throw new JScriptEvaluateRuntimeException(exp, ex);
            }
        }

        public static object Evaluate(string exp, object root)
        {
            try
            {
                return Evaluate(exp, new Hashtable(), root);
            }
            catch (Exception ex)
            {
                throw new JScriptEvaluateRuntimeException(exp, ex);
            }
        }
    }
}
