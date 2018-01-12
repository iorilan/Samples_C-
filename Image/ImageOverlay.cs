 public IImage Transform(IImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            IImage newImage = null;

            RectangleF _dstRectangle = new RectangleF(_x, _y, _width, _height);

            try
            {
                newImage = (IImage)image.Clone();
                Image currentImage = (Bitmap)image.Image.Clone();
                using (Graphics graphics = Graphics.FromImage(currentImage))
                {
                    graphics.DrawImage(OverlapImage, _dstRectangle);
                    newImage.Image = currentImage;
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
		
		
		 public IImage TransformEx(IImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            IImage newImage = null;

            //叠加以后的宽度
            int overlapedWidth = X + Width;
            //叠加以后的高度
            int overlapedHeight = Y + Height;

            if (overlapedWidth < image.Width && overlapedHeight < image.Height)
            {
                //当叠加以后的区域没有超出原始图片高度的时候调用基类的处理方法
                return base.Transform(image);
            }

            int dstWidth = overlapedWidth > image.Width ? overlapedWidth : image.Width;
            int dstHeight = overlapedHeight > image.Height ? overlapedHeight : image.Height;

            try
            {
                newImage = (IImage)image.Clone();

                using (Image innerImage = image.Image, overlaped = new Bitmap(dstWidth, dstHeight, innerImage.PixelFormat))
                {
                    using (Graphics graphics = Graphics.FromImage(overlaped))
                    {
                        //绘制背景图
                        graphics.DrawImage(innerImage, 0, 0, innerImage.Width, innerImage.Height);

                        //绘制叠加的图片
                        graphics.DrawImage(OverlapImage, X, Y, Width, Height);
                    }
                    newImage.Image = overlaped;
                }

                return newImage;
            }
            catch
            {
                if (newImage != null)
                {
                    newImage.Dispose();
                }

                return null;
            }
        }