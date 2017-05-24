using System;

namespace Hangfire.Topshelf.Jobs.Model
{

  /// <summary>
  /// HR_CHGENR(人事任用單)
  /// </summary>

  public class HR_CHGENR
  {
    /// <summary> 
    /// 任免異動單編號
    /// </summary>
    public string FMNO; // nvarchar(15)
    /// <summary> 
    /// 員工姓名
    /// </summary>
    public string EMPLYNM; // nvarchar(12)

    /// <summary>  
    /// 身分證字號
    /// </summary>
    public string PIDNO; // nvarchar(10)

    /// <summary>  
    /// 性別  
    /// </summary>
    public string SEX; // nvarchar(2)

    /// <summary> 
    ///婚姻 
    ///</summary>
    public string MARY; // nvarchar(1)

    /// <summary>  
    /// 異動別  
    ///</summary>
    public string HRCTP; // nvarchar(1)

    /// <summary> 
    /// 血型  
    ///</summary>
    public string BLOOD; // nvarchar(5)

    /// <summary> 
    /// 備註  
    ///</summary>
    public string REMARK; // nvarchar(60)

    /// <summary> 
    /// 到職日  
    ///</summary>
    public DateTime? REGDT; // datetime

    /// <summary>  
    /// 生效日  
    ///</summary>
    public DateTime? EFFDT; // datetime

    /// <summary> 
    /// 建檔日 
    ///</summary>
    public DateTime? CRDT; // datetime

    /// <summary> 
    /// 單況  
    ///</summary>
    public string FMSTS; // nvarchar(2)

    /// <summary>  廠別代號  </summary>
    public string FA_NO; // nvarchar(4)

    /// <summary>  公司代碼  </summary>
    public string COMID; // nvarchar(3)

    /// <summary>  部門代號  </summary>
    public string DEPID; // nvarchar(6)

    /// <summary>  職務代碼  </summary>
    public string JOBID; // nvarchar(2)

    /// <summary>  國籍代碼  </summary>
    public string NATION; // nvarchar(3)

    /// <summary>  戶籍地址  </summary>
    public string REGADRS; // nvarchar(254)

    /// <summary>  通訊地址  </summary>
    public string MAILADRS; // nvarchar(254)

    /// <summary>  行動電話  </summary>
    public string HP; // nvarchar(15)

    /// <summary>  連絡電話  </summary>
    public string CONTEL; // nvarchar(15)

    /// <summary>  匯款帳戶  </summary>
    public string SANO; // nvarchar(15)

    /// <summary>  連絡人姓名  </summary>
    public string ECNTNM; // nvarchar(12)

    /// <summary>  連絡人電話  </summary>
    public string ECNTPHONE; // nvarchar(20)

    /// <summary>  匯款帳名  </summary>
    public string SANM; // nvarchar(80)

    /// <summary>  連絡行動電話  </summary>
    public string PHPS; // nvarchar(15)

    /// <summary>  連絡人關係  </summary>
    public string NOM; // nvarchar(2)

    /// <summary>  職務代理人  </summary>
    public string EMPLYID; // nvarchar(10)

    /// <summary>  承辦人員  </summary>
    public string PEMPLYID; // nvarchar(10)

    /// <summary>  是否住宿  </summary>
    public string PERD_05; // nvarchar(1)

    ///  <summary> 
    ///  是否殘障 
    ///  </summary>
    public string PERD_06; // nvarchar(1)

    ///  <summary>
    ///  地區 
    ///  </summary>
    public string OWNCITY; // nvarchar(10)

    /// <summary>  生日  </summary>
    public DateTime? BRTHDT; // datetime

    /// <summary> 
    /// 機構代號  
    ///</summary>
    public string BANKID; // nvarchar(9)

    ///<summary
    /// 班別代號  
    ///</summary>
    public string SFT_NO; // nvarchar(1)
  }
}