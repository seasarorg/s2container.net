#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Windows.Forms;
using Seasar.Windows.Attr;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 画面遷移定義FormDispatcherインターフェイス
    /// </summary>
    /// <remarks>
    /// モーダレスで表示するフォームは、diconファイルでinstance=prototypeにする
    /// <newpara>
    /// フォームに値を渡すときは表示メソッドに引数を用意し、同名のプロパティを対象のフォームに用意する。
    /// </newpara>
    /// <newpara>
    /// メソッド(または対象フォーム)と名前空間を変更し、使いまわしてもよい。
    /// </newpara>
    /// </remarks>
    public interface IFormDispatcher
    {
        /// <summary>
        /// 社員一覧フォームを表示する
        /// </summary>
        /// <returns>ダイアログ結果</returns>
        [TargetForm(typeof (FrmEmployeeList), ModalType.Modal)]
        DialogResult ShowDataList();

        /// <summary>
        /// 社員編集フォームを表示する
        /// </summary>
        /// <param name="Id">社員ID</param>
        /// <returns>ダイアログ結果</returns>
        [TargetForm(typeof (FrmEmployeeEdit), ModalType.Modal)]
        DialogResult ShowDataEdit(Nullable<int> Id);

        /// <summary>
        /// 部門一覧フォームを表示する
        /// </summary>
        /// <returns>ダイアログ結果</returns>
        [TargetForm(typeof (FrmDepartmentList), ModalType.Modal)]
        DialogResult ShowMasterList();

        /// <summary>
        /// 部門編集フォームを表示する
        /// </summary>
        /// <param name="Id">部門ID</param>
        /// <returns>ダイアログ結果</returns>
        [TargetForm(typeof (FrmDepartmentEdit), ModalType.Modal)]
        DialogResult ShowMasterEdit(Nullable<int> Id);
    }
}