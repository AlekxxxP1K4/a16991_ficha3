using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FichaIPMA;
using Newtonsoft.Json;

namespace FichaIPMA_Ex_2
{
    class Program
    {
        static Dictionary<int, string> LerLocais()
        {

           // Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            string jsonString = null;
            using (StreamReader reader =
                new StreamReader(@"../../distrits-islands.json"))
            {
                jsonString = reader.ReadToEnd();
            }

            Local obj = JsonConvert.DeserializeObject<Local>(jsonString);
            
            return obj.data.GroupBy(l => l.globalIdLocal).ToDictionary(k => k.Key, v => v.Select(f => f.local).First());

        }

        static Previsao LerFicheiroPrevisao(int globalIdLocal)
        {
            string jsonString = null;
            using (StreamReader reader =
                new StreamReader(@"../../data_forecast/" + globalIdLocal + ".json"))
            {
                jsonString = reader.ReadToEnd();
            }

            Previsao obj = JsonConvert.DeserializeObject<Previsao>(jsonString);
            return obj;
        }


        static void Main(string[] args)
        {
            Dictionary<int, string> dicLocais = LerLocais();

            // Apenas para mostrar o conteúdo da estrutura dicinário...
            foreach (KeyValuePair<int, string> kv in dicLocais)
            {
                Console.WriteLine($"globalIdLocal= {kv.Key} cidade= {kv.Value}");

                Previsao previsaoIPMA = LerFicheiroPrevisao(kv.Key);

                previsaoIPMA.local = kv.Value;


                using (StreamWriter file = File.CreateText($@"./previsaoDetail/{kv.Key}-detalhe.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, previsaoIPMA);
                }

                using (StreamWriter file = File.CreateText($@"./previsaoDetail/{previsaoIPMA.globalIdLocal}-detalhe.xml"))
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Previsao));
                    writer.Serialize(file, previsaoIPMA);
                }
            }
            Console.ReadKey();
        }
    }
}
