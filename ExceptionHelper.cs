 public static void FillAllInnerExceptionInfo(this Exception ex, ref string info)
        {
            if (ex != null)
            {
                info += ex.Message;
                if (ex.InnerException != null)
                {
                    FillAllInnerExceptionInfo(ex.InnerException, ref info);
                }
            }
        }

        public static string AllInformation(this Exception ex)
        {
            string message = "";
            if (ex != null)
            {
                message += "\r\n[Message ] " + ex.Message;
                message += "\r\n[SOURCE ] " + ex.Source;
                message += "\r\n[STACK trace ]" + ex.StackTrace;
                if (ex.InnerException != null)
                {
                    message += "\r\nInner Exception info :";
                    ex.InnerException.FillAllInnerExceptionInfo(ref message);
                }
            }

            return message;
        }