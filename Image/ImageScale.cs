     public IImage Transform(IImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            int destinationWidth = Width;
            int destinationHeight = Height;
            int destinationX = 0;
            int destinationY = 0;

            IImage newImage = null;

            try
            {
                newImage = (IImage)image.Clone();

                using (Image innerImage = image.Image, scaled = new Bitmap(Width, Height, innerImage.PixelFormat))
                {
                    using (Graphics graphics = Graphics.FromImage(scaled))
                    {
                        float widthF = (float)innerImage.Width;
                        float heightF = (float)innerImage.Height;

                        float aspect = widthF / heightF;
                        float targetAspect = (float)Width / (float)Height;

                        if (_keepRatio && widthF != heightF)
                        {
                            float scale;

                            if (aspect < targetAspect)
                            {
                                // 变化以后高度宽度较窄
                                scale = (float)Height / (float)heightF;
                            }
                            else
                            {
                                // 变换以后目标的高度较小
                                scale = (float)Width / (float)widthF;
                            }

                            destinationHeight = System.Convert.ToInt32(heightF * scale);
                            destinationWidth = System.Convert.ToInt32(widthF * scale);
                            destinationX = (Width - destinationWidth) / 2;
                            destinationY = (Height - destinationHeight) / 2;
                        }

                        //定义图片在缩放的时候才用高质量的缩放方式
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        graphics.DrawImage(innerImage,
                            new Rectangle(destinationX, destinationY, destinationWidth, destinationHeight),
                            new Rectangle(0, 0, innerImage.Width, innerImage.Height),
                            GraphicsUnit.Pixel);
                    }

                    newImage.Image = scaled;
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