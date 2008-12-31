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
using System.Collections.Generic;
using System.Reflection;
using Seasar.Dxo.Annotation;
using Seasar.Dxo.Converter;
using Seasar.Dxo.Exception;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;

namespace Seasar.Dxo.Interceptor
{
    /// <summary>
    /// Data Exchange Objectインターセプター
    /// </summary>
    public class DxoInterceptor : AbstractInterceptor
    {
        private string _dateFormat;
        private IDictionary<string, ConversionRuleAttribute> _dxoMapping;

        /// <summary>
        /// DxoMapping用の取得アノテーションのコレクション
        /// </summary>
        public IDictionary<string, ConversionRuleAttribute> DxoMapping
        {
            get { return _dxoMapping; }
        }

        public event EventHandler<ConvertEventArgs> PrepareConvert;
        public event EventHandler<ConvertEventArgs> ConvertCompleted;
        public event EventHandler<ConvertEventArgs> ConvertFail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public override object Invoke(IMethodInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            MethodBase methodBase = invocation.Method;
            if (methodBase.IsAbstract)
            {
                MethodInfo methodInfo = GetComponentDef(invocation).ComponentType.GetMethod(methodBase.Name);
                Type type = methodInfo.ReturnType;
                if (!type.IsInterface && !type.IsAbstract)
                {
                    object[] args = invocation.Arguments;
                    object dest;
                    if (type != typeof (void))
                    {
                        if (type.IsArray)
                            dest = Array.CreateInstance(type.GetElementType(), 0);
                        else
                            dest = Activator.CreateInstance(type);
                    }
                    else
                    {
                        if (args.Length > 1)
                            dest = args[1];
                        else
                            throw new DxoException();
                    }
                    object source;
                    if (args.Length == 0)
                        throw new DxoException();
                    else
                        source = args[0];

                    _CollectConversionRuleAttribute(methodInfo);
                    _CollectDatePatternMapping(methodInfo);
                    // sourceが配列の場合
                    if (source.GetType().IsArray)
                    {
                        return AssignFromArrayToArray(source, dest);
                    }
                    else if (source.GetType().IsGenericType)
                    {
                        if (source.GetType().IsInterface && dest.GetType().IsInterface)
                        {
                            if (source.GetType().Name == "IList" && dest.GetType().Name == "IList")
                                return AssignFromListToList(source, dest);
                            else
                                return AssignTo(source, dest);
                        }
                        else
                        {
                            Type srcType = source.GetType().GetInterface("IList");
                            Type destType = dest.GetType().GetInterface("IList");
                            if (srcType != null && destType != null && srcType == destType)
                                return AssignFromListToList(source, dest);
                            else
                                return AssignTo(source, dest);
                        }
                    }
                    else if(dest.GetType().GetInterface("IDictionary") == typeof(IDictionary))
                   {
                        IPropertyConverter converter = ConverterFactory.GetConverter(source, dest.GetType());
                        converter.Convert(String.Empty, source, ref dest, dest.GetType());

                        return dest;
                    }
                    else if(!source.GetType().IsGenericType && dest.GetType().GetInterface("IList") == typeof(IList))
                    {
                        return AssignFromObjectToList(source, dest);
                    }
                    else
                    {
                        return AssignTo(source, dest);
                    }
                }
                else
                {
                    return invocation.Proceed();
                }
            }
            else
            {
                return invocation.Proceed();
            }
        }

        /// <summary>
        /// 配列から配列へオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="source">変換元配列</param>
        /// <param name="dest">変換先配列</param>
        protected virtual object AssignFromArrayToArray(object source, object dest)
        {
            object[] sourceObjs = source as object[];

            object[] destObjs = dest as object[];
            if (sourceObjs == null)
                throw new DxoException();
            if (destObjs == null || destObjs.Length == 0)
            {
                Array array = Array.CreateInstance(dest.GetType().GetElementType(), sourceObjs.Length);
                dest = array.Clone();
                destObjs = dest as object[];
            }
            if (destObjs == null || sourceObjs.Length != destObjs.Length)
                throw new DxoException();

            for (int i = 0; i < sourceObjs.Length; i++)
            {
                if (destObjs[i] == null)
                    destObjs[i] = Activator.CreateInstance(dest.GetType().GetElementType(), false);

                AssignTo(sourceObjs[i], destObjs[i]);
            }
            return dest;
        }

