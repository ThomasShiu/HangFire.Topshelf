namespace Hangfire.Topshelf.Jobs.Model
{
  public class HR_CHGENR_Query
  {
    /// <summary> 
    /// 任免異動單編號
    /// </summary>
    public string FMNO; // nvarchar(15)

    /// <summary> 
    /// 新員工編號
    /// </summary>    
    public string NEMPLYID { get; set; }
  }
}