using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topshelf.Jobs.Model
{

    /// <summary>
    /// HR_CHGENR(人事任用單)
    /// </summary>
  
    public class HR_CHGENR
    {       
        [Description("任免異動單編號")]
        public string FMNO; // nvarchar(15)

        [MaxLength(10)]
        [Description("新員工編號")]
        public string NEMPLYID; // nvarchar(10)

        [MaxLength(12)]
        [Description("員工姓名")]
        public string EMPLYNM; // nvarchar(12)

        [MaxLength(10)]
        [Description("身分證字號")]
        public string PIDNO; // nvarchar(10)

        [MaxLength(2)]
        [Description("性別")]
        public string SEX; // nvarchar(2)

        [MaxLength(1)]
        [Description("婚姻")]
        public string MARY; // nvarchar(1)

        [MaxLength(1)]
        [Description("異動別")]
        public string HRCTP; // nvarchar(1)

        [MaxLength(5)]
        [Description("血型")]
        public string BLOOD; // nvarchar(5)

        [MaxLength(60)]
        [Description("備註")]
        public string REMARK; // nvarchar(60)

        [Description("到職日")]
        public DateTime? REGDT; // datetime

        [Description("生效日")]
        public DateTime? EFFDT; // datetime

        [Description("建檔日")]
        public DateTime? CRDT; // datetime

        [MaxLength(2)]
        [Description("單況")]
        public string FMSTS; // nvarchar(2)

        [MaxLength(4)]
        [Description("廠別代號")]
        public string FA_NO; // nvarchar(4)

        [MaxLength(3)]
        [Description("公司代碼")]
        public string COMID; // nvarchar(3)

        [MaxLength(6)]
        [Description("部門代號")]
        public string DEPID; // nvarchar(6)

        [MaxLength(2)]
        [Description("職務代碼")]
        public string JOBID; // nvarchar(2)

        [MaxLength(3)]
        [Description("國籍代碼")]
        public string NATION; // nvarchar(3)

        [MaxLength(254)]
        [Description("戶籍地址")]
        public string REGADRS; // nvarchar(254)

        [MaxLength(254)]
        [Description("通訊地址")]
        public string MAILADRS; // nvarchar(254)

        [MaxLength(15)]
        [Description("行動電話")]
        public string HP; // nvarchar(15)

        [MaxLength(15)]
        [Description("連絡電話")]
        public string CONTEL; // nvarchar(15)

        [MaxLength(15)]
        [Description("匯款帳戶")]
        public string SANO; // nvarchar(15)

        [MaxLength(12)]
        [Description("連絡人姓名")]
        public string ECNTNM; // nvarchar(12)

        [MaxLength(20)]
        [Description("連絡人電話")]
        public string ECNTPHONE; // nvarchar(20)

        [MaxLength(80)]
        [Description("匯款帳名")]
        public string SANM; // nvarchar(80)

        [MaxLength(15)]
        [Description("連絡行動電話")]
        public string PHPS; // nvarchar(15)

        [MaxLength(2)]
        [Description("連絡人關係")]
        public string NOM; // nvarchar(2)

        [MaxLength(10)]
        [Description("職務代理人")]
        public string EMPLYID; // nvarchar(10)

        [MaxLength(10)]
        [Description("承辦人員")]
        public string PEMPLYID; // nvarchar(10)

        [MaxLength(1)]
        [Description("是否住宿")]
        public string PERD_05; // nvarchar(1)

        [MaxLength(1)]
        [Description("是否殘障")]
        public string PERD_06; // nvarchar(1)

        [MaxLength(10)]
        [Description("地區")]
        public string OWNCITY; // nvarchar(10)

        [MaxLength(13)]
        public string EXC_INSDBID; // nvarchar(13)

        public DateTime? EXC_INSDATE; // smalldatetime

        [MaxLength(13)]
        public string EXC_UPDDBID; // nvarchar(13)

        public DateTime? EXC_UPDDATE; // smalldatetime

        [MaxLength(5)]
        public string EXC_SYSOWNR; // nvarchar(5)

        [MaxLength(1)]
        public string EXC_ISLOCKED; // nvarchar(1)

        [Description("生日")]
        public DateTime? BRTHDT; // datetime

        [MaxLength(9)]
        [Description("機構代號")]
        public string BANKID; // nvarchar(9)

        [MaxLength(1)]
        [Description("班別代號")]
        public string SFT_NO; // nvarchar(1)
    }



}
