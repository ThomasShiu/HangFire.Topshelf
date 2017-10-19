namespace Hangfire.Topshelf.Jobs.Model
{
  public class MES_Machines_Query
  {

    /// <summary> 
    /// 機台編號
    /// </summary>
    public string MachineID { get; set; }

    /// <summary> 
    /// 機台名稱
    /// </summary>    
    public string MachineName { get; set; }

    /// <summary> 
    /// IP位址
    /// </summary>    
    public string IPaddress { get; set; }
  }


}