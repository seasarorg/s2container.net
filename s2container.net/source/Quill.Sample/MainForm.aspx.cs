using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quill.Sample {
    public partial class MainForm : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var datas = new List<Dictionary<string, string>>();
            var data = new Dictionary<string, string>();
            data["Code"] = "123-4567";
            data["Address"] = "あいうえお";
            datas.Add(data);
            //gridDatas.DataSource = datas;
        }

        protected void btnSearch_Click(object sender, EventArgs e) {

        }

        protected void btnUpdateSuccess_Click(object sender, EventArgs e) {

        }

        protected void btnUpdateFailure_Click(object sender, EventArgs e) {

        }
    }
}