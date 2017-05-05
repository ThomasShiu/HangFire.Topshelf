using System;
using System.Data;
using System.Text;
using System.Web;
using Hangfire.Topshelf.Jobs.Model;
using Manoli.Utils.CSharpFormat;

namespace Hangfire.Topshelf.Jobs.Code
{
    /// <summary>
    /// Class GenerateHTMLReport.
    /// </summary>
    /// <remarks> �ѦҺ��}
    /// 1. http://www.manoli.net/csharpformat/
    /// </remarks>
    public class GenerateHTMLReport
    {
        /// <summary>
        /// Texts to HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        private static string TextToHtml( string text )
        {
            text = string.Format("<pre>{0}</pre>", HttpUtility.HtmlEncode(text));
            return text;
        }

        /// <summary>
        /// ����Html����
        /// </summary>
        /// <param name="aDT">�e�{��Ƥ��e</param>
        /// <param name="aRule">�ˬd�W�h</param>
        /// <returns>�Ǧ^���᪺ͫ�r��(StringBuilder)</returns>
        public static StringBuilder GenerateBody( DataTable aDT, ICheckRule aRule )
        {
            DataTable dt = aDT;

            StringBuilder strB = new StringBuilder();
            //create html & table
            strB.AppendLine("<html><head>");
            strB.AppendLine("<style type=\"text/css\">");
            strB.AppendLine("@charset \"utf-8\";");
            strB.AppendLine("body {background-color: #CCFFFF;}");
            strB.AppendLine("body, td, th {color: #333333;}");
            strB.AppendLine("h1, h2 {color: #000033;}");
            strB.AppendLine("h3, h4, h5, h6 {color: #006699;}");
            strB.AppendLine("a {color: #003366;}");
            strB.AppendLine(".csharpcode, .csharpcode pre");
            strB.AppendLine("{	font-size: small;");
            strB.AppendLine("	color: black;");
            strB.AppendLine("	font-family: Consolas, \"Courier New\", Courier, Monospace;");
            strB.AppendLine("	background-color: #ffffff;}");
            strB.AppendLine(".csharpcode pre { margin: 0em; }");
            strB.AppendLine(".csharpcode .rem { color: #008000; }");
            strB.AppendLine(".csharpcode .kwrd { color: #0000ff; }");
            strB.AppendLine(".csharpcode .str { color: #006080; }");
            strB.AppendLine(".csharpcode .op { color: #0000c0; }");
            strB.AppendLine(".csharpcode .preproc { color: #cc6633; }");
            strB.AppendLine(".csharpcode .asp { background-color: #ffff00; }");
            strB.AppendLine(".csharpcode .html { color: #800000; }");
            strB.AppendLine(".csharpcode .attr { color: #ff0000; }");
            strB.AppendLine(".csharpcode .alt ");
            strB.AppendLine("{	background-color: #f4f4f4;");
            strB.AppendLine("	width: 100%;");
            strB.AppendLine("	margin: 0em;}");
            strB.AppendLine(".csharpcode .lnum { color: #606060; }");
            strB.AppendLine("</style>");
            strB.AppendLine("<body><head>");
            strB.AppendFormat("<h1><strong>{0} �d�ֳ��i��</strong></h1>", DateTime.Now.ToString("yyyy/MM/dd"));
            strB.AppendLine("<h2>���D�G</h2><p>");
            strB.AppendFormat("<strong>{0}</strong>", aRule.TITL);
            strB.AppendLine("<P>");
            strB.AppendLine("<h2>���D�����G</h2><p>");
            strB.AppendLine(TextToHtml(aRule.REMARK));
            strB.AppendLine("<P>");
            strB.AppendFormat("��Ʈw�G{0}", aRule.DBID);
            strB.AppendLine("<h2>���~��ƦC��G</h2>");
            strB.AppendLine("<center>");
            strB.Append("<table border='2px' cellpadding='5' cellspacing='0' ");
            strB.Append("style='border: solid 1px Silver; font-size: x-small;'>");
            strB.AppendLine("<tr align='left' valign='top'>");
            //cteate table header
            foreach (DataColumn dc in dt.Columns)
            {
                strB.AppendFormat("<td valign='middle'><strong>{0}</strong></td>", dc.ColumnName);
            }
            strB.AppendLine("</tr>");
            //create table body
            foreach (DataRow dr in dt.Rows)
            {
                strB.AppendLine("<tr align='left' valign='top'>");
                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    strB.AppendFormat("<td align='center' valign='middle'>{0}</td>", cellValue);
                }
                strB.AppendLine("</tr>");
            }
            strB.AppendLine("</table></center>");  //table footer

            strB.AppendLine("<P>");
            strB.AppendLine("<h2>�d��SQL �G</h2>");
            strB.AppendLine("<P>");
            TsqlFormat sf = new TsqlFormat();
            strB.AppendLine(sf.FormatCode(aRule.LSSQL));
            // end of html file
            strB.AppendLine("</body></html>");  // end line
            return strB;
        }

        public static StringBuilder GenerateBody2(DataTable aDT, double aVal1, double aVal2)
        {
            DateTime Bdt = DateTime.Today;           
            DateTime Edt = new DateTime(Bdt.AddMonths(1).Year, Bdt.AddMonths(1).Month, 1).AddDays(-1);
            decimal sumamt1 = 0;
            decimal sumamt2 = 0;
            StringBuilder strB = new StringBuilder();
            //create html & table
            strB.AppendLine("<html><head>");
            strB.AppendLine("<html><head>");
            strB.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            strB.AppendLine("<style type=\"text/css\">");
            strB.AppendLine("@charset \"utf-8\";");
            strB.AppendLine("body {background-color: #FFFFFF;}");
            strB.AppendLine("body, td, th {color: #333333;}");
            strB.AppendLine("h1, h2 {color: #000033;}");
            strB.AppendLine("h3, h4, h5, h6 {color: #006699;}");
            strB.AppendLine("a {color: #003366;}");
            strB.AppendLine("</style><body>");
            strB.AppendFormat("<h1><strong> �C��~�Z�q�� {0}</strong></h1>",DateTime.Today.ToString("yyyy/MM/dd"));
            strB.AppendLine("<h2>�t�����ӦC��G</h2>");
            strB.AppendLine("<table border='2px' align=\"center\" cellpadding=\"0\" cellspacing=\"1\" style='border: solid 1px Silver; font-size: x-small;'>");
            strB.AppendFormat("   <tr><th colspan=\"3\" align=\"center\" valign=\"middle\">�έp�����G{0}-{1}(�ꤺ+��T)</th></tr><tr>", Edt.ToString("MM")+"01", Edt.ToString("MMdd"));
            strB.AppendLine("    <th width=\"159\" rowspan=\"2\" align=\"center\" bgcolor=\"#E2EFDA\">��ڥX�f(��~�Z)</th>");
            strB.AppendLine("    <th width=\"157\" rowspan=\"2\" align=\"center\" bgcolor=\"#FFE69E\">�P�f�}�ߵo��<br>���B(�t��)</th>");
            strB.AppendLine("    <th width=\"144\" rowspan=\"2\" align=\"center\">�t�����B</th>");
            strB.AppendLine("  </tr><tr></tr>");
            strB.AppendFormat("  <tr><th align=\"right\" bgcolor=\"#E2EFDA\">{0} </th>", aVal1);
            strB.AppendFormat("    <th align=\"right\" bgcolor=\"#FFE69E\">{0} </th>",aVal2);
            strB.AppendFormat("    <th align=\"right\">{0}</th>", aVal1- aVal2);
            strB.AppendLine("  </tr><tr><th colspan=\"3\">���H�W���B���t�B�O�B��A�B����</th></tr></table>");
            strB.AppendLine("<table border='2px' align=\"center\" cellpadding=\"0\" cellspacing=\"1\" style='border: solid 1px Silver; font-size: x-small;'>");
            strB.AppendLine("   <tr> <td colspan=\"3\" align=\"center\"><h3>�X�f�û{�C�~�Z�A�|���}�ߵo��</h3></td></tr>");
            strB.AppendLine("  <tr>");
            strB.AppendLine("    <td width=\"104\" align=\"center\"><strong>�t�d�~��</strong></td>");
            strB.AppendLine("    <td width=\"43\" align=\"center\"><strong>�Ȥ�W��</strong></td>");
            strB.AppendLine("    <td width=\"91\" align=\"center\"><strong>���B</strong></td>");
            strB.AppendLine("  </tr>");
            sumamt1 = 0;
            //create table body
            foreach (DataRow dr in aDT.Rows)
            {
                
                if (dr.Field<decimal>("VAL1") != 0) {
                    strB.AppendFormat("  <tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"right\">{2}</td></tr>",
                         dr.Field<String>("EMP_NM"),
                         dr.Field<String>("SHORT_NM"),
                         dr.Field<decimal>("VAL1")
                         );
                    sumamt1 += dr.Field<decimal>("VAL1");
                }
            }           
          
            strB.AppendLine("  <tr>");
            strB.AppendLine("    <td colspan=\"2\" align=\"center\" bgcolor=\"#FFC000\"><strong>�X�p</strong></td>");
            strB.AppendFormat("    <td align=\"right\" bgcolor=\"#FFC000\"><strong>{0} </strong></td>",sumamt1);
            strB.AppendLine("  </tr>");
            strB.AppendLine("  </table>");
            strB.AppendLine("<table border='2px' align=\"center\" cellpadding=\"0\" cellspacing=\"1\" style='border: solid 1px Silver; font-size: x-small;'>");
            strB.AppendLine("    <tr>");
            strB.AppendLine("      <td colspan=\"3\" align=\"center\"><h3>�L�h�w�{�C�~�Z�A�o��</h3></td>");
            strB.AppendLine("    </tr>");
            strB.AppendLine("    <tr>");
            strB.AppendLine("      <td width=\"78\" align=\"center\"><strong>�t�d�~��</strong></td>");
            strB.AppendLine("      <td width=\"114\" align=\"center\"><strong>�Ȥ�W��</strong></td>");
            strB.AppendLine("      <td width=\"116\" align=\"center\"><strong>���B</strong></td>");
            strB.AppendLine("    </tr>");
            //create table body
            sumamt2 = 0;
            foreach (DataRow dr in aDT.Rows)
            {
                  
                if (dr.Field<decimal>("VAL2") != 0)
                {
                    strB.AppendFormat("  <tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"right\">{2}</td></tr>",
                         dr.Field<String>("EMP_NM"),
                         dr.Field<String>("SHORT_NM"),
                         dr.Field<decimal>("VAL2")
                         );
                    sumamt2 += dr.Field<decimal>("VAL2");
                }
            }           
            strB.AppendLine("      <td colspan=\"2\" align=\"center\" bgcolor=\"#FFC000\">�X�p</td>");
            strB.AppendFormat("      <td align=\"center\" bgcolor=\"#FFC000\">{0} </td>", sumamt2);
            strB.AppendLine("    </tr>");
            strB.AppendLine("</table>");
            strB.AppendLine("  <table border='2px' align=\"center\" cellpadding=\"0\" cellspacing=\"1\" style='border: solid 1px Silver; font-size: x-small;'>");
            strB.AppendLine("  <tr><td height=\"31\" colspan=\"2\" align=\"center\" bgcolor=\"#FFCCCC\"><strong>�t���X�p</strong></td>");
            strB.AppendFormat("    <td colspan=\"4\" align=\"center\" bgcolor=\"#FFCCCC\"><strong>{0}</strong></td>",sumamt2- sumamt1);
            strB.AppendLine("  </tr></table>");
            // end of html file
            strB.AppendLine("</body></html>");  // end line
            return strB;
        }
    }
}