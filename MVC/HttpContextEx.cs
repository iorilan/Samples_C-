  public static string TrytoGetUserHostIP
        {
            get
            {
                var currentContext = HttpContext.Current;
                if (currentContext != null)
                {
                    var hostIp = currentContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrWhiteSpace(hostIp))
                    {
                        hostIp = currentContext.Request.UserHostAddress;
                    }

                    if (hostIp == null)
                    {
                        hostIp = "unknown";
                    }

                    return hostIp;
                }

                return "no-HttpContext";
            }
        }