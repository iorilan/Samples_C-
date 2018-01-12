using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CsvHelper;

namespace SeletarPortal
{
    public class CsvFileResult<T> : FileResult where T : class
    {
        private IEnumerable<T> _data;

        public CsvFileResult(IEnumerable<T> data)
            : base("text/csv")
        {
            _data = data;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var outPutStream = response.OutputStream;
            response.AddHeader("content-disposition", "attachment; filename=" + "export.csv");
            using (var streamWriter = new StreamWriter(outPutStream, System.Text.Encoding.UTF8))
            using (var writer = new CsvWriter(streamWriter))
            {
                //TODO : what if there are tons of records to write.
                //// without flush in between there may be hang.
                writer.WriteRecords(_data);
            }
        }

        public void WriteToStream(ref Stream stream)
        {
            //// the reason why put a copy of stream here :
            //// is that 'csvwriter' will close memory stream after writing 
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8))
            using (var writer = new CsvWriter(streamWriter))
            {
                writer.WriteRecords(_data);
            }
            stream = new MemoryStream(memoryStream.GetBuffer());

        }
    }
}