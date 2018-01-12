using System.Transactions;

namespace NEC.ESBU.Ticketing.Business.Logics.DataAccessExtensions
{
    public static class NoLockHelper 
    {
        public static TransactionScope ReadUncomitedScope()
        {
            return new TransactionScope(TransactionScopeOption.Required
                , new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
        }

    }
}
