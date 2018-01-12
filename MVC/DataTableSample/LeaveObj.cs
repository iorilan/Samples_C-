using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BCMS.DataAccess.DataModels;

namespace BCMS.BusinessLogic.Models
{
    #region Objects

    public class CreateLeaveApplicationViewModel
    {
        public string BusCaptainID { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ApplicantRemarks { get; set; }

        public string Image64BitString { get; set; }
    }

    public class CancelLeaveApplicationViewModel
    {
        public string BusCaptainID { get; set; }

        public int LeaveApplicationID { get; set; }
    }


    public enum LeaveApprovalStatus
    {
        Pending = 1,
        Rejected = 3,
        Approved = 2,
        Canceled = 4
    }

    public class LeaveApplicationObj : BaseResponseRecord
    {
        [DisplayColumn(7)]
        [SortingColumn(7)]
        public int ID { get; set; }

        [DisplayColumn(0)]
        [SortingColumn(0)]
        [DisplayName("Bc Id")]
        public string PersonnelNumber { get; set; }

        [SortingColumn(5)]
        [DisplayName("Leave Type")]
        public string LeaveType { get; set; }

        [DisplayColumn(5)]
        [DisplayName("Leave Type")]
        public string LeaveTypeFriendlyText { get; set; }

        public void UpdateFriendlyText(IList<LeaveType> leaveTypes)
        {
            var find = leaveTypes.FirstOrDefault(x => x.LeaveType1 == LeaveType);
            if (find != null)
            {
                LeaveTypeFriendlyText = find.FriendlyText;
            }
        }

        [DisplayColumn(2)]
        [SortingColumn(2)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        public long StartDateInUnixFormat { get; set; }

        [DisplayColumn(3)]
        [SortingColumn(3)]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public long EndDateInUnixFormat { get; set; }

        [DisplayName("Applicant Remarks")]
        public string ApplicantRemarks { get; set; }

        [DisplayName("Approver Remarks")]
        public string ApproverRemarks { get; set; }

        [SortingColumn(6)]
        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayColumn(6)]
        [DisplayName("Status")]
        public string StatusFriendlyText {
            get
            {
                if (this.Status == "1") return "Pending Approval";
                else if (this.Status == "3") return "Rejected";
                else if (this.Status == "2") return "Approved";
                else if (this.Status == "4") return "Cancelled";
                else return "";
            }
        }

        [DisplayColumn(4)]
        [SortingColumn(4)]
        [DisplayName("Submitted Date")]
        public DateTime SubmittedDate { get; set; }

        public long SubmittedDateInUnixFormat { get; set; }

        [DisplayName("Approval Date")]
        public DateTime ApprovalDate { get; set; }

        public long ApprovalDateInUnixFormat { get; set; }

        [DisplayColumn(1)]
        [SortingColumn(1)]
        [DisplayName("Personnel Name")]
        public string PersonnelName { get; set;  }

        public string Image64BitString { get; set; }

        public bool NeedDocument { get; set; }
    }


    [Obsolete("nowhere is using it")]
    public class LeaveQuota : BaseResponseRecord
    {
        [DisplayName("Bus Captain Id")]
        public string BusCaptainID { get; set; }

        [DisplayName("Leave Type")]
        public string LeaveType { get; set;  }

        [DisplayName("Leave Type")]
        public string LeaveTypeFriendlyText { get; set; }

        public decimal InitialQuota { get; set; }

        public decimal CurrentQuota { get; set; }

        public int LeaveYear { get; set; }
    }

    public class LeaveTypeObj : BaseResponseRecord
    {
        [DisplayName("Leave Type Code")]
        public string TypeCode {
            get
            {
                return LeaveType1;
            }
        }

        public string LeaveType1 { get; set; }

        [DisplayName("Leave Type")]
        public string FriendlyText { get; set; }

        [DisplayName("Need Attachment")]
        public bool NeedDocument { get; set; }
    }

    #endregion
}
