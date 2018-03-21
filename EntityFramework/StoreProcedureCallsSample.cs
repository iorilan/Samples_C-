using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Business.Logics.Helpers
{
    public class TicketingStoreProcedureCalls : ITicketingStoreProcedureCalls
    {
        public ITicketingContext Context { get; set; }

        /// <summary>
        /// should be only created from TicketingDataContext
        /// </summary>
        /// <param name="context"></param>
        public TicketingStoreProcedureCalls(ITicketingContext context)
        {
            Context = context;
        }

        public DateTime GetFirstUsageDate(TKT_Ticket_Lookup lookup)
        {
            using (var context = Context.NewInstance())
            {
                var result = context.Database.SqlQuery<SP_QueryFirstUsageDate>("EXEC SP_GetTicketFirstUsageInfo @TicketCode", new SqlParameter("TicketCode", lookup.TicketCode)).FirstOrDefault();
                return result == null ? DateTime.MinValue : result.UsageDate;
            }
        }

        public SP_TicketQueryUsageModel TicketRemainingUsage(string ticketCode, string pkgLineGroup, string facilityId, string accessId)
        {
            using (var context = Context.NewInstance())
            {
                var result = context.Database.SqlQuery<SP_TicketQueryUsageModel>("EXEC SP_QueryTicketUsage @TicketCode, @LineGroup, @FacilityID, @AccessID",
                    new SqlParameter("TicketCode", ticketCode),
                    new SqlParameter("LineGroup", pkgLineGroup),
                    new SqlParameter("FacilityID", facilityId),
                    new SqlParameter("AccessID", accessId)).FirstOrDefault();
                if (result == null)
                {
                    throw new Exception("no result returns while calling store procedure 'SP_QueryTicketUsage'");
                }
                return result;
            }
        }

        public IEnumerable<SP_CableCarUsageHistory> RetrieveCableCarUsageHistory(string ticketNumber, string facilityID)
        {
            using (var context = Context.NewInstance())
            {
                var result = context.Database.SqlQuery<SP_CableCarUsageHistory>("EXEC SP_GetCableCarUsageHistory @TicketCode, @FacilityID",
                    new SqlParameter("TicketCode", ticketNumber),
                    new SqlParameter("FacilityID", facilityID)).ToList();
                return result;
            }
        }
    }
}
