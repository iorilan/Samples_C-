using System;
using NEC.ESBU.Ticketing.Client.ValidatorBase.AdminFunctions;
using NEC.ESBU.Ticketing.Client.ValidatorBase.Config;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.LocalStorage
{

    //    For offline mode, we need to capture these fields and separated by pipe “|” as the delimiter
    //·         Usage Date
    //·         Ticket Number
    //·         Facility ID
    //·         Operation ID
    //·         Validator ID


    //For the file name, please save it in this naming convention: VALIDATOR_OFFLINE_YYYYMMDD.txt
    public class OfflineValidUsage : LocalStorageBaseModel
    {
        public OfflineValidUsage(string ticketNo, string facilityId, string operationId, string itemDescription, string direction = "entry", string remarks = "")
        {
            TicketNumber = ticketNo;
            FacilityID = facilityId;
            OperationID = operationId;
            UsageDate = DateTime.Now;
            ValidatorID = ValidatorEnv.ValidatorID();
            SendedToServer = "No";
            Direction = direction;
            Remarks = remarks;
            ItemDescription = itemDescription;
            OperatorId = Login.LoggedInAdmin;
        }

        public OfflineValidUsage(string ticketNo, string direction, string itemDescription, string remarks)
            : this(ticketNo, ValidatorEnv.LocalFacilityInfo.FacilityId, ValidatorEnv.LocalFacilityInfo.OperationId, itemDescription, direction, remarks)
        {

        }


        public OfflineValidUsage() { }

        public DateTime UsageDate { get; set; }

        public string TicketNumber { get; set; }

        public string FacilityID { get; set; }

        public string OperationID { get; set; }

        public string ValidatorID { get; set; }

        public string OperatorId { get; set; }

        public string SendedToServer { get; set; }

        public string Direction { get; set; }
        public string Remarks { get; set; }
        public string ItemDescription { get; set; }

        public override string ToString()
        {
            //·         Usage Date
            //·         Ticket Number
            //·         Facility ID
            //·         Operation ID
            //·         Validator ID
            //.         OperatorID
            //.         ItemDescription
            //.         Remarks
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", UsageDate, TicketNumber, FacilityID, OperationID, ValidatorID, Login.LoggedInAdmin, ItemDescription, Remarks);
        }
    }
}
