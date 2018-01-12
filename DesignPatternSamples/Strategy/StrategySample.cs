namespace DesignPyttern.Strategy
{
    public abstract class BaseStategy
    {
        public virtual int GetTicketPrice(int price)
        {
            return price;
        }
    }

    public class CommonPersonStrategy : BaseStategy
    {
        public override int GetTicketPrice(int price)
        {
            return price;
        }
    }


    public class StudentStrategy : BaseStategy
    {
        public override int GetTicketPrice(int price)
        {
            return price / 2;
        }
    }

    public class Context
    {
        private readonly BaseStategy _context;
        public Context(string personType)
        {
            switch (personType)
            {
                case "c":
                    _context = new CommonPersonStrategy();
                    break;
                case "s":
                    _context = new StudentStrategy();
                    break;
            }
        }


        public int GetTicketPrice(int price)
        {
            return _context.GetTicketPrice(price);
        }
    }



}
