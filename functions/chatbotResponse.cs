using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AdaptiveCardsBot
{
    public class chatbotResponse
    {
        #region Response by bot to user

        public string botresponse(string input)
       
        {
            List<string> xmlnamme = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\response.xml");
            XmlNodeList xmlNodeList1 = xmlDoc.DocumentElement.SelectNodes("/Chatresponse/" + input + "/Response");
            //Console.WriteLine(xmlNodeList);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("/Chatresponse");
            XmlNodeList nodeList = xmlNodeList;
            for (int i = 1; i <= xmlNodeList1.Count; i++)
            {
                string Nameqw = xmlDoc.DocumentElement.SelectSingleNode(input + "/Response[" + i + ']').InnerText;
                xmlnamme.Add(Nameqw);
                ////var Count1 = xmlnamme.Count();
                //Console.WriteLine("List ADD :" + Nameqw);
            }
            Random r1 = new Random();


            return xmlnamme[r1.Next(0,xmlnamme.Count)];
        }
        #endregion
    }
}
