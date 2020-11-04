using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace FichaIPMA
{
    class Program
    {
        static Dictionary<int, string> LerLocais(string ficheiro)
        {

            Dictionary<int, string> dicLocais = new Dictionary<int, string>();

            // Expressão Regular para instanciar objeto Regex
            String erString = @"^[0-9]{7},[123],([1-9]?\d,){2}[A-Z]{3},([^,\n]*)$";


            Regex g = new Regex(erString);
            using (StreamReader r = new StreamReader(ficheiro))
            {
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    // Tenta correspondência (macthing) da ER com cada linha do ficheiro
                    Match m = g.Match(line);
                    if (m.Success)
                    {
                        //  estrutura de cada linha com correspondência:
                        //      globalIdLocal,idRegiao,idDistrito,idConcelho,idAreaAviso,local
                        //  separar pelas vírgulas...
                        string[] campos = m.Value.Split(',');
                        int codLocal = int.Parse(campos[0]);
                        string cidade = campos[5];
                        // Guardar na estrutura de dados dicionário
                        // dicLocais.Add( CHAVE ,  VALOR )
                        dicLocais.Add(codLocal, cidade);
                    }
                    else
                    {
                        Console.WriteLine($"Linha inválida: {line}");
                    }
                }
            }
            return dicLocais;
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
            Dictionary<int, string> dicLocais = LerLocais(@"../../locais.csv");

            // Apenas para mostrar o conteúdo da estrutura dicinário...
            foreach (KeyValuePair<int, string> kv in dicLocais)
            {
                Console.WriteLine($"globalIdLocal= {kv.Key} cidade= {kv.Value}");

                // para cada linha do ficheiro CSV ... 
                Previsao previsaoIPMA = LerFicheiroPrevisao(kv.Key);

                previsaoIPMA.local = kv.Value;


                using (StreamWriter file = File.CreateText($@"./previsaoDetail/{kv.Key}-detalhe.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, previsaoIPMA);
                }
               


            }

            Console.ReadKey();
        }
    }
}
