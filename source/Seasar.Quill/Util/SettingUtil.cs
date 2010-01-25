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
using Seasar.Quill.Dao;
using Seasar.Quill.Dao.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Quill.Exception;
using System.Reflection;
using System.Text;
using System.IO;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Transaction/Dao設定クラスの決定を行うユーティリティ
    /// </summary>
    public class SettingUtil
    {
        //private const string DEFALT_DATASOURCE_NAME = "DataSource";

        ///// <summary>
        ///// 文字列("で囲まれているか)判定するための正規表現
        ///// </summary>
        //private static readonly Regex _regexIsString = new Regex("^\".*\"$");

        ///// <summary>
        ///// Dao設定クラスの型を取得する
        ///// </summary>
        ///// <returns></returns>
        //public static Type GetDaoSettingType()
        //{
        //    Type retType;
        //    QuillSection section = QuillSectionHandler.GetQuillSection();
        //    if (section == null || string.IsNullOrEmpty(section.DaoSetting))
        //    {
        //        //  設定がない場合は既定のDao設定を使う
        //        retType = GetDefaultDaoSettingType();
        //    }
        //    else
        //    {
        //        string typeName = section.DaoSetting;
        //        if (TypeUtil.HasNamespace(typeName) == false)
        //        {
        //            //  名前空間の指定がなければ既定の名前空間を使う
        //            typeName = string.Format("{0}.{1}",
        //                QuillConstants.NAMESPACE_DAOSETTING, typeName);
        //        }
        //        retType = ClassUtil.ForName(typeName);
        //    }
        //    return retType;
        //}

        ///// <summary>
        ///// Transaction設定クラスの型を取得する
        ///// </summary>
        ///// <returns></returns>
        //public static Type GetTransationSettingType()
        //{
        //    Type retType;
        //    QuillSection section = QuillSectionHandler.GetQuillSection();
        //    if (section == null || string.IsNullOrEmpty(section.TransactionSetting))
        //    {
        //        //  属性引数による指定もapp.configにも設定がなければ
        //        //  デフォルトのトランザクション設定を使う
        //        retType = GetDefaultTransactionType();
        //    }
        //    else
        //    {
        //        //  トランザクション設定クラスが設定ファイルで指定されていればそちらを使う
        //        string typeName = section.TransactionSetting;
        //        if (TypeUtil.HasNamespace(typeName) == false)
        //        {
        //            //  名前空間なしの場合は既定の名前空間から
        //            typeName = string.Format("{0}.{1}",
        //                QuillConstants.NAMESPACE_TXSETTING, typeName);
        //        }
        //        retType = ClassUtil.ForName(typeName);
        //        if (retType == null)
        //        {
        //            throw new ClassNotFoundRuntimeException(typeName);
        //        }

        //        ValidateTransactionSettingType(retType);
        //    }
        //    return retType;
        //}

        /// <summary>
        /// 実行中のSeasar.Quill.dllが置いてあるディレクトリのパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyDirectoryPath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            StringBuilder builder = new StringBuilder();
            builder.Append(assembly.CodeBase);
            //  アドレス表記を削除
            builder.Replace("file:///", string.Empty);
            builder.Replace(assembly.GetName().Name + ".DLL", string.Empty);
            //  ディレクトリパスのみ取り出す
            string retPath = Path.GetDirectoryName(builder.ToString());
            return retPath + Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// 出力ディレクトリを含むQuill設定ファイルのパスを返す
        /// </summary>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public static string GetQuillConfigPath(string configFileName)
        {
            StringBuilder builder = new StringBuilder(GetAssemblyDirectoryPath());
            builder.Append(configFileName);
            return builder.ToString();
        }

        /// <summary>
        /// 既定のQuill設定ファイルパスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultQuillConfigPath()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetAssemblyDirectoryPath());
            builder.Append(Assembly.GetExecutingAssembly().GetName().Name);
            builder.Append(".dll.config");
            return builder.ToString();
        }

        /// <summary>
        /// Dao設定クラスかどうか検証
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="QuillApplicationException">IDaoSetting実装クラスでないとき</exception>
        public static void ValidateDaoSettingType(Type type)
        {
            if (!typeof(IDaoSetting).IsAssignableFrom(type))
            {
                throw new QuillApplicationException("EQLL0025", type.Name);
            }
        }

        /// <summary>
        /// Transaction設定クラスかどうか検証
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="QuillApplicationException">ITransactionSetting実装クラスでないとき</exception>
        public static void ValidateTransactionSettingType(Type type)
        {
            if (!typeof(ITransactionSetting).IsAssignableFrom(type))
            {
                throw new QuillApplicationException("EQLL0026", type.Name);
            }
        }

        /// <summary>
        /// 標準で使うトランザクション設定クラスの型を返す
        /// </summary>
        /// <returns></returns>
        public static Type GetDefaultTransactionType()
        {
            return typeof(TypicalTransactionSetting);
        }

        /// <summary>
        /// 標準で使うDao設定クラスの型を返す
        /// </summary>
        /// <returns></returns>
        public static Type GetDefaultDaoSettingType()
        {
            return typeof(TypicalDaoSetting);
        }
    }
}
