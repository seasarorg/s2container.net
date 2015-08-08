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
using System.Collections.Generic;
using System.Reflection;
using Seasar.Dxo.Annotation;
using Seasar.Dxo.Converter;
using Seasar.Dxo.Exception;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Util;

namespace Seasar.Dxo.Interceptor
{
    /// <summary>
    /// Data Exchange Objectインターセプター
    /// </summary>
    public class DxoInterceptor : AbstractInterceptor
    {
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
                throw new ArgumentNullException(nameof(invocation));

            var methodBase = invocation.Method;
            if (methodBase.IsAbstract)
            {
                var dxoMapping = CreateDxoMapping();
                
                var methodInfo = GetComponentDef(invocation).ComponentType.GetMethod(methodBase.Name);
                var returnType = methodInfo.ReturnType;
                if (!returnType.IsInterface && !returnType.IsAbstract)
                {
                    var args = invocation.Arguments;
                    object dest;
                    if (returnType != typeof (void))
                    {
                        if (returnType.IsArray)
                            dest = ArrayUtil.NewInstance(returnType.GetElementType(), 0);
//                            dest = Array.CreateInstance(returnType.GetElementType(), 0);
                        else
                            dest = ClassUtil.NewInstance(returnType);
//                            dest = Activator.CreateInstance(returnType);
                    }
                    else
                    {
                        if (args.Length > 1)
                            dest = args[1];
                        else
                            throw new DxoException();
                    }
                    if (args.Length == 0)
                        throw new DxoException();
                    var source = args[0];

                    CollectConversionRuleAttribute(dxoMapping, methodInfo);
                    var dateFormat = GetDatePatternFormat(methodInfo);
                    // sourceが配列の場合
                    if (source.GetExType().IsArray)
                    {
                        return AssignFromArrayToArray(dxoMapping, source, dest, dateFormat);
                    }
                    else if (source.GetExType().IsGenericType)
                    {
                        if (source.GetExType().IsInterface && dest.GetExType().IsInterface)
                        {
                            if (source.GetExType().Name == "IList" && dest.GetExType().Name == "IList")
                                return AssignFromListToList(dxoMapping, source, dest, dateFormat);
                            else
                                return AssignTo(dxoMapping, source, dest, 0, dateFormat);
                        }
                        else
                        {
                            var srcType = source.GetExType().GetInterface("IList");
                            var destType = dest.GetExType().GetInterface("IList");
                            if (srcType != null && destType != null && srcType == destType)
                                return AssignFromListToList(dxoMapping, source, dest, dateFormat);
                            else
                                return AssignTo(dxoMapping, source, dest, 0, dateFormat);
                        }
                    }
                    else if(dest.GetExType().GetInterface("IDictionary") == typeof(IDictionary))
                   {
                        var converter = ConverterFactory.GetConverter(source, dest.GetExType());
                        converter.Convert(String.Empty, source, ref dest, dest.GetExType());

                        return dest;
                    }
                    else if(!source.GetExType().IsGenericType && dest.GetExType().GetInterface("IList") == typeof(IList))
                    {
                        return AssignFromObjectToList(dxoMapping, source, dest, dateFormat);
                    }
                    else
                    {
                        return AssignTo(dxoMapping, source, dest, 0, dateFormat);
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
        /// 変換ルール情報の格納場所を生成
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, ConversionRuleAttribute> CreateDxoMapping()
        {
            return new Dictionary<string, ConversionRuleAttribute>();
        }

        /// <summary>
        /// 配列から配列へオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="source">変換元配列</param>
        /// <param name="dest">変換先配列</param>
        /// <param name="dateFormat">日付書式</param>
        protected virtual object AssignFromArrayToArray(IDictionary<string, ConversionRuleAttribute> dxoMapping, 
            object source, object dest, string dateFormat)
        {
            var sourceObjs = source as object[];

            var destObjs = dest as object[];
            if (sourceObjs == null)
                throw new DxoException();
            if (destObjs == null || destObjs.Length == 0)
            {
                var array = ArrayUtil.NewInstance(dest.GetExType().GetElementType(), sourceObjs.Length);
//                var array = Array.CreateInstance(dest.GetExType().GetElementType(), sourceObjs.Length);
                dest = array.Clone();
                destObjs = dest as object[];
            }
            if (destObjs == null || sourceObjs.Length != destObjs.Length)
                throw new DxoException();

            for (var i = 0; i < sourceObjs.Length; i++)
            {
                if (destObjs[i] == null)
                    destObjs[i] = ClassUtil.NewInstance(dest.GetExType().GetElementType());
//                    destObjs[i] = Activator.CreateInstance(dest.GetExType().GetElementType(), false);

                AssignTo(dxoMapping, sourceObjs[i], destObjs[i], 0, dateFormat);
            }
            return dest;
        }

        /// <summary>
        /// IListからIListへオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="source">変換元配列</param>
        /// <param name="dest">変換先配列</param>
        /// <param name="dateFormat">日付書式</param>
        protected virtual object AssignFromListToList(IDictionary<string, ConversionRuleAttribute> dxoMapping, 
            object source, object dest, string dateFormat)
        {
            var srcList = source as IList;
            var destList = dest as IList;
            if (srcList != null && destList != null)
            {
                foreach (var srcObj in srcList)
                {
                    var types = dest.GetExType().GetGenericArguments();
                    var destObj = ClassUtil.NewInstance(types[0]);
//                    var destObj = Activator.CreateInstance(types[0], false);
                    AssignTo(dxoMapping, srcObj, destObj, 0, dateFormat);
                    destList.Add(destObj);
                }
            }
            return dest;
        }

        /// <summary>
        /// ObjectからIListへオブジェクトへのアサインを実施する
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="source">変換オブジェクト</param>
        /// <param name="dest">変換先配列</param>
        /// <param name="dateFormat">日付書式</param>
        protected virtual object AssignFromObjectToList(IDictionary<string, ConversionRuleAttribute> dxoMapping, 
            object source, object dest, string dateFormat)
        {
            var destList = dest as IList;
            if (destList != null)
            {
                var types = dest.GetExType().GetGenericArguments();
                var destObj = ClassUtil.NewInstance(types[0]);
//                var destObj = Activator.CreateInstance(types[0], false);
                AssignTo(dxoMapping, source, destObj, 0, dateFormat);
                destList.Add(destObj);
            }

            return dest;
        }

        /// <summary>
        /// オブジェクトへのアサインを実施します
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        /// <param name="cnt">ネストカウンター</param>
        /// <param name="dateFormat">日付書式</param>
        protected virtual object AssignTo(IDictionary<string, ConversionRuleAttribute> dxoMapping, object source, object dest, int cnt, string dateFormat)
        {
            if (cnt < 2)
            {
                var properties = source.GetExType().GetProperties();
                foreach (var property in properties)
                {
                    _TryExchangeSameNameProperty(dxoMapping, property, source, dest, dest.GetExType(), cnt, dateFormat);
                }
            }
            return dest;
        }

        /// <summary>
        /// DxoMapping用属性を全て取得して、内部に保持します
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="method">実行メソッド情報</param>
        protected virtual void CollectConversionRuleAttribute(IDictionary<string, ConversionRuleAttribute> dxoMapping, MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(ConversionRuleAttribute), false);
            foreach (ConversionRuleAttribute attr in attrs)
            {
                if (!String.IsNullOrEmpty(attr?.PropertyName))
                {
                    dxoMapping.Add(attr.PropertyName, attr);
                }
            }
        }

        /// <summary>
        /// DatePattern用属性を全て取得して、内部に保持します。
        /// </summary>
        /// <param name="method">実行メソッド情報</param>
        protected virtual string GetDatePatternFormat(MemberInfo method)
        {
            var dateFormat = String.Empty;
            var attrs = method.GetCustomAttributes(typeof (DatePatternAttribute), false);
            foreach (DatePatternAttribute attr in attrs)
            {
                if (!String.IsNullOrEmpty(attr?.Format))
                    dateFormat = attr.Format;
            }
            return dateFormat;
        }

        /// <summary>
        /// 任意のプロパティを対象のオブジェクトのプロパティに変換します
        /// </summary>
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="sourceInfo">対象となっているプロパティ情報</param>
        /// <param name="source">対象となっているプロパティを持つオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        /// <param name="destType">変換対象のオブジェクトの型</param>
        /// <param name="cnt">ネストカウンター</param>
        /// <param name="dateFormat">日付書式</param>
        private void _TryExchangeSameNameProperty(IDictionary<string, ConversionRuleAttribute> dxoMapping,
            PropertyInfo sourceInfo, object source, object dest, Type destType, int cnt, string dateFormat)
        {
            try
            {
                cnt++;
                var targetPropertyName = sourceInfo.Name;
                var existProperty = dxoMapping.ContainsKey(sourceInfo.Name);
                if (existProperty)
                {
                    var targetName = dxoMapping[sourceInfo.Name].TargetPropertyName;
                    if (String.IsNullOrEmpty(targetPropertyName))
                    {
                        targetPropertyName = sourceInfo.Name;
                    }
                    else
                    {
                        targetPropertyName = targetName;
                    }
                }

                //同じ名前のプロパティが変換先同一プロパティにあるか?
                var destInfo = destType.GetProperty(targetPropertyName);
                if (destInfo != null && destInfo.CanRead && destInfo.CanWrite)
                {
                    _ConvertProperty(dxoMapping, sourceInfo, source, dest, destInfo, existProperty, dateFormat);
                }
                    // 異なる場合、再帰で調査する
                else
                {
                    var srcValue = PropertyUtil.GetValue(source, source.GetExType(), sourceInfo.Name);
//                    var srcValue = sourceInfo.GetValue(source, null);
                    if (srcValue != null)
                    {
                        // 変換元を調査する
                        if (srcValue.GetExType().Namespace != "System")
                        {
                            AssignTo(dxoMapping, srcValue, dest, cnt, dateFormat);
                        }
                            // 変換先を調査する
                        else
                        {
                            var properties = destType.GetProperties();
                            foreach (var property in properties)
                            {

                                var destValue = PropertyUtil.GetValue(dest, dest.GetExType(), property.Name);
//                                var destValue = property.GetValue(dest, null);
                                if (destValue != null && destValue.GetExType().BaseType != typeof(ValueType) &&
                                    destValue.GetExType().Namespace != "System")
                                {
                                    AssignTo(dxoMapping, source, destValue, cnt, dateFormat);
                                }
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
        /// <param name="dxoMapping">Dxo変換情報</param>
        /// <param name="sourceInfo">対象となっているプロパティ情報</param>
        /// <param name="source">対象となっているプロパティを持つオブジェクト</param>
        /// <param name="dest">変換対象のオブジェクト</param>
        /// <param name="destInfo">変換対象のオブジェクト情報</param>
        /// <param name="existProperty">属性の存在フラグ</param>
        /// <param name="dateFormat">日付書式</param>
        private void _ConvertProperty(IDictionary<string, ConversionRuleAttribute> dxoMapping, PropertyInfo sourceInfo, 
            object source, object dest, PropertyInfo destInfo, bool existProperty, string dateFormat)
        {
//            var sourceValue = sourceInfo.GetValue(source, null);
//            var destValue = destInfo.GetValue(dest, null);
            var sourceValue = PropertyUtil.GetValue(source, source.GetExType(), sourceInfo.Name);
            var destValue = PropertyUtil.GetValue(dest, dest.GetExType(), destInfo.Name);

            if (sourceValue != null)
            {
                IPropertyConverter converter;
                if (existProperty)
                {
                    var attr = dxoMapping[sourceInfo.Name];
                    if (attr.Ignore)
                    {
                        converter = null;
                    }
                    else
                    {
                        if (attr.PropertyConverter != null)
                            converter = ClassUtil.NewInstance(attr.PropertyConverter) as IPropertyConverter;
//                            converter = Activator.CreateInstance(attr.PropertyConverter) as IPropertyConverter;
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
                    converter.Format = (dateFormat ?? String.Empty);
                    _AttachConverterEvent(converter);
                    try
                    {
                        //コンバータによる変換
                        converter.Convert(sourceInfo.Name, sourceValue, ref destValue, destInfo.PropertyType);
                        PropertyUtil.SetValue(dest, dest.GetExType(), destInfo.Name, destInfo.PropertyType, destValue);
//                        destInfo.SetValue(dest, destValue, null);
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
            converter.PrepareConvert += PrepareConvert;
            converter.ConvertCompleted += ConvertCompleted;
            converter.ConvertFail += ConvertFail;
        }

        /// <summary>
        /// コンバータのイベントをデタッチします
        /// </summary>
        /// <param name="converter">現在のコンバータをセット</param>
        private void _DetachConverterEvent(IPropertyConverter converter)
        {
            converter.PrepareConvert -= PrepareConvert;
            converter.ConvertCompleted -= ConvertCompleted;
            converter.ConvertFail -= ConvertFail;
        }
    }
}
