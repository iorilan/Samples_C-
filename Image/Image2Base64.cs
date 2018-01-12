 public static string Image2Base64WithScale(Image image, int width, int height)
        {
            string strRet = string.Empty;

            try
            {
                using (Image thumb = GetThumbnailImage(image, width, height, true))
                {
                    using (Bitmap bmp = new Bitmap(width, height))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

                            g.DrawImage(thumb, 0, 0);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                ((Image)bmp).Save(ms, GetImageCodecInfo(ImageFormat.Jpeg), JpegParms());

                                strRet = Convert.ToBase64String(ms.GetBuffer());
                            }
                        }
                    }
                }
            }
            catch { }

            return strRet;
        }