        /// <summary>
        /// IListからIListへオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="source">変換元配列</param>
        /// <param name="dest">変換先配列</param>
        protected virtual object AssignFromListToList(object source, object dest)
        {
            IList srcList = source as IList;
            IList destList = dest as IList;
            if (srcList != null && destList != null)
            {
                foreach (object srcObj in srcList)
                {
                    Type[] types = dest.GetType().GetGenericArguments();
                    object destObj = Activator.CreateInstance(types[0], false);
                    AssignTo(srcObj, destObj);
                    destList.Add(destObj);
                }
            }
            return dest;
        }

        /// <summary>
        /// ObjectからIListへオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="source">変換オブジェクト</param>
        /// <param name="dest">変換先配列</param>
        protected virtual object AssignFromObjectToList(object source, object dest)
        {
            IList destList = dest as IList;
            if (destList != null)
            {
                Type[] types = dest.GetType().GetGenericArguments();
                object destObj = Activator.CreateInstance(types[0], false);
                AssignTo(source, destObj);
                destList.Add(destObj);
            }

            return dest;
        }

        /// <summary>
        /// オブジェクトへのアサインを実施します
        /// </summary>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        protected virtual object AssignTo(object source, object dest)
        {
            PropertyInfo[] properties = source.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                _TryExchangeSameNameProperty(property, source, dest, dest.GetType());
            }
            return dest;
        }

        /// <summary>
        /// DxoMapping用属性を全て取得して、内部に保持します
        /// </summary>
        /// <param name="method">実行メソッド情報</param>
        private void _CollectConversionRuleAttribute(MethodInfo method)
        {
            if (_dxoMapping == null)
                _dxoMapping = new Dictionary<string, ConversionRuleAttribute>();

            if (_dxoMapping.Count > 0)
                _dxoMapping.Clear();

            lock (_dxoMapping)
            {
                object[] attrs = method.GetCustomAttributes(typeof (ConversionRuleAttribute), false);
                foreach (ConversionRuleAttribute attr in attrs)
                {
                    if (attr != null && !String.IsNullOrEmpty(attr.PropertyName))
                        _dxoMapping.Add(attr.PropertyName, attr);
                }
            }
        }

        /// <summary>
        /// DatePattern用属性を全て取得して、内部に保持します。
        /// </summary>
        /// <param name="method">実行メソッド情報</param>
        private void _CollectDatePatternMapping(MemberInfo method)
        {
            _dateFormat = String.Empty;
            object[] attrs = method.GetCustomAttributes(typeof (DatePatternAttribute), false);
            foreach (DatePatternAttribute attr in attrs)
            {
                if (attr != null && !String.IsNullOrEmpty(attr.Format))
                    _dateFormat = attr.Format;
            }
        }

