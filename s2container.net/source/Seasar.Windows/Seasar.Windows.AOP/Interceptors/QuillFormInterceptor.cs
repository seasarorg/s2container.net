#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill;
using Seasar.Windows.Attr;

namespace Seasar.Windows.AOP.Interceptors
{
    /// <summary>
    /// 指定されたFormを表示するQuill用Interceptor
    /// </summary>
    public class QuillFormInterceptor : AbstractInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>DialogResult</returns>
        /// <remarks>Defaultの戻り値はDialgResult.No</remarks>
        public override object Invoke(IMethodInvocation invocation)
        {
            DialogResult retOfReplace = DialogResult.No;

            object ret = retOfReplace;

            // メソッドの引数値の取得
            object[] args = invocation.Arguments;
            ParameterInfo[] pis = invocation.Method.GetParameters();

            IDictionary<string, object> hashOfParams = new Dictionary<string, object>();
            IList<string> listOfParams = new List<string>();
            IDictionary<string, string> hashOfPropNames = new Dictionary<string, string>();

            foreach (ParameterInfo pi in pis)
            {
                hashOfParams.Add(pi.Name.ToLower(), args[pi.Position]);
                listOfParams.Add(pi.Name.ToLower());
            }

            // WindowsFormの表示
            object[] attributes = invocation.Method.GetCustomAttributes(false);
            foreach (object o in attributes)
            {
                if (o is TargetFormAttribute)
                {
                    // 戻り値のプロパティ名を取得する
                    TargetFormAttribute attribute = (TargetFormAttribute)o;
                    Type formType = attribute.FormType;

                    // Formオブジェクトの取得
                    QuillInjector injector = QuillInjector.GetInstance();
                    QuillContainer container = injector.Container;
                    QuillComponent component = container.GetComponent(formType);
                    Form form;
                    if (component != null)
                    {
                        form = (Form) component.GetComponentObject(formType);
                        if (form == null)
                        {
                            form = (Form)Activator.CreateInstance(formType);
                            injector.Inject(form);
                        }
                    }
                    else
                    {
                        form = (Form)Activator.CreateInstance(formType);
                        injector.Inject(form);
                    }

                    // 戻り値プロパティ名の取得
                    string propertyName = String.Empty;
                    if (attribute.ReturnPropertyName != String.Empty)
                        propertyName = attribute.ReturnPropertyName;

                    // フォームに値をセットする
                    PropertyInfo[] infos = form.GetType().GetProperties();
                    foreach (PropertyInfo info in infos)
                    {
                        hashOfPropNames.Add(info.Name.ToLower(), info.Name);
                    }

                    for (int i = 0; i < listOfParams.Count; i++)
                    {
                        if (hashOfPropNames.ContainsKey(listOfParams[i]))
                        {
                            string propName = hashOfPropNames[listOfParams[i]];
                            PropertyInfo property = form.GetType().GetProperty(propName);
                            property.SetValue(form, hashOfParams[listOfParams[i]], null);
                        }
                    }

                    // WindowsFormの表示
                    if (attribute.Mode == ModalType.Modal)
                    {
                        retOfReplace = form.ShowDialog();
                        ret = retOfReplace;
                        // WindowsFormから戻り値用プロパティから値を取得する
                        if (propertyName != String.Empty)
                        {
                            PropertyInfo propInfo = form.GetType().GetProperty(propertyName);
                            if (propInfo != null)
                                ret = propInfo.GetValue(form, null);
                        }
                    }
                    else
                    {
                        form.Show();
                    }
                }
            }

            return ret;
        }
    }
}