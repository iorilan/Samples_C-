 public static class SiteMapExtenssions
    {
        public static bool IsOnPath(this SiteMapNode parent, SiteMapNode child, bool compareByUrl = false)
        {
            if (parent == null || child == null)
            {
                return false;
            }

            if (compareByUrl)
            {
                if (parent.Url.Equals(child.Url, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }else
            {
                if (parent == child)
                {
                    return true;
                }
            }
            

            SiteMapNode currentNode = child;
            while (currentNode.ParentNode != null)
            {
                if (compareByUrl)
                {
                    if (currentNode.ParentNode.Url.Equals(parent.Url, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }else
                {
                    if (currentNode.ParentNode == parent)
                    {
                        return true;
                    }
                }
                

                currentNode = currentNode.ParentNode;
            }

            return false;
        }
    }