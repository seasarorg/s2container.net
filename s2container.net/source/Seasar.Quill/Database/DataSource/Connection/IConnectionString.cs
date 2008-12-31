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

namespace Seasar.Quill.Database.DataSource.Connection
{
    /// <summary>
    /// 接続文字列取得インターフェース
    /// </summary>
    /// <remarks>
    /// 外部ファイルからの読み込み、暗号・復号など
    /// 接続文字列の取得に何らかのロジックを使用する場合は
    /// このインターフェースを実装してapp.configの
    /// quillセクション内に名前空間付でクラス名を記述して下さい。
    /// </remarks>
    public interface IConnectionString
    {
        /// <summary>
        /// 接続文字列の取得
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();
    }
}
