using System;

namespace Hangfire.Topshelf.Jobs.Model
{
  public class HR_HISCHD_Query
  {
    public string EMPLYID { get; set; }

    public string IDNO { get; set; }

    public DateTime? CHGDT { get; set; }

    public DateTime? DDT { get; set; }

    public string FMNO { get; set; }

  }
}