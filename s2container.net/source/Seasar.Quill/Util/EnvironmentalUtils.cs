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

using System.Diagnostics;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// 実行環境まわりの共通処理クラス
    /// </summary>
    public sealed class EnvironmentalUtils
    {
        /// <summary>
        /// デザイナ上で呼び出されているか判定
        /// VisualStudioデザイナ上で稼動しているプロセス
        /// </summary>
        /// <remarks>
        /// ・VisualStudioデザイナ上：devenv
        /// ・VisualStudioから実行：vshost
        /// ・exeダブルクリック：（exe名）
        /// </remarks>
        /// <returns></returns>
        public static bool IsDesignMode()
        {
            var currentProcess = Process.GetCurrentProcess();
            return currentProcess.ProcessName == "devenv";
        }

    }
}
