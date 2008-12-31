#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using MbUnit.Core.Invokers;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Log;
using Seasar.Framework.Message;
using Seasar.Framework.Util;

namespace Seasar.Framework.Unit
{
    public class S2FrameworkTestCaseRunner
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected S2FrameworkTestCaseBase _fixture;
        protected MethodInfo _method;
        private IS2Container _container;
        private IList _bindedFields;
        private Hashtable _errors;

        protected IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public virtual object Run(IRunInvoker invoker, object o, IList args)
        {
            _fixture = o as S2FrameworkTestCaseBase;
            _method = _fixture.GetType().GetMethod(invoker.Name);
            SetUpContainer();
            _fixture.Container = _container;
            try
            {
                try
                {
                    SetUpMethod();
                    SetUpForEachTestMethod();
                    _container.Init();
                    try
                    {
                        SetUpAfterContainerInit();
                        try
                        {
                            BindFields();
                            SetUpAfterBindFields();
                            try
                            {
                                BeginTransactionContext();
                                return invoker.Execute(o, args);
                            }
                            catch (Exception e)
                            {
                                ExceptionHandler(e);
                                throw;
                            }
                            finally
                            {
                                EndTransactionContext();
                                TearDownBeforeUnbindFields();
                                UnbindFields();
                            }
                        }
                        catch (Exception e)
                        {
                            ExceptionHandler(e);
                            throw;
                        }
                        finally
                        {
                            TearDownBeforeContainerDestroy();
                        }
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler(e);
                        throw;
                    }
                    finally
                    {
                        _container.Destroy();
                    }
                }
                catch (Exception e)
                {
                    ExceptionHandler(e);
                    throw;
                }
                finally
                {
                    TearDownForEachTestMethod();
                    TearDownMethod();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler(e);
                throw;
            }
            finally
            {
                for (int i = 0; i < 3; ++i)
                {
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                TearDownContainer();
            }
        }

        protected virtual void SetUpContainer()
        {
            SingletonS2ContainerFactory.Init();
            _container = SingletonS2ContainerFactory.Container;
        }

        protected virtual void TearDownContainer()
        {
            SingletonS2ContainerFactory.Destroy();
            _container = null;
        }

        protected virtual void SetUpMethod()
        {
            MethodInfo setupAllMethod = _fixture.GetType().GetMethod("SetUp");
            if (setupAllMethod != null)
            {
                SetUpAttribute a = Attribute.GetCustomAttribute(setupAllMethod, typeof(SetUpAttribute)) as SetUpAttribute;
                if (a == null)
                {
                    MethodUtil.Invoke(setupAllMethod, _fixture, null);
                }
            }
        }

        protected virtual void SetUpForEachTestMethod()
        {
            string targetName = GetTargetName();
            if (targetName.Length > 0)
            {
                MethodInfo setupMethod = _fixture.GetType().GetMethod("SetUp" + targetName);
                if (setupMethod != null)
                {
                    MethodUtil.Invoke(setupMethod, _fixture, null);
                }
            }
        }

        protected virtual void TearDownForEachTestMethod()
        {
            MethodInfo teadDownAllMethod = _fixture.GetType().GetMethod("TearDown");
            if (teadDownAllMethod != null)
            {
                TearDownAttribute a = Attribute.GetCustomAttribute(teadDownAllMethod, typeof(TearDownAttribute)) as TearDownAttribute;
                if (a == null)
                {
                    MethodUtil.Invoke(teadDownAllMethod, _fixture, null);
                }
            }

            string targetName = GetTargetName();
            if (targetName.Length > 0)
            {
                MethodInfo tearDownMethod = _fixture.GetType().GetMethod("TearDown" + targetName);
                if (tearDownMethod != null)
                {
                    MethodUtil.Invoke(tearDownMethod, _fixture, null);
                }
            }
        }

        protected virtual void TearDownMethod()
        {
            MethodInfo teadDownAllMethod = _fixture.GetType().GetMethod("TearDown");
            if (teadDownAllMethod != null)
            {
                TearDownAttribute a = Attribute.GetCustomAttribute(teadDownAllMethod, typeof(TearDownAttribute)) as TearDownAttribute;
                if (a == null)
                {
                    MethodUtil.Invoke(teadDownAllMethod, _fixture, null);
                }
            }
        }

        protected virtual void BeginTransactionContext()
        {

        }

        protected virtual void EndTransactionContext()
        {

        }

        protected virtual void SetUpAfterContainerInit()
        {
        }

        protected virtual void SetUpAfterBindFields()
        {
        }

        protected virtual void TearDownBeforeUnbindFields()
        {
        }

        protected virtual void TearDownBeforeContainerDestroy()
        {
        }

        protected string GetTargetName()
        {
            string name = _method.Name;

            if (name.ToLower().StartsWith("test"))
            {
                name = name.Substring(4);
            }

            if (name.ToLower().EndsWith("test"))
            {
                name = name.Substring(0, name.Length - 4);
            }

            return name;
        }

        protected void BindFields()
        {
            _bindedFields = new ArrayList();
            for (Type type = _fixture.GetType();
                (type != typeof(S2FrameworkTestCaseBase) && type != typeof(S2TestCase) && type != null);
                type = type.BaseType)
            {

                FieldInfo[] fields = type.GetFields(
                            BindingFlags.DeclaredOnly |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                            BindingFlags.Static);

                for (int i = 0; i < fields.Length; ++i)
                {
                    BindField(fields[i]);
                }
            }
        }

        protected void BindField(FieldInfo fieldInfo)
        {
            if (IsAutoBindable(fieldInfo))
            {
                if (fieldInfo.FieldType.ToString() == "System.DateTime")
                {
                    DateTime dateValue = (DateTime) fieldInfo.GetValue(_fixture);
                    if (DateTime.MinValue != dateValue)
                    {
                        return;
                    }
                }
                else if (fieldInfo.GetValue(_fixture) != null)
                {
                    return;
                }
                string name = NormalizeName(fieldInfo.Name);
                object component = null;
                if (_container.HasComponentDef(name))
                {
                    Type componentType = _container.GetComponentDef(name).ComponentType;
                    if (componentType == null)
                    {
                        component = _container.GetComponent(name);
                        if (component != null)
                        {
                            componentType = component.GetType();
                        }
                    }

                    if (componentType != null
                                && fieldInfo.FieldType.IsAssignableFrom(componentType))
                    {
                        if (component == null)
                        {
                            component = _container.GetComponent(name);
                        }
                    }
                    else
                    {
                        component = null;
                    }
                }
                if (component == null
                    && _container.HasComponentDef(fieldInfo.FieldType))
                {
                    component = _container.GetComponent(fieldInfo.FieldType);
                }
                if (component != null)
                {
                    /// TODO 例外ラップとユーティリティにまとめる？
                    fieldInfo.SetValue(_fixture, component);
                    _bindedFields.Add(fieldInfo);
                }
            }
        }

        protected string NormalizeName(string name)
        {
            return name.TrimEnd('_').TrimStart('_');
        }

        protected bool IsAutoBindable(FieldInfo fieldInfo)
        {
            return !fieldInfo.IsStatic && !fieldInfo.IsLiteral
                        && !fieldInfo.IsInitOnly; // && !fieldInfo.FieldType.IsValueType;
        }

        protected void UnbindFields()
        {
            for (int i = 0; i < _bindedFields.Count; ++i)
            {
                FieldInfo fieldInfo = (FieldInfo) _bindedFields[i];
                try
                {
                    if (!fieldInfo.FieldType.IsValueType)
                    {
                        fieldInfo.SetValue(_fixture, null);
                    }
                }
                catch (ArgumentException e)
                {
                    Console.Error.WriteLine(e);
                }
                catch (FieldAccessException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }

        protected void ExceptionHandler(Exception e)
        {
            if (_errors == null)
            {
                _errors = new Hashtable();
            }

            if (!_errors.ContainsKey(e.GetHashCode()))
            {
                object[] attrs = _method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false);
                foreach (ExpectedExceptionAttribute attribute in attrs)
                {
                    if (IsMatchExpectedException(attribute.ExceptionType, e))
                    {
                        return;
                    }
                }

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug(MessageFormatter.GetSimpleMessage("ESSR0017", new object[] { e }), e);
                }
                else
                {
                    Console.Error.WriteLine(e);
                }

                _errors.Add(e.GetHashCode(), e);
            }
        }

        private bool IsMatchExpectedException(Type ExpectedExceptionType, Exception e)
        {
            if (ExpectedExceptionType == e.GetType())
            {
                return true;
            }
            else if (e.InnerException != null)
            {
                return IsMatchExpectedException(ExpectedExceptionType, e.InnerException);
            }
            else
            {
                return false;
            }
        }
    }
}