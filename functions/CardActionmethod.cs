using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot
{
    public class CardActionmethod
    {
        #region Share Function
        public string Share()
        {
            var json = File.ReadAllText(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\adaptiveCard.json");
            dynamic jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string quote = jObject["body"][0]["text"];


            return quote;
        }
        #endregion
    }
}
