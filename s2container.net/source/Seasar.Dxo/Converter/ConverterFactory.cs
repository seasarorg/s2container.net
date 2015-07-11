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
using System.Diagnostics;
using System.Drawing;
using Seasar.Dxo.Converter.Impl;
using ImageConverter=Seasar.Dxo.Converter.Impl.ImageConverter;

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// モデルを相互変換するためのコンバータを生成するためのファクトリクラス
    /// </summary>
    public static class ConverterFactory
    {
        private static readonly IPropertyConverter assignableConverter = new AssignableConverter();
        private static readonly IPropertyConverter listConverter = new ListConverter();
        private static readonly IPropertyConverter dictionaryConverter = new DictionaryConverter();
        private static readonly IPropertyConverter enumConverter = new EnumConverter();
        private static readonly IPropertyConverter convertibleConverter = new ConvertibleConverter();
        private static readonly IPropertyConverter typeConverterConverter = new TypeConverterConverter();
        private static readonly IPropertyConverter imageConverter = new ImageConverter();
        private static readonly IPropertyConverter datetimeConverter = new DateTimeConverter();
        private static readonly IPropertyConverter stringConverter = new StringConverter();
        private static readonly IPropertyConverter boolConverter = new BooleanConverter();
        private static readonly IPropertyConverter charConverter = new CharConverter();

        private static readonly IDictionary<Type, IPropertyConverter> converters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ConverterFactory()
        {
            //コンバータ辞書
            converters = new Dictionary<Type, IPropertyConverter>();
        }

        /// <summary>
        /// 適切なコンバータを取得します
        /// </summary>
        /// <param name="source">コンバータを必要としている、元オブジェクトをセットします</param>
        /// <param name="destType">元オブジェクトを変換する型をセットします</param>
        /// <returns>IPropertyConverter 適切なコンバータが戻ります</returns>
        public static IPropertyConverter GetConverter(object source, Type destType)
        {
            Debug.Assert(destType != null, String.Format(DxoMessages.EDXO1001, "destType"));
//            Debug.Assert(destType != null, "destTypeは非nullのはず");
            if (destType == null)
                throw new ArgumentNullException("destType");

            if ((source != null && source.GetType().Equals(destType)) // 同じ型か
                || (source != null && source.GetType().IsSubclassOf(destType))) // 派生型か
            {
                return assignableConverter;
            }
            else if (destType.IsGenericType) //ジェネリックか
            {
                //型パラメタを省いた型を取得
                Type openType = destType.GetGenericTypeDefinition();

                //コンバータのオープンな型
                Type converterOpenType = null;

                //変換先ジェネリクスの型パラメタ配列
                Type[] typeParams = destType.GetGenericArguments();

                //ICollection<>か
                if (typeof (ICollection<>).IsAssignableFrom(openType))
                {
                    converterOpenType = typeof (GenericsCollectionConverter<>);
                }
                    //IList<>か
                else if (typeof (IList<>).IsAssignableFrom(openType))
                {
                    converterOpenType = typeof (GenericsListConverter<>);
                }
                    //IDictionary<>か
                else if (typeof (IDictionary<,>).IsAssignableFrom(openType))
                {
                    converterOpenType = typeof (GenericsDictionaryConverter<,>);
                }

                //ジェネリクスコンバータを生成、又は取得
                if (converterOpenType != null)
                    return (_GetGenericsConverter(converterOpenType, typeParams));
                else
                    return typeConverterConverter;
            }
            else
            {
                if (destType.IsArray) //配列か
                {
                    Type[] typeParams = new Type[] {destType.GetElementType()};
                    if (source != null && source.GetType() == typeof (string))
                    {
                        return charConverter;
                    }
                    else
                    {
                        //ジェネリクスコンバータを生成、又は取得
                        return _GetGenericsConverter(typeof (ArrayConverter<>), typeParams);
                    }
                }
                else if (typeof (IList).IsAssignableFrom(destType)) //リストか
                {
                    return listConverter;
                }
                else if (typeof (IDictionary).IsAssignableFrom(destType)) //辞書か
                {
                    return dictionaryConverter;
                }
                else if (destType == typeof (bool) || destType == typeof (Boolean))
                {
                    return boolConverter;
                }
                else if (destType.IsPrimitive) //プリミティブか
                {
                    return convertibleConverter;
                }
                else if (destType.IsEnum) //列挙か
                {
                    return enumConverter;
                }
                else if (typeof (Image).IsAssignableFrom(destType)) //イメージか
                {
                    return imageConverter;
                }
                else if (destType == typeof (DateTime)) // 日付
                {
                    return datetimeConverter;
                }
                else if (destType == typeof (string)) // 文字
                {
                    return stringConverter;
                }
                else //適合しない場合はTypeConvterterを使ってみる
                {
                    return typeConverterConverter;
                }
            }
        }

        /// <summary>
        /// Genrricsを使用した型コンバータを取得します
        /// </summary>
        /// <param name="openType">Genericsのオープンタイプをセットします</param>
        /// <param name="typeParams">Genericsの型パラメタをセットします</param>
        /// <returns>IPropertyConverter 生成されるか、又は辞書から取得したコンバータが戻ります</returns>
        private static IPropertyConverter _GetGenericsConverter(Type openType, Type[] typeParams)
        {
            Type converterType = openType.MakeGenericType(typeParams);
            if (! converters.ContainsKey(converterType))
            {
                //Genericsコレクション用コンバータのインスタンスを生成
                IPropertyConverter propertyConverter =
                    Activator.CreateInstance(converterType) as IPropertyConverter;
                converters[converterType] = propertyConverter;
            }
            return converters[converterType];
        }
    }
}
