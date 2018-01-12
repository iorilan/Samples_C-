using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebCode.TestEntitys;
using System.Linq;

namespace WebCode.asp.net.Repeater
{

    public partial class RepeaterTest : Page
    {

        #region properties

        protected int PageSize
        {
            get { return int.Parse(ddlPageSize.SelectedValue); }
        }

        protected int CurrentPageNo
        {
            get { return !string.IsNullOrEmpty(lblCurrentPageNo.Text) ? int.Parse(lblCurrentPageNo.Text) : -1; }
            set { lblCurrentPageNo.Text = value.ToString(); }
        }

        protected int TotalPageNo
        {
            get { return !string.IsNullOrEmpty(lblTotalPageNo.Text) ? int.Parse(lblTotalPageNo.Text) : -1; }
            set { lblTotalPageNo.Text = value.ToString(); }
        }

        protected int GoToPage { get { return int.Parse(txtPageNo.Text); } }
        #endregion

        #region Init

        protected void RegPageEvent()
        {
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            btnGoToPageNo.Click += btnGoToPageNo_Click;
            btnSubmit.Click += btnSubmit_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegPageEvent();
            if (IsPostBack) return;

            CurrentPageNo = 0;
            btnNext_Click(sender, e);
        }

        #endregion

        #region Repeater paging

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var ds = GetLastestDataSource();

            if (CurrentPageNo == 1) return;
            CurrentPageNo--;

            BindRepeater(ds);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var ds = GetLastestDataSource();

            if (CurrentPageNo == TotalPageNo) return;
            CurrentPageNo++;

            BindRepeater(ds);
        }

        private void BindRepeater(IEnumerable<Student> ds)
        {
            rptTest.DataSource = ds.Skip(PageSize * (CurrentPageNo - 1)).Take(PageSize);
            rptTest.DataBind();
        }

        private IEnumerable<Student> GetLastestDataSource()
        {
            var ds = TestDs();
            var page = ds.Count / PageSize;

            if (page * PageSize != ds.Count)
                TotalPageNo = 1 + page;
            else
                TotalPageNo = page;

            return ds;
        }

        private void btnGoToPageNo_Click(object sender, EventArgs e)
        {
           
            var ds = GetLastestDataSource();

            if (GoToPage > TotalPageNo || GoToPage < 1) return;

            CurrentPageNo = GoToPage;

            BindRepeater(ds);
        }

        #endregion

        #region Data Source
        List<Student> TestDs()
        {
            var sl = new List<Student>();
            for (var i = 1; i < 36; i++)
            {
                sl.Add(new Student()
                {
                    Name = "Stu_" + i.ToString(),
                    Id = i
                });
            }
            return sl;
        }

        #endregion

        #region Submit

        void btnSubmit_Click(object sender, EventArgs e)
        {
            var rlt = "";
            foreach (RepeaterItem item in rptTest.Items)
            {
                var chk = (item.FindControl("chkRptItem") as CheckBox);

                if (chk != null && chk.Checked)
                {
                    var id = item.FindControl("lblRptItem_Id") as Label;
                    var name = item.FindControl("lblRptItem_Name") as Label;
                    rlt += string.Format("Id: {0} , Name: {1} ", id.Text, name.Text);
                    
                }
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "", "alert('" + rlt + "');", true);
        }
        #endregion
    }


}