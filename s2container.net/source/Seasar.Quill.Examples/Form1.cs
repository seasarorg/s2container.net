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
using System.Windows.Forms;
using Seasar.Quill.Examples.Logic;
using Seasar.Quill.Examples.Entity;

namespace Seasar.Quill.Examples
{
    public partial class Form1 : Form
    {
        protected EmployeeLogic employeeLogic;

        public Form1()
        {
            InitializeComponent();
        }

        private void EmpButton_Click(object sender, EventArgs e)
        {
            // 検索結果を表示するTextBoxを初期化する
            enameTextBox.Text = string.Empty;

            // 検索の条件となる社員コード
            int empNo;

            if (!int.TryParse(empNoTextBox.Text, out empNo))
            {
                // 社員コードが数字でない場合はエラーメッセージを表示する
                MessageBox.Show("社員コードは数字で入力してください");
                return;
            }

            // 社員コードから社員を検索する
            Employee emp = employeeLogic.GetEmployeeByEmpNo(empNo);

            if (emp == null)
            {
                // 社員が存在しない場合はエラーメッセージを表示する
                MessageBox.Show("該当する社員は存在しませんでした");
            }
            else
            {
                // 社員が存在した場合は社員名を表示する
                enameTextBox.Text = emp.Ename;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初期値をセットする
            empNoTextBox.Text = "7499";
        }
    }
}