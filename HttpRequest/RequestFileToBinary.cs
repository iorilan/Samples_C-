///  
using (var http = new HttpClient())
            {
                var url = ...

                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] byteArr = target.ToArray();

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(byteArr), "imageData", "imageData.jpg");
                HttpResponseMessage response = http.PostAsync(url, form).Result;

                var retJson = response.Content.ReadAsStringAsync().Result;

//...
            }