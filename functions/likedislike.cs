using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AdaptiveCardsBot
{
    public class likedislike
    {
        #region Like Function
        public void like(string Quote)
        {
            XmlDocument xmlDoc1 = new XmlDocument();
            // xmlDoc1.Load(@"C:\Users\kbpri\source\repos\MotivationalQuotes\Quotes.xml");
            List<string> xmlreaddata = new List<string>();
            // Create a new file in C:\\ dir  
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;
            var nodeCount = 0;
            using (var reader = XmlReader.Create("C:\\Users\\kbpri\\source\\repos\\botbuilder-samples\\samples\\csharp_dotnetcore\\06.using-cards\\Resources\\liked.xml"))
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


            string path = "C:\\Users\\kbpri\\source\\repos\\botbuilder-samples\\samples\\csharp_dotnetcore\\06.using-cards\\Resources\\liked.xml";
            XDocument doc = XDocument.Load(path);
            XElement root = new XElement("Quotes",Quote);
           root.Add(new XAttribute("ID", nodeCount+1));
            doc.Element("Likes").Add(root);
            doc.Save(path);

        }
        #endregion

        #region Dislike Function
        public void Dislike(string Quote)
        {
           // XmlDocument xmlDoc1 = new XmlDocument();
            // xmlDoc1.Load(@"C:\Users\kbpri\source\repos\MotivationalQuotes\Quotes.xml");
          //  List<string> xmlreaddata = new List<string>();
            // Create a new file in C:\\ dir  
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.Indent = true;
            var nodeCount = 0;
            using (var reader = XmlReader.Create("C:\\Users\\kbpri\\source\\repos\\botbuilder-samples\\samples\\csharp_dotnetcore\\06.using-cards\\Resources\\Disliked.xml"))
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


            string path = "C:\\Users\\kbpri\\source\\repos\\botbuilder-samples\\samples\\csharp_dotnetcore\\06.using-cards\\Resources\\Disliked.xml";
            XDocument doc = XDocument.Load(path);
            XElement root = new XElement("Quotes", Quote);
            root.Add(new XAttribute("ID", nodeCount + 1));
            doc.Element("Disliked").Add(root);
            doc.Save(path);
        }
        #endregion
    }
}
