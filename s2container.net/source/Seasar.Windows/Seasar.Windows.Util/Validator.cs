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
using System.Text;

namespace Seasar.Windows.Utils
{
    /// <summary>
    /// 入力チェック用ユーティリティクラス
    /// </summary>
    public sealed class Validator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Validator()
        {
            ;
        }

        /// <summary>
        /// MS932での文字列の長さを取得する
        /// </summary>
        /// <param name="src">長さを計算する文字列</param>
        /// <returns>長さ</returns>
        public static int GetLengthBySJIS(string src)
        {
            // Shift_JIS = 932
            Encoding enc = Encoding.GetEncoding(932);

            return (enc.GetByteCount(src));
        }

        /// <summary>
        /// 文字列の長さを取得する
        /// </summary>
        /// <param name="src">長さを計算する文字列</param>
        /// <param name="encoding">文字コード</param>
        /// <returns>長さ</returns>
        public static int GetLength(string src, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return (encoding.GetByteCount(src));
        }

        /// <summary>
        /// 文字長さの範囲に収まっているかをチェックする
        /// </summary>
        /// <param name="src">チェック対象文字列</param>
        /// <param name="lower">最低の長さ</param>
        /// <param name="upper">最長の長さ</param>
        /// <returns></returns>
        public static bool IsInRangeBySJIS(string src, int lower, int upper)
        {
            int length = GetLengthBySJIS(src);
            if (length < lower || upper < length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 文字長さの範囲に収まっているかをチェックする
        /// </summary>
        /// <param name="src">チェック対象文字列</param>
        /// <param name="lower">最低の長さ</param>
        /// <param name="upper">最長の長さ</param>
        /// <returns></returns>
        /// <param name="encoding">文字エンコード</param>
        public static bool IsInRange(string src, int lower, int upper, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            int length = GetLength(src, encoding);
            if (length < lower || upper < length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 値が範囲内に収まっているかをチェックする
        /// </summary>
        /// <param name="value">チェック対象数値</param>
        /// <param name="lower">最小の値</param>
        /// <param name="upper">最大の値</param>
        /// <returns></returns>
        public static bool IsInRange(long value, long lower, long upper)
        {
            if (value < lower || upper < value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 値が範囲内に収まっているかをチェックする
        /// </summary>
        /// <param name="value">チェック対象数値</param>
        /// <param name="lower">最小の値</param>
        /// <param name="upper">最大の値</param>
        /// <returns></returns>
        public static bool IsInRange(int value, int lower, int upper)
        {
            if (value < lower || upper < value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 値が範囲内に収まっているかをチェックする
        /// </summary>
        /// <param name="value">チェック対象数値</param>
        /// <param name="lower">最小の値</param>
        /// <param name="upper">最大の値</param>
        /// <returns></returns>
        public static bool IsInRange(double value, double lower, double upper)
        {
            if (value < lower || upper < value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 日付を比較する
        /// </summary>
        /// <param name="value">チェック対象日付</param>
        /// <param name="compare">比較日付</param>
        /// <returns>同じ日なら0、最初日付が比較日付より以前ならマイナス、逆なら正</returns>
        public static int CompareDays(DateTime value, DateTime compare)
        {
            DateTime firstDate = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
            DateTime secondDate = new DateTime(compare.Year, compare.Month, compare.Day, 0, 0, 0);
            if (firstDate.Ticks == secondDate.Ticks)
            {
                return 0;
            }
            else
            {
                return (secondDate.Subtract(firstDate).Days);
            }
        }

        /// <summary>
        /// 月を比較する
        /// </summary>
        /// <param name="value">チェック対象日付</param>
        /// <param name="compare">比較日付</param>
        /// <returns>同じ日なら0、最初日付が比較日付より以前ならマイナス、逆なら正</returns>
        public static int CompareMonth(DateTime value, DateTime compare)
        {
            DateTime firstDate = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
            DateTime secondDate = new DateTime(compare.Year, compare.Month, compare.Day, 0, 0, 0);
            if (firstDate.Year == secondDate.Year && firstDate.Month == secondDate.Month)
            {
                return 0;
            }
            else
            {
                return ((secondDate.Year - firstDate.Year) * 12 + (secondDate.Month - firstDate.Month));
            }
        }

        /// <summary>
        /// 年を比較する
        /// </summary>
        /// <param name="value">チェック対象日付</param>
        /// <param name="compare">比較日付</param>
        /// <returns>同じ日なら0、最初日付が比較日付より以前ならマイナス、逆なら正</returns>
        public static int CompareYear(DateTime value, DateTime compare)
        {
            DateTime firstDate = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
            DateTime secondDate = new DateTime(compare.Year, compare.Month, compare.Day, 0, 0, 0);

            return (secondDate.Year - firstDate.Year);
        }

        /// <summary>
        /// 正確に日付を比較する
        /// </summary>
        /// <param name="value">チェック対象日付</param>
        /// <param name="compare">比較日付</param>
        /// <returns>同じミリ秒なら0、最初日付が比較日付より以前ならマイナス、逆なら正</returns>
        public static long CompareStrict(DateTime value, DateTime compare)
        {
            return (value.Ticks - compare.Ticks);
        }
    }
}
