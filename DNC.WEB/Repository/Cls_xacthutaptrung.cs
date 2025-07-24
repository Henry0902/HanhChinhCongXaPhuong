using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for Cls_xacthutaptrung
/// </summary>
public class Cls_xacthutaptrung
{
	public static string username_xacthuc()
    {
        //const string CASHOST = "http://10.16.31.5:55/cas/";
        const string CASHOST = "https://id.vnpt.com.vn/cas/";
        string tkt =HttpContext.Current.Request.QueryString["ticket"];
        // Lay dia chi ung dung local
        string service = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
        //1.1 Lay tham so ticket tu server , neu chua xac thuc qua cong sso tap doan redirect sang sso de login
        if (tkt == null || tkt.Length == 0)
        {
            string redir = CASHOST + "login?" + "service=" + service;
            HttpContext.Current.Response.Redirect(redir);
            return "";
        }
        else
        {
            // 2 Da co ticket cua sso tap doan - lay tham so giai ma thong tin nguoi dung ,len sso server
            string validateurl = CASHOST + "serviceValidate?" + "ticket=" + tkt + "&" + "service=" + service;
            StreamReader Reader = new StreamReader(new WebClient().OpenRead(validateurl));
            string resp = Reader.ReadToEnd();
            // I like to have the text in memory for debugging rather than parsing the stream

            // Giai ma tham so tra ve
            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);

            string netid = null;
            // Doc file xml lay tham so user dang nhap netId
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    string tag = reader.LocalName;
                    if (tag == "user")
                        netid = reader.ReadString();
                    //Session["username"] = netid;
                }
            }
            // if you want to parse the proxy chain, just add the logic above
            reader.Close();
            // If there was a problem, leave the message on the screen. Otherwise, return to original page.
            if (netid == null)
            {  // truong hop giai ma khong hop le 
                return "";
            }
            else
            {
                return netid;
            }

        }
    }

    public static string username_xacthuc1()
    {
        //const string CASHOST = "http://10.16.31.5:55/cas/";
        const string CASHOST = "https://id.vnpt.com.vn/cas/";
        string tkt = HttpContext.Current.Request.QueryString["ticket"];
        // Lay dia chi ung dung local
        string service = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
        //1.1 Lay tham so ticket tu server , neu chua xac thuc qua cong sso tap doan redirect sang sso de login
        if (tkt == null || tkt.Length == 0)
        {
            HttpContext.Current.Response.Redirect("https://docbao.vn");
            return "";
        }
        return "";
    }     
}