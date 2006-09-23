using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using log4net;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 社員登録画面
    /// </summary>
    public partial class FrmEmployeeEdit : Form
    {
        /// <summary>
        /// 社員ID
        /// </summary>
        private Nullable<int> _id;

        /// <summary>
        /// 画面登録用サービス
        /// </summary>
        private IEmployeeEditService _service;

        /// <summary>
        /// ログ(log4net)
        /// </summary>
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 例外エラーメッセージ書式
        /// </summary>
        private const string EXCEPTION_MSG_FORMAT = "予期できないエラーが発生しました。詳細を確認してください。（{0}）";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FrmEmployeeEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 社員ID
        /// </summary>
        public Nullable<int> Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 画面登録用サービス
        /// </summary>
        public IEmployeeEditService Service
        {
            get { return _service; }
            set { _service = value; }
        }

        /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmployeeEdit_Load(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}がロードされました", Name);

            _InitializeControls();
            if ( _id.HasValue )
            {
                EmployeeDto data = _service.GetData(_id.Value);
                if ( data != null )
                {
                    _ShowData(data);
                    btnDelete.Enabled = true;
                }
                else
                {
                    throw new ApplicationException("社員データが見つかりませんでした");
                }
            }
        }

        /// <summary>
        /// 登録ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ( MessageBox.Show("本当に登録しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.No )
                    return;

                EmployeeDto data = new EmployeeDto();
                if ( !_SetInputData(data) ) return;

                if ( _service.ExecUpdate(data) > 0 )
                {
                    _InitializeControls();
                    MessageBox.Show("登録しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    throw new ApplicationException("登録に失敗しました");
            }
            catch ( Exception ex )
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 削除ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if ( MessageBox.Show("本当に削除しますか？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                     DialogResult.No )
                    return;

                if ( _id.HasValue )
                {
                    if ( _service.ExecDelete(_id.Value) > 0 )
                    {
                        MessageBox.Show("削除しました", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    else
                    {
                        throw new ApplicationException("削除に失敗しました");
                    }
                }
                else
                {
                    MessageBox.Show("削除対象を選んでください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch ( Exception ex )
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(String.Format(EXCEPTION_MSG_FORMAT, ex.Message), Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 閉じたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}を終了", Name);
            Close();
        }

        /// <summary>
        /// コントロールを初期化する
        /// </summary>
        private void _InitializeControls()
        {
            txtCode.Text = "";
            txtName.Text = "";
            dtpEntry.Value = DateTime.Today;

            _InitializeGenderBox();
            _InializeDepartmentBox();

            btnDelete.Enabled = false;
        }

        /// <summary>
        /// 性別コンボボックスを初期化する
        /// </summary>
        private void _InitializeGenderBox()
        {
            IList<GenderDto> list = _service.GetGenderAll();

            cmbGender.DataSource = list;
            cmbGender.ValueMember = "Id";
            cmbGender.DisplayMember = "Name";
            cmbGender.SelectedIndex = 0;
        }

        /// <summary>
        /// 部門コンボボックスを初期化する
        /// </summary>
        private void _InializeDepartmentBox()
        {
            IList<DepartmentDto> list = _service.GetDepartmentAll();

            cmbDepart.DataSource = list;
            cmbDepart.ValueMember = "Id";
            cmbDepart.DisplayMember = "Name";
            cmbDepart.SelectedIndex = 0;
        }

        /// <summary>
        /// 入力データをセットする
        /// </summary>
        /// <param name="data">セット社員データ</param>
        /// <returns>登録の可否</returns>
        private bool _SetInputData(EmployeeDto data)
        {
            bool ret = true;

            if ( _id.HasValue )
                data.Id = _id;
            else
                data.Id = null;

            // 社員コード
            if ( txtCode.Text != "" )
            {
                if ( Regex.IsMatch(txtCode.Text, @"^\d{6}") )
                {
                    data.Code = txtCode.Text;
                }
                else
                {
                    MessageBox.Show("コードに数字以外の文字があります", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ret = false;
                }
            }
            else
            {
                MessageBox.Show("コードを入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            // 社員名
            if ( txtName.Text != "" )
            {
                data.Name = txtName.Text;
            }
            else
            {
                MessageBox.Show("名前を入力してください", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ret = false;
            }

            // 性別
            data.Gender = (int) cmbGender.SelectedValue;

            // 入社日
            if ( dtpEntry.Checked )
                data.EntryDay = dtpEntry.Value;
            else
                data.EntryDay = null;
            // 部門
            data.DeptNo = (int) cmbDepart.SelectedValue;

            return ret;
        }

        /// <summary>
        /// データを表示する
        /// </summary>
        /// <param name="data">表示データ</param>
        public void _ShowData(EmployeeDto data)
        {
            txtCode.Text = data.Code;
            txtName.Text = data.Name;
            cmbGender.SelectedValue = data.Gender;
            if ( data.EntryDay.HasValue )
                dtpEntry.Value = data.EntryDay.Value;
            else
                dtpEntry.Checked = false;
            cmbDepart.SelectedValue = data.DeptNo;
        }
    }
}