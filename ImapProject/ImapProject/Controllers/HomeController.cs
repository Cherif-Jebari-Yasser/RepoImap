using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S22.Imap;

namespace ImapProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try {

                ImapClient imap = new ImapClient("imap.mail.yahoo.com", 993, true);
                imap.Login("******@yahoo.com", "*******", AuthMethod.Login);
                IEnumerable<uint> uids = imap.Search(SearchCondition.All());
                dynamic messages = imap.GetMessages(uids);
                List<string> bodys = new List<string>();
                for (var i = 0; i < messages.Count; i++)
                {
                    if (messages[i].From.Address == "MAILER-DAEMON@yahoo.com")
                    {
                        bodys.Add(messages[i].Body);
                    }
                }



                List<string> bounces = new List<string>();
                for (int i = 0; i < bodys.Count; i++)
                {

                    var index1 = bodys[i].IndexOf("\r\n\r\n<") + "\r\n\r\n<".Length;
                    var index2 = bodys[i].IndexOf(">:\r\n550: 5.2.1");
                    //var index1 = bodys[i].IndexOf("\r\n\r\n<") + "\r\n\r\n<".Length;
                    //var index2 = bodys[i].IndexOf(">:\r\n554:");
                    if (index1 != -1 && index2 != -1)
                    {
                        bounces.Add(bodys[i].Substring(index1, index2 - index1));
                    }

                }

                ViewBag.count = bounces.Count;
                    ViewBag.bounces = bounces;
                
                
            }
            catch (Exception e)
            {
                ViewBag.exception = e.ToString();
            }

                
           


          


            return View();
        }
    }
}