        /// <summary>
        /// 任意のプロパティを対象のオブジェクトのプロパティに変換します
        /// </summary>
        /// <param name="sourceInfo">対象となっているプロパティ情報</param>
        /// <param name="source">対象となっているプロパティを持つオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        /// <param name="destType">変換対象のオブジェクトの型</param>
        private void _TryExchangeSameNameProperty(PropertyInfo sourceInfo, object source, object dest, Type destType)
        {
            try
            {
                PropertyInfo destInfo;
                string targetPropertyName = sourceInfo.Name;
                bool existProperty = _dxoMapping.ContainsKey(sourceInfo.Name);
                if (existProperty)
                {
                    string targetName = _dxoMapping[sourceInfo.Name].TargetPropertyName;
                    if (String.IsNullOrEmpty(targetPropertyName))
                        targetPropertyName = sourceInfo.Name;
                    else
                        targetPropertyName = targetName;
                }

                //同じ名前のプロパティが変換先同一プロパティにあるか?
                destInfo = destType.GetProperty(targetPropertyName);
                if (destInfo != null && destInfo.CanRead && destInfo.CanWrite)
                {
                    _ConvertProperty(sourceInfo, source, dest, destInfo, existProperty);
                }
                    // 異なる場合、再帰で調査する
                else
                {
                    object srcValue = sourceInfo.GetValue(source, null);
                    if (srcValue != null)
                    {
                        // 変換元を調査する
                        if (srcValue.GetType().Namespace != "System")
                        {
                            AssignTo(srcValue, dest);
                        }
                            // 変換先を調査する
                        else
                        {
                            PropertyInfo[] properties = destType.GetProperties();
                            foreach (PropertyInfo property in properties)
                            {
                                object destValue = property.GetValue(dest, null);
                                if (destValue != null && destValue.GetType().BaseType != typeof (ValueType) &&
                                    destValue.GetType().Namespace != "System")
                                    AssignTo(source, destValue);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new DxoException(e.Message, e);
            }
        }

        /// <summary>
        /// 同じ名前のプロパティが変換先同一プロパティにあるときにコンバートする
        /// </summary>
        /// <param name="sourceInfo">対象となっているプロパティ情報</param>
        /// <param name="source">対象となっているプロパティを持つオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        /// <param name="destInfo">変換対象のオブジェクト情報</param>
        /// <param name="existProperty">属性の存在フラグ</param>
        private void _ConvertProperty(PropertyInfo sourceInfo, object source, object dest, PropertyInfo destInfo,
                                      bool existProperty)
        {
            object sourceValue = sourceInfo.GetValue(source, null);
            object destValue = destInfo.GetValue(dest, null);

            if (sourceValue != null)
            {
                IPropertyConverter converter;
                if (existProperty)
                {
                    ConversionRuleAttribute attr = _dxoMapping[sourceInfo.Name];
                    if (attr.Ignore)
                    {
                        converter = null;
                    }
                    else
                    {
                        if (attr.PropertyConverter != null)
                            converter = Activator.CreateInstance(attr.PropertyConverter) as IPropertyConverter;
                        else
                            converter = ConverterFactory.GetConverter(sourceValue, destInfo.PropertyType);
                    }
                }
                else
                {
                    //コンバータをファクトリから取得
                    converter = ConverterFactory.GetConverter(sourceValue, destInfo.PropertyType);
                }
                if (converter != null)
                {
                    if (!String.IsNullOrEmpty(_dateFormat))
                        converter.Format = _dateFormat;

                    _AttachConverterEvent(converter);
                    try
                    {
                        //コンバータによる変換
                        converter.Convert(sourceInfo.Name, sourceValue, ref destValue, destInfo.PropertyType);
                        destInfo.SetValue(dest, destValue, null);
                    }
                    finally
                    {
                        _DetachConverterEvent(converter);
                    }
                }
            }
        }

        /// <summary>
        /// コンバータのイベントをアタッチします
        /// </summary>
        /// <param name="converter">現在のコンバータ</param>
        private void _AttachConverterEvent(IPropertyConverter converter)
        {
            converter.PrepareConvert += this.PrepareConvert;
            converter.ConvertCompleted += this.ConvertCompleted;
            converter.ConvertFail += this.ConvertFail;
        }

        /// <summary>
        /// コンバータのイベントをデタッチします
        /// </summary>
        /// <param name="converter">現在のコンバータをセット</param>
        private void _DetachConverterEvent(IPropertyConverter converter)
        {
            converter.PrepareConvert -= this.PrepareConvert;
            converter.ConvertCompleted -= this.ConvertCompleted;
            converter.ConvertFail -= this.ConvertFail;
        }
    }
}