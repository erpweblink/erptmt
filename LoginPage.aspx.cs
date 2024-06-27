using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginPage : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session["Username"] = null;
        }
    }


edit by ashish
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        //Cls_Main.Conn_Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM TblUserMaster WHERE Username='" + txtusername.Text + "' AND Password='" + txtpassword.Text + "'", con);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@Username", txtusername.Text.Trim());
        cmd.Parameters.AddWithValue("@Password", txtpassword.Text.Trim());
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {

                string Username = dr["Username"].ToString();
                string Role = dr["Role"].ToString();
                string status = dr["Status"].ToString();
                if (status == "True")
                {
                    if (!string.IsNullOrEmpty(Username))
                    {
                       // Session["ID"] = dr["ID"].ToString();
                        Session["Username"] = dr["Username"].ToString();
                        Session["Role"] = dr["Role"].ToString();
                        txtusername.Text = ""; txtpassword.Text = "";

                        Response.Redirect("Admin/Dashboard.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Login Failed, Activate Your Account First..!!');window.location='Loginpage.aspx'; ", true);
                    // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelerror('Login Failed, Activate Your Account First..!!')", true);
                    txtusername.Text = ""; txtpassword.Text = "";
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Login Failed, Incorrect Username or Password..!!');window.location='Loginpage.aspx'; ", true);
            // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabelerror('Login Failed, Incorrect Username or Password..!!')", true);
            txtusername.Text = ""; txtpassword.Text = "";
        }
        cmd.Connection.Close();
    }
}
