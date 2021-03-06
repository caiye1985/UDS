using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using UDS.Inc;
using UDS.Components;

namespace UDS.SubModule.AssginRule
{
	/// <summary>
	/// ClassTree 的摘要说明。
	/// </summary>
	public class ClassTree : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button OK;
		protected DataTable dataTbl1,dataTbl2;
        protected System.Web.UI.WebControls.TreeView TreeView1;
	
		/// <summary>
		/// 表示要显示的根节点的ID
		/// </summary>

		private string SrcID;
		private string DisplayType;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			SrcID		= Request.QueryString["SrcID"]			!=null?Request.QueryString["SrcID"].ToString()		:"";
			DisplayType = Request.QueryString["DisplayType"]	!=null?Request.QueryString["DisplayType"].ToString():"";

			if(!Page.IsPostBack)
			{	
				InitRootNodeDataTable();
				InitTreeRootNode(TreeView1.Nodes);
				TreeView1.ExpandDepth = 1;
			}
					
			
		}
	
		private void ExpandNode(string NodeID)
		{
			
			
		}

		/// <summary>
		/// 初始化 RootNode DataTable
		/// </summary>
		private void InitRootNodeDataTable()
		{
			Database data = new Database();
			SqlDataReader dataReader = null;
			String username = Request.Cookies["Username"].Value.ToString();
			SqlParameter[] prams = {
									   data.MakeInParam("@UserName",      SqlDbType.VarChar , 20, username)
									
								   };
			try
			{
				data.RunProc("sp_GetShowClass", prams,out dataReader);
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString());
				//UDS.Components.Error.Log(ex.ToString());
			}
			dataTbl1 = dataReader.ToDataTable(true); 
			dataTbl1.TableName = "TreeView";
		}

		/// <summary>
		/// 初始化 ChildNode DataTable
		/// </summary>
		private void InitChildNodeDataTable(int ClassParentID)
		{
			Database data = new Database();
			SqlDataReader dataReader = null;
			SqlParameter[] prams = {
									   data.MakeInParam("@Class_id",      SqlDbType.Int  , 20, ClassParentID)
								   };
			try
			{
				data.RunProc("sp_GetAllChildClass", prams,out dataReader);
			}
			catch(Exception ex)
			{
				Response.Write(ex.ToString());
				//UDS.Components.Error.Log(ex.ToString());
			}
			dataTbl2 = dataReader.ToDataTable(true); 
			dataTbl2.TableName = "TreeView";
		}

		/// <summary>
		/// 初始化TreeView 的 RootNode
		/// </summary>
		private void InitTreeRootNode(TreeNodeCollection TNC)
		{
			DataView dataView  = new DataView();
			dataView		   = dataTbl1.Copy().DefaultView;
			//	dataView.RowFilter = "ClassParentID = ClassID";
			foreach(DataRowView drv in dataView)
			{	
				TreeNode tn    = new TreeNode();
				tn.Value		   = drv["ClassID"].ToString();
				tn.Text		   = "<span onmousemove=javascript:title='"+drv["ClassName"]+"'>"+drv["ClassName"].ToString()+"</span>";
				tn.ImageUrl    = GetIcon(drv["ClassType"].ToString());
				tn.NavigateUrl = "RightList.aspx?ClassID="+drv["ClassID"].ToString() + "&SrcID=" + SrcID.ToString() + "&DisplayType=" + DisplayType.ToString();
				tn.Target      = "RightList";
				TNC.Add(tn);
				InitChildNodeDataTable(Int32.Parse(tn.Value.ToString()));
				InitTreeChildNode(tn.ChildNodes,tn.Value);
			}
			dataTbl1 = null;
			dataTbl2 = null;

			TreeNode tnn    = new TreeNode();
			tnn.Value		   = "0";
			tnn.Text		   = "<span onmousemove=javascript:title='全局对象'>全局对象</span>";
			tnn.ImageUrl    = GetIcon("-1");
			tnn.NavigateUrl = "RightList.aspx?ClassID=0&SrcID=" + SrcID.ToString() + "&DisplayType=" + DisplayType.ToString();
			tnn.Target      = "RightList";
			TNC.Add(tnn);

			

		}	

		/// <summary>
		/// 初始化TreeView 的 ChildNode
		/// </summary>
		private void InitTreeChildNode(TreeNodeCollection TNC,string classParentID)
		{
			DataView dataView  = new DataView();
			dataView		   = dataTbl2.Copy().DefaultView ;
			dataView.RowFilter = "ClassParentID = " + classParentID + "";
			foreach(DataRowView drv in dataView)
			{	
				TreeNode tn    = new TreeNode();
				tn.Value = drv["ClassID"].ToString();
				tn.Text		   = "<span onmousemove=javascript:title='"+drv["ClassName"]+"'>"+drv["ClassName"].ToString()+"</span>";
				tn.ImageUrl    = GetIcon(drv["ClassType"].ToString());
				tn.NavigateUrl = "RightList.aspx?ClassID="+drv["ClassID"].ToString() + "&SrcID=" + SrcID.ToString() + "&DisplayType=" + DisplayType.ToString();
				tn.Target      = "RightList";
				TNC.Add(tn);
				InitTreeChildNode(tn.ChildNodes,tn.Value);
			}
		}	
			
		#region 获取节点图标
		/// <summary>
		/// 获取节点图标
		/// </summary>
		private string GetIcon(string ClassType)
		{
            string rtnValue = VirtualPathUtility.ToAbsolute("~/DataImages/");
			switch (ClassType)
			{
				case "0":
					rtnValue+= "flag.gif" ;
					break;
				case "1":
					rtnValue+= "myDoc.gif" ;
					break;
				case "2":
					rtnValue+= "mail.gif" ;
					break;
				case "3":
					rtnValue+= "page.gif" ;
					break;
				case "4":
					rtnValue+= "staff.gif" ;
					break;
				case "5":
					rtnValue+= "help_page.gif" ;
					break;
				case "6":
					rtnValue+= "MyTask.gif";
					break;
				case "7":
					rtnValue+= "mail.gif" ;
					break;
				case "8":
					rtnValue+= "myDoc.gif" ;
					break;
				case "9":
					rtnValue+= "DocFlow.gif" ;
					break;
				case "10":
					rtnValue+= "ClientManage.gif" ;
					break;
				case "11":
					rtnValue+= "myLinkman.gif" ;
					break;
				case "12":
					rtnValue+= "position.gif" ;
					break;
				case "13":
					rtnValue+= "role.gif" ;
					break;
				case "14":
					rtnValue+= "kaoqin.gif" ;
					break;
				case "15":
					rtnValue+= "workadmin.gif" ;
					break;
				case "16":
					rtnValue+= "message.gif" ;
					break;
				default: 
					rtnValue+= "red_ball.gif";
					break;
			}
			return rtnValue;
		}
		#endregion


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		设计器支持所需的方法 - 不要使用
		///		代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		



	}
}
