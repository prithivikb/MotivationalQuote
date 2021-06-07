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
  
    public class ReadXml
    {


        #region random Motivational Quote is selected from database
        public void carddata()
        {
            StreamReader streamReader = new StreamReader(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\user.json");
            StreamReader r = streamReader;

            string json1 = r.ReadToEnd();
            List<userdata> items = JsonSerializer.Deserialize<List<userdata>>(json1);
            var occupation = items[0].Occupation;
            var nodeCount = 0;
            using (var reader = XmlReader.Create(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\Quotes.xml"))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element &&
                        reader.Name == "Quotes")
                    {
                        nodeCount++;
                    }
                }
            }
            //Console.WriteLine("Count" + nodeCount);

            //Read from Quotes Data Base
            List<string> xmlnamme = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\Quotes.xml");
            XmlNodeList xmlNodeList1 = xmlDoc.DocumentElement.SelectNodes("/Motivational_Quotes/"+occupation+"/Quotes");
            //Console.WriteLine(xmlNodeList);
            XmlNodeList xmlNodeList = xmlDoc.DocumentElement.SelectNodes("/Motivational_Quotes");
            XmlNodeList nodeList = xmlNodeList;

            // read from Disliked Quotes

            XmlDocument Disliked = new XmlDocument();
            Disliked.Load(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\Disliked.xml");
            XmlNodeList Disliked_Node = Disliked.DocumentElement.SelectNodes("/Disliked/Quotes");
            XmlNodeList Disliked_nodeList = Disliked_Node;
            List<string>  Disliked_List = new List<string>();
            if(Disliked_Node.Count !=0)
            { 
            for(int i=1;i<Disliked_Node.Count+1;i++ )
            {
                string Disliked_Quote = Disliked.DocumentElement.SelectSingleNode("/Disliked/Quotes["+i+']').InnerText;
                Disliked_List.Add(Disliked_Quote); 
            }
            }
            //Console.WriteLine("Test:" + xmlDoc.DocumentElement.SelectSingleNode("Name[" + 2 + "]").InnerText);
            // XmlNode node = nodeList;
            for (int i = 1; i <= xmlNodeList1.Count; i++)
            {
                string Nameqw = xmlDoc.DocumentElement.SelectSingleNode(occupation+"/Quotes[" + i + ']').InnerText;
                if (Disliked_List.Contains(Nameqw)) 
                {
                    continue;
                }
                else
                { 
                xmlnamme.Add(Nameqw);
                }
                ////var Count1 = xmlnamme.Count();
                //Console.WriteLine("List ADD :" + Nameqw);
            }
           // var Count = xmlnamme.Count();
           Random r1 = new Random();
            if(xmlnamme.Count!=0)
            { 
            jsonwrite(xmlnamme[r1.Next(0,xmlnamme.Count)]);
            }
            else
            {
                jsonwrite("No Quotes at the moment");
            }

        }
        #endregion

         
        #region Update card data with Motivation Quote.
        public void jsonwrite(string quote)
        {
            var json = File.ReadAllText(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\adaptiveCard.json");
            dynamic jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            // JToken jToken = jObject.SelectToken("body.text");
            jObject["body"][0]["text"] = quote;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\adaptiveCard.json", output);
           // string updatedJsonString = jObject.ToString();
          //  File.WriteAllText(@"C:\Users\kbpri\source\repos\123\samples\csharp_dotnetcore\07.using-adaptive-cards\Resources\SolitaireCard.json", updatedJsonString);
        }
        #endregion
    }
}
    

    

