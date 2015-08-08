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
using Seasar.Framework.Util;
using ImageConverter=Seasar.Dxo.Converter.Impl.ImageConverter;

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// モデルを相互変換するためのコンバータを生成するためのファクトリクラス
    /// </summary>
    public static class ConverterFactory
    {
        private static readonly IPropertyConverter _assignableConverter = new AssignableConverter();
        private static readonly IPropertyConverter _listConverter = new ListConverter();
        private static readonly IPropertyConverter _dictionaryConverter = new DictionaryConverter();
        private static readonly IPropertyConverter _enumConverter = new EnumConverter();
        private static readonly IPropertyConverter _convertibleConverter = new ConvertibleConverter();
        private static readonly IPropertyConverter _typeConverterConverter = new TypeConverterConverter();
        private static readonly IPropertyConverter _imageConverter = new ImageConverter();
        private static readonly IPropertyConverter _datetimeConverter = new DateTimeConverter();
        private static readonly IPropertyConverter _stringConverter = new StringConverter();
        private static readonly IPropertyConverter _boolConverter = new BooleanConverter();
        private static readonly IPropertyConverter _charConverter = new CharConverter();

        private static readonly IDictionary<Type, IPropertyConverter> _converters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ConverterFactory()
        {
            //コンバータ辞書
            _converters = new Dictionary<Type, IPropertyConverter>();
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
                throw new ArgumentNullException(nameof(destType));

            if ((source != null && source.GetExType() == destType) // 同じ型か
                || (source != null && source.GetExType().IsSubclassOf(destType))) // 派生型か
            {
                return _assignableConverter;
            }
            else if (destType.IsGenericType) //ジェネリックか
            {
                //型パラメタを省いた型を取得
                var openType = destType.GetGenericTypeDefinition();

                //コンバータのオープンな型
                Type converterOpenType = null;

                //変換先ジェネリクスの型パラメタ配列
                var typeParams = destType.GetGenericArguments();

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
                    return _typeConverterConverter;
            }
            else
            {
                if (destType.IsArray) //配列か
                {
                    Type[] typeParams = {destType.GetElementType()};
                    if (source != null && source.GetExType() == typeof (string))
                    {
                        return _charConverter;
                    }
                    else
                    {
                        //ジェネリクスコンバータを生成、又は取得
                        return _GetGenericsConverter(typeof (ArrayConverter<>), typeParams);
                    }
                }
                else if (typeof (IList).IsAssignableFrom(destType)) //リストか
                {
                    return _listConverter;
                }
                else if (typeof (IDictionary).IsAssignableFrom(destType)) //辞書か
                {
                    return _dictionaryConverter;
                }
                else if (destType == typeof (bool) || destType == typeof (Boolean))
                {
                    return _boolConverter;
                }
                else if (destType.IsPrimitive) //プリミティブか
                {
                    return _convertibleConverter;
                }
                else if (destType.IsEnum) //列挙か
                {
                    return _enumConverter;
                }
                else if (typeof (Image).IsAssignableFrom(destType)) //イメージか
                {
                    return _imageConverter;
                }
                else if (destType == typeof (DateTime)) // 日付
                {
                    return _datetimeConverter;
                }
                else if (destType == typeof (string)) // 文字
                {
                    return _stringConverter;
                }
                else //適合しない場合はTypeConvterterを使ってみる
                {
                    return _typeConverterConverter;
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
            var converterType = openType.MakeGenericType(typeParams);
            if (! _converters.ContainsKey(converterType))
            {
                //Genericsコレクション用コンバータのインスタンスを生成
                var propertyConverter = ClassUtil.NewInstance(converterType) as IPropertyConverter;
//                    Activator.CreateInstance(converterType) as IPropertyConverter;
                _converters[converterType] = propertyConverter;
            }
            return _converters[converterType];
        }
    }
}
