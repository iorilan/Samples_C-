using System.Transactions;

namespace Business.Logics.DataAccessExtensions
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
