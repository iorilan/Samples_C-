namespace SmppStack.Helpers
{
    public class IntegerHelper
    {
        /// <summary>
        ///     ���͸ߵ�λת��
        /// </summary>
        /// <param name="int32">��Ҫת������������</param>
        /// <returns>����ת����Ľ��</returns>
        public static int SwapInt32(int int32)
        {
            return (int32 & 0xFF) << 24 | (int32 >> 8 & 0xFF) << 16 | (int32 >> 16 & 0xFF) << 8 | (int32 >> 24 & 0xFF);
        }

        /// <summary>
        ///     ���͸ߵ�λת��
        /// </summary>
        /// <param name="int32">��Ҫת������������</param>
        /// <returns>����ת����Ľ��</returns>
        public static uint SwapInt32(uint int32)
        {
            return (int32 & 0xFF) << 24 | (int32 >> 8 & 0xFF) << 16 | (int32 >> 16 & 0xFF) << 8 | (int32 >> 24 & 0xFF);
        }
    }
}