namespace SmppStack.Helpers
{
    public class IntegerHelper
    {
        /// <summary>
        ///     整型高低位转换
        /// </summary>
        /// <param name="int32">需要转换的整形数字</param>
        /// <returns>返回转换后的结果</returns>
        public static int SwapInt32(int int32)
        {
            return (int32 & 0xFF) << 24 | (int32 >> 8 & 0xFF) << 16 | (int32 >> 16 & 0xFF) << 8 | (int32 >> 24 & 0xFF);
        }

        /// <summary>
        ///     整型高低位转换
        /// </summary>
        /// <param name="int32">需要转换的整形数字</param>
        /// <returns>返回转换后的结果</returns>
        public static uint SwapInt32(uint int32)
        {
            return (int32 & 0xFF) << 24 | (int32 >> 8 & 0xFF) << 16 | (int32 >> 16 & 0xFF) << 8 | (int32 >> 24 & 0xFF);
        }
    }
}