using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CardsBot
{
    public class botimages
    {
        #region select random images from database
        public void carddata()
        {
            Random r = new Random();
            var fileCount = (from file in Directory.EnumerateFiles(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\bot images\", "*.jpeg", SearchOption.AllDirectories)
                             select file).Count();


            int ran = r.Next(1, fileCount);

            jsonwrirte(ran);

        }
        #endregion

        #region update card data with image location
        public void jsonwrirte(int img)
        {
            var json = File.ReadAllText(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\images.json");
            dynamic jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            string url = "C:\\Users\\kbpri\\source\\repos\\botbuilder-samples\\samples\\csharp_dotnetcore\\06.using-cards\\Resources\\bot images\\" + img + ".jpeg";

            jObject["body"][0]["url"] = url;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\Resources\images.json", output);
        }
        #endregion
    }
}
