using System;

namespace Hangfire.Topshelf.Jobs.Model
{
  public class HR_CHGTOR_Query
  {
    public string FMNO { get; set; }

    public string EMPLYID { get; set; }

    public DateTime? FLDT { get; set; }
  }
}