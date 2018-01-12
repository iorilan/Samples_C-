      private IImage MakeImageTransparent(IImage image, float percentage)
        {
            if (percentage < 0 || percentage > 1)
            {
                throw new ArgumentOutOfRangeException("Percentage should between 0 and 1!");
            }

            Image origimage = image.Image;

            if (image.Image.PixelFormat != PixelFormat.Format32bppArgb)
            {
                IImage newimage = new BaseImage(new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb));

                using (Graphics g = Graphics.FromImage(newimage.Image))
                {
                    g.DrawImage(image.Image, 0, 0);
                }

                image = newimage;
            }

            const int ARGBsize = 4;
            const int movecount = 15;

            //图片大小
            int size = image.Width * image.Height * ARGBsize;

            //缓冲区大小
            MemoryStream stream = new MemoryStream(size + (size / 4096) + 300);

            ImageCodecInfo codecinfo = GetEncoderInfo("image/tiff");
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Compression;
            EncoderParameter encparam = new EncoderParameter(encoder, (long)EncoderValue.CompressionNone);
            EncoderParameters encparams = new EncoderParameters(1); ;
            encparams.Param[0] = encparam;

            try
            {
                image.Image.Save(stream, codecinfo, encparams);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            int iPercentage = (int)(percentage * (1 << movecount));

            Byte[] buffer = stream.GetBuffer();

            // 每像素的第四个字节是alpha通道
            for (int x = 8 + 3; x < size + 8; x += ARGBsize)
            {
                buffer[x] = (byte)((buffer[x] * iPercentage) >> movecount);
            }

            if (image != origimage)
            {
                image.Dispose();
            }

            stream.Seek(0, SeekOrigin.Begin);
            image = new BaseImage(new Bitmap(stream));

            return image;
        }