#region Copyright

/*
 * Copyright 2006 the Seasar Foundation and the Others.
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
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Forms;
using log4net;
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
        private IS2Container container_;

        //ログ(log4net)
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="container">DIコンテナ</param>
        public FormInterceptor(IS2Container container)
        {
            container_ = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>DialogResult</returns>
        /// <remarks>Defaultの戻り値はDialgResult.No</remarks>
        public override object Invoke(IMethodInvocation invocation)
        {
            DialogResult ret = DialogResult.No;

            // メソッドの引数値の取得
            object[] args = invocation.Arguments;
            ParameterInfo[] pis = invocation.Method.GetParameters();
            Hashtable hashOfParams = CollectionsUtil.CreateCaseInsensitiveHashtable();
            IList listOfParams = new ArrayList();
            foreach (ParameterInfo pi in pis)
            {
                hashOfParams.Add(pi.Name, args[pi.Position]);
                listOfParams.Add(pi.Name);
            }

            // WindowsFormの表示
            object[] attributes = invocation.Method.GetCustomAttributes(false);
            foreach (object o in attributes)
            {
                if ( o is TargetFormAttribute )
                {
                    TargetFormAttribute attribute = (TargetFormAttribute) o;
                    Type formType = attribute.FormType;
                    Form form = (Form) container_.GetComponent(formType);
                    for ( int i = 0; i < listOfParams.Count; i++ )
                    {
                        PropertyInfo property = form.GetType().GetProperty((string) listOfParams[i]);
                        property.SetValue(form, hashOfParams[listOfParams[i]], null);
                    }

                    if ( attribute.Mode == ModalType.Modal )
                        ret = form.ShowDialog();
                    else
                        form.Show();
                }
            }

            return ret;
        }
    }
}