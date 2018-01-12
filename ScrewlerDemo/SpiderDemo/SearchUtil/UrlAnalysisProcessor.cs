using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;
using System.IO;
using SpiderDemo.Entity;
namespace SpiderDemo.SearchUtil
{
    public static class UrlAnalysisProcessor
    {

        public static void GetHrefs(Link url, Stream s, List<Link> lnkPool)
        {
            try
            {
                ////没有HTML流，直接返回
                if (s == null)
                {
                    return;
                }

                ////解析出连接往缓存里面放，等着前面页面来拿，目前每个线程最多缓存300个，多了就别存了，那边取的太慢了！
                if (lnkPool.Count >= CacheHelper.MaxNum)
                {
                    return;
                }

                ////加载HTML，找到了HtmlAgilityPack，试试这个组件怎么样
                HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();

                ////指定了UTF8编码，理论上不会出现中文乱码了
                doc.Load(s, Encoding.Default);

                ////移除脚本
                foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
                    script.Remove();

                ////移除样式
                foreach (var style in doc.DocumentNode.Descendants("style").ToArray())
                    style.Remove();

                /////获得所有连接
                IEnumerable<HtmlNode> nodeList = doc.DocumentNode.SelectNodes("//a[@href]");

                string allText = doc.DocumentNode.InnerText;
                int index = 0;
                ////如果包含关键字，为符合条件的连接
                if ((index = allText.IndexOf(CacheHelper.KeyWord)) != -1)
                {
                    ////把包含关键字的上下文取出来
                    if (index > 20 && index < allText.Length - 20 - CacheHelper.KeyWord.Length)
                    {
                        string keyText = allText.Substring(index - 20, 20) +
                          "<span style='color:green'>" + allText.Substring(index, CacheHelper.KeyWord.Length) + "</span> " +
                           allText.Substring(index + CacheHelper.KeyWord.Length, 20) + "<br />";

                        url.Context = keyText;
                    }

                    
                    CacheHelper.validLnk.Add(url);
                    //RecordUtility.AppendLog(url.LinkName + "<br />");
                    ////爬到了一个符合条件的连接，计数器+1
                    CacheHelper.SpideNum++;
                }

                if (nodeList == null) {
                    return;
                }

                foreach (HtmlNode node in nodeList)
                {
                    if (node.Attributes["href"] == null)
                    {
                        continue;
                    }
                    else
                    {

                        Link lk = new Link()
                        {
                            Href = node.Attributes["href"].Value,
                            LinkName = "<a href='" + node.Attributes["href"].Value +
                            "' target='blank' >" + node.InnerText + "  " +
                            node.Attributes["href"].Value + "</a>" + "<br />"
                        };
                        if (lk.Href.StartsWith("javascript"))
                        {
                            continue;
                        }
                        else if (lk.Href.StartsWith("#"))
                        {
                            continue;
                        }
                        else if (lnkPool.Contains(lk))
                        {
                            continue;
                        }
                        else
                        {
                            ////添加到指定的连接池里面
                            lnkPool.Add(lk);

                        }
                    }
                }



            }

            catch (Exception ex)
            {

            }
        }
    }
}