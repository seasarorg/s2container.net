#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container;
using Seasar.Windows.Attr;

namespace Seasar.Windows.AOP.Interceptors
{
    /// <summary>
    /// 指定されたFormを表示するInterceptor
    /// </summary>
    public class FormInterceptor : AbstractInterceptor
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        private readonly IS2Container _container;

        /// <summary>
        /// 返り値用プロパティ名
        /// </summary>
        private string _returnPropertyName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public FormInterceptor(IS2Container container)
        {
            _container = container;
            _returnPropertyName = string.Empty;
        }

        /// <summary>
        /// 返り値用プロパティ名
        /// </summary>
        public string Property
        {
            get { return _returnPropertyName; }
            set { _returnPropertyName = value; }
        }

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
                    TargetFormAttribute attribute = (TargetFormAttribute) o;
                    Type formType = attribute.FormType;
                    Form form = (Form) _container.GetComponent(formType);
                    if (form == null)
                        throw new NullReferenceException(SWFMessages.ASWF0001);
                    

                    string propertyName;
                    if (attribute.ReturnPropertyName != string.Empty)
                    {
                        propertyName = attribute.ReturnPropertyName;
                    }
                    else
                    {
                        if (_returnPropertyName != null)
                            propertyName = _returnPropertyName;
                        else
                            propertyName = string.Empty;
                    }

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
                        if (propertyName != string.Empty)
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
