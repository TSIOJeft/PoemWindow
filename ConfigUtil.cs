using System.IO;
using Newtonsoft.Json;

namespace LitePeom
{
    public class ConfigUtil
    {
        public ConfigArray loadConfig()
        {
            if (!File.Exists("./peomConfig/Config.json")) return new ConfigArray();
            StreamReader streamReader = new StreamReader("./peomConfig/Config.json");
            string json = streamReader.ReadToEnd();
            streamReader.Close();
            ConfigArray config = JsonConvert.DeserializeObject<ConfigArray>(json);
            return config;
        }

        public void saveConfig(ConfigArray configArray)
        {
            if (!Directory.Exists("./peomConfig")) Directory.CreateDirectory("./peomConfig");
            StreamWriter streamWriter = new StreamWriter("./peomConfig/Config.json");
            streamWriter.Write(JsonConvert.SerializeObject(configArray));
            streamWriter.Flush();
            streamWriter.Close();
        }
    }

    public class ConfigArray
    {
        public string PeomUrl = "https://v1.hitokoto.cn/?c=i";
        public string PeomKey = "hitokoto";
        public int FontSize = 20;
        public int WindowWidth = 220;
        public int WindowHeight = 60;
        public string Font = "";
    }
}