﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/t4models).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Mapping;

namespace Hangfire.Topshelf.Jobs.DataModels
{
    /// <summary>
    /// Database       : HAMS_2017
    /// Data Source    : D:\30_Code\Test_Case\HAMS_2017.mdb
    /// Server Version : 04.00.0000
    /// </summary>
    public partial class HAMS_2017DB : LinqToDB.Data.DataConnection
    {
        public ITable<pbcatcol> pbcatcols { get { return this.GetTable<pbcatcol>(); } }
        public ITable<pbcatedt> pbcatedts { get { return this.GetTable<pbcatedt>(); } }
        public ITable<pbcatfmt> pbcatfmts { get { return this.GetTable<pbcatfmt>(); } }
        public ITable<pbcattbl> pbcattbls { get { return this.GetTable<pbcattbl>(); } }
        public ITable<pbcatvld> pbcatvlds { get { return this.GetTable<pbcatvld>(); } }
        public ITable<PubEvent> PubEvents { get { return this.GetTable<PubEvent>(); } }

        public HAMS_2017DB()
        {
            InitDataContext();
        }

        public HAMS_2017DB(string configuration)
            : base(configuration)
        {
            InitDataContext();
        }

        partial void InitDataContext();
    }

   

    [Table("PubEvent")]
    public partial class PubEvent
    {
        [PrimaryKey, Identity]
        public int rowAutoID { get; set; } // Long
        [Column, Nullable]
        public string eventType { get; set; } // text(50)
        [Column, Nullable]
        public string eventDate { get; set; } // text(10)
        [Column, NotNull]
        public string eventTime { get; set; } // text(10)
        [Column, Nullable]
        public string eventName { get; set; } // text(50)
        [Column, NotNull]
        public string eventCode { get; set; } // text(10)
        [Column, NotNull]
        public string eventCard { get; set; } // text(20)
        [Column, Nullable]
        public string eventShift { get; set; } // text(5)
        [Column, Nullable]
        public string personID { get; set; } // text(20)
        [Column, Nullable]
        public string personName { get; set; } // text(50)
        [Column, Nullable]
        public string deptID { get; set; } // text(10)
        [Column, Nullable]
        public string deptName { get; set; } // text(20)
        [Column, Nullable]
        public string deptCode { get; set; } // text(10)
        [Column, NotNull]
        public string deviceID { get; set; } // text(10)
        [Column, Nullable]
        public string deviceName { get; set; } // text(50)
        [Column, Nullable]
        public string deviceType { get; set; } // text(20)
        [Column, Nullable]
        public string doorName { get; set; } // text(20)
        [Column, NotNull]
        public string deviceL1ID { get; set; } // text(20)
        [Column, Nullable]
        public string deviceL1Name { get; set; } // text(20)
        [Column, Nullable]
        public string readerNo { get; set; } // text(4)
        [Column, Nullable]
        public char? IsKey { get; set; } // text(1)
        [Column, Nullable]
        public char? InOut { get; set; } // text(1)
        [Column, Nullable]
        public char? TransMark { get; set; } // text(1)
        [Column, Nullable]
        public char? Card_Type { get; set; } // text(1)
        [Column, Nullable]
        public string orgeventDate { get; set; } // text(10)
        [Column, Nullable]
        public string orgeventTime { get; set; } // text(10)
        [Column, Nullable]
        public char? ismanual { get; set; } // text(1)
        [Column, NotNull]
        public string pkey { get; set; } // text(20)
        [Column, Nullable]
        public string orgeventshift { get; set; } // text(5)
        [Column, Nullable]
        public string NewEventCode_Id { get; set; } // text(10)
        [Column, Nullable]
        public string NewEventCode_Name { get; set; } // text(50)
        [Column, Nullable]
        public string NewEventCode_Type { get; set; } // text(2)
    }  
}
