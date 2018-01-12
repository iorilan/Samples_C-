 public class NumberCode
    {
        public static string GenerateNumberCode(int digits)
        {
            var random = new Random();

            string result = "";

            for (int i = 0; i < digits; i++)
            {
                int randomNumber = random.Next(0, 9);
                result += string.Format("{0}", randomNumber);
            }
            return result;
        }
    }