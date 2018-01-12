
    public class ImageProcessing : IDisposable
    {
        private readonly Stream _stream;
        private Bitmap _bitmap;

        public ImageProcessing(Stream stream)
        {
            _stream = stream;
            _stream.Seek(0, 0);
        }
        
        public bool TryResize(int? width, int? height, out Stream output)
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }

            if (width == null && height == null)
            {
                throw new Exception("Must define width or height");
            }

            var src = Image.FromStream(_stream) as Bitmap;
            if (src != null)
            {
                if (width == null)
                {
                    var ratio = (Convert.ToDouble(height.Value) / src.Height);
                    width = Convert.ToInt32(src.Width * ratio);
                }
                else
                {
                    if (height == null)
                    {
                        var ratio = (Convert.ToDouble(width.Value) / src.Width);
                        height = Convert.ToInt32(src.Height * ratio);
                    }    
                }

                _bitmap = new Bitmap(width.Value, height.Value);
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.DrawImage(src, new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                        new Rectangle(0,0,src.Width, src.Height), 
                        GraphicsUnit.Pixel);

                    var memoryStream = new MemoryStream();
                    _bitmap.Save(memoryStream, src.RawFormat);

                    memoryStream.Seek(0, 0);
                    output = memoryStream;

                    return true;
                }
            }
            else
            {
                output = new MemoryStream();
                return false;
            }
        }

        public bool TryCrop(int targetWidth, int targetHeight, Rectangle sourceSize, out Stream output)
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }

            var src = Image.FromStream(_stream) as Bitmap;
            if (src != null)
            {
                _bitmap = new Bitmap(targetWidth, targetHeight);
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.DrawImage(src, new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                        sourceSize,
                        GraphicsUnit.Pixel);

                    var memoryStream = new MemoryStream();
                    _bitmap.Save(memoryStream, src.RawFormat);

                    output = memoryStream;

                    return true;
                }
            }
            else
            {
                output = new MemoryStream();
                return false;
            }
        }

        public void Dispose()
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();    
            }
        }
	}