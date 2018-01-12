 public static Guid ConvertMD5ToGuid(string md5str)
        {
            byte[] b = new byte[md5str.Length / 2];
            int index = 0;
            for (int i = 0; i < md5str.Length; i = i + 2)
            {
                string str = md5str.Substring(i, 2);
                b[index++] = Convert.ToByte(Convert.ToInt32(str, 16));
            }
            Guid id = new Guid(b);

            return id;
        }

        public static string ConvertGuidToMD5(Guid id)
        {
            byte[] data = id.ToByteArray();
            StringBuilder sBuilder = new StringBuilder(64);
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }