public bool ADLogin(string userName, string password)
        {
            // sample :
            // LDAP://xxx.com
            string domain = System.Configuration.ConfigurationManager.AppSettings["AD_Domain"];
            
            try
            {
                DirectoryEntry entry = new DirectoryEntry(domain, userName, password);
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = string.Format("(SAMAccountName={0})", userName);
                search.PropertiesToLoad.Add("cn");


                SearchResult result = search.FindOne();
                if (result == null)
                    return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }


            return true;
        }