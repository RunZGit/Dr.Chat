﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Bot_Application1
{
    public class LUISClient
    {
        const string luisURL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/666e81f3-21dd-44e8-b8ef-b94723adb5a4?subscription-key=30168a99da494d81994f7830c874230f";
        public static async Task<Luis> ParseUserInput(string strInput)
        {

            string strEscaped = Uri.EscapeDataString(strInput);

            using (var client = new HttpClient())
            {
                // TODO: put URI in config file
                // TODO: insert your LUIS URL here
                string uri = luisURL + "&q=" + strEscaped;
                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    Luis _Data = JsonConvert.DeserializeObject<Luis>(jsonResponse);
                    return _Data;
                }
                else
                {
                    throw new Exception("Lui service is not running at " + luisURL);
                }
            }
        }
    }

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
        public override string ToString()
        {
            return String.Format("intent: {0}\rscore:{1}", intent, score);
        }
    }

    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
        public override string ToString()
        {
            return String.Format("intent: {0}\rscore:{1}", intent, score);
        }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
        public override string ToString()
        {
            return String.Format("entity:{0}\rtype:{1}\rstartIndex:{2}\rendIndex:{3}", entity, type, startIndex, endIndex);
        }
    }

    public class Luis
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Intent> intents { get; set; }
        public List<Entity> entities { get; set; }
        public override string ToString()
        {
            String a = "";
            if (intents == null)
                a = null;
            else
                foreach (Intent i in intents)
                    a += i.ToString();
            String b = "";
            if (entities == null)
                b = null;
            else
                foreach (Entity i in entities)
                    b += i.ToString();
            return String.Format("Queries:{0}\ntopScoring:{1}\nintent:{2}\nentities:{3}", query, topScoringIntent, a, b);
        }

        internal List<string> getList()
        {
            List<String> ls = new List<string>();
            foreach (Entity e in entities)
            {
                ls.Add(e.entity);
            }
            return ls;
        }
    }

}