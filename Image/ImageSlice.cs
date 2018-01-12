 public IImage Transform(IImage image)
        {
            if (_x > image.Width || _y > image.Height)
            {
               return null;
            }

            IImage newImage = null;

            try
            {
                newImage = (IImage)image.Clone();
                Bitmap sliced;
                using (sliced = new Bitmap(_width, _height, image.Image.PixelFormat))
                {
                    using (Graphics g = Graphics.FromImage(sliced))
                    {
                        g.DrawImage(image.Image, DestRectangle, SourceRectangle, GraphicsUnit.Pixel);
                    }

                    newImage.Image = sliced;
                }

                return newImage;
            }
            catch (Exception ex)
            {
                if (newImage != null)
                {
                    newImage.Dispose();
                }
                throw ex;
            }
        }