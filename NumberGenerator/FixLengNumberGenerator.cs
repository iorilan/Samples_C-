/// <summary>
    /// The implementation of the damm algorithm based on the details on http://en.wikipedia.org/wiki/Damm_algorithm
    /// </summary>
    public static class FixLenNumberGenerator
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.Assembly.GetExecutingAssembly().ToString());

        /// <summary>
        /// The quasigroup table from http://en.wikipedia.org/wiki/Damm_algorithm
        /// </summary>
        /// 

        public static string GenerateTicketNumber(string itemId, string transactionNumber, string ticketType)
        {
            string result = string.Empty;

            switch (ticketType.ToUpper())
            {
                case "TICKET":
                    result = "7";
                    break;
                case "PASS":
                    result = "8";
                    break;
                case "PACKAGE":
                    result = "9";
                    break;
                default:
                    result = "7";
                    break;
            }

            result += DateTime.Now.ToString("yyyyMMdd");

            System.Threading.Thread.Sleep(1);
            Random rand = new Random();
            int seed = rand.Next() + int.Parse(DateTime.Now.ToString("HHmmssfff"));

            rand = new Random(seed);

            result += rand.Next().ToString();
            //log.Info("Ticket Number [" + result + "] length without checksum [" + result.Length + "]");

            if (result.Length > 19)
            {
                result = result.Substring(0, 19);
            }

            result = result.PadRight(19, '0');
            result = GenerateCheckSum(result);

            log.Info("Ticket Number Generated [" + result + "] length with checksum [" + result.Length + "]");

            return result;
        }

        static int[,] matrix = new int[,]
        {
            {0, 3, 1, 7, 5, 9, 8, 6, 4, 2},
            {7, 0, 9, 2, 1, 5, 4, 8, 6, 3},
            {4, 2, 0, 6, 8, 7, 1, 3, 5, 9},
            {1, 7, 5, 0, 9, 8, 3, 4, 2, 6},
            {6, 1, 2, 3, 0, 4, 5, 9, 7, 8},
            {3, 6, 7, 4, 2, 0, 9, 5, 8, 1},
            {5, 8, 6, 9, 7, 2, 0, 1, 3, 4},
            {8, 9, 4, 5, 3, 6, 2, 0, 1, 7},
            {9, 4, 3, 8, 6, 1, 7, 2, 0, 5},
            {2, 5, 8, 1, 4, 3, 6, 7, 9, 0}
        };

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public static int CalculateCheckSum(string number)
        {
            try
            {
                var numbers = (from n in number select int.Parse(n.ToString()));
                int interim = 0;
                var en = numbers.GetEnumerator();
                while (en.MoveNext())
                {
                    interim = matrix[interim, en.Current];
                }
                return interim;
            }
            catch (Exception ex)
            {
                log.Error("### (Format Validation)Ticket Number ###" + number);
                throw ex;
            }
            
        }

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public static int CalculateCheckSum(int number)
        {
            return CalculateCheckSum(number.ToString());
        }

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public static int CalculateCheckSum(long number)
        {
            return CalculateCheckSum(number.ToString());
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public static string GenerateCheckSum(string number)
        {
            var checkSumNumber = CalculateCheckSum(number);
            return number + checkSumNumber.ToString();
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public static int GenerateCheckSum(int number)
        {
            var checkSumNumber = CalculateCheckSum(number);
            return (number * 10) + checkSumNumber;
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public static long GenerateCheckSum(long number)
        {
            var checkSumNumber = CalculateCheckSum(number);
            return (number * 10) + checkSumNumber;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public static bool Validate(string number)
        {
            return CalculateCheckSum(number) == 0;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public static bool Validate(int number)
        {
            return CalculateCheckSum(number) == 0;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public static bool Validate(long number)
        {
            return CalculateCheckSum(number) == 0;
        }
    }