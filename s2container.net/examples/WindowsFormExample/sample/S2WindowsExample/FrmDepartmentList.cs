using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Seasar.WindowsExample.Logics.Dto;
using Seasar.WindowsExample.Logics.Service;

namespace Seasar.WindowsExample.Forms
{
    /// <summary>
    /// 部門一覧画面
    /// </summary>
    public partial class FrmDepartmentList : Form
    {
        /// <summary>
        /// 画面ディスパッチャー
        /// </summary>
        private IFormDispatcher _dispatcher;

        /// <summary>
        /// 部門一覧サービス
        /// </summary>
        private IDepartmentListService _service;

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
        /// <param name="dispatcher">画面ディスパッチャー</param>
        public FrmDepartmentList(IFormDispatcher dispatcher)
        {
            InitializeComponent();

            _InitializeGridView();

            this._dispatcher = dispatcher;
        }

        /// <summary>
        /// 部門一覧サービス
        /// </summary>
        public IDepartmentListService Service
        {
            get { return _service; }
            set { _service = value; }
        }

        /// <summary>
        /// 新規ボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                _dispatcher.ShowMasterEdit(null);
                _ShowList();
            }
            catch ( Exception ex )
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 閉じるボタンを押したときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            logger.InfoFormat("{0}を終了", Name);
            Close();
        }

        /// <summary>
        /// フォームをロードしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDepartmentList_Load(object sender, EventArgs e)
        {
            try
            {
                logger.InfoFormat("{0}がロードされました", Name);

                _ShowList();
            }
            catch ( Exception ex )
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// グリッドをダブルクリックしたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Nullable<int> id = ( Nullable<int> ) gridList.CurrentRow.Cells["ColumnId"].Value;
                _dispatcher.ShowMasterEdit(id);

                _ShowList();
            }
            catch ( Exception ex )
            {
                logger.ErrorFormat(EXCEPTION_MSG_FORMAT, ex.Message);
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// GridViewを初期化する
        /// </summary>
        private void _InitializeGridView()
        {
            gridList.RowCount = 0;
        }

        /// <summary>
        /// Gridに表示する
        /// </summary>
        private void _ShowList()
        {
            IList<DepartmentDto> list = _service.GetAll();

            gridList.DataSource = list;
        }
    }
}