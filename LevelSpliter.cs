using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

//建立Level切割类
class LevelSpliter
   {
    //新功能更新而停用//创建ps实例
    //新功能更新而停用///public Preset ps = new Preset();
    //切割resource.json
    public void LevelSplit(string Jpath)
    {
        try
        {
            //读取文本
            string json = File.ReadAllText(Jpath);
            //将读取文本转换为JSON对象
            JObject rss = JObject.Parse(json);
            //新功能更新而停用//提取slot
            //新功能更新而停用///long sc = long.Parse(rss["slot_count"].ToString());
            //新功能更新而停用//将slot值覆盖预置resource.json头的slot
            //新功能更新而停用///ps.slot_count = sc;
            //得到level.json的文件夹路径（不包括json的文件名）
            string activeDir = Path.GetDirectoryName(Jpath);
            //在原有路径基础上添加路径字条
            string newPath = System.IO.Path.Combine(activeDir, Path.GetFileNameWithoutExtension(Jpath) + ".dir");
            //创建新文件夹level.dir
            System.IO.Directory.CreateDirectory(newPath);
            //创建新文件夹Waves
            System.IO.Directory.CreateDirectory(newPath + "//Waves");
            //新功能更新而停用//分割resource.json头（A.json）至resource.dir
            //新功能更新而停用///File.WriteAllText(Path.GetDirectoryName(Jpath) + "/resource.dir/A.json", JsonConvert.SerializeObject(ps, Formatting.Indented));
            ///JObject groups = (JObject)rss["groups"];废弃代码，用了出错
            //objects转json数组
            JArray Oja = JArray.Parse(rss["objects"].ToString());
            //分解出#comment
            //废弃代码///string Comment = rss["#comment"].ToString();
            //无用代码，废弃//创建数组jac
            //无用代码，废弃///JArray jac = new JArray();
            //无用代码，废弃//加入comment
            //无用代码，废弃///jac.Add(rss.First);
            //创建comment的JObject对象
            JObject joc = new JObject();
            joc.Add(rss.First);
            //输出#comment.json文件
            File.WriteAllText(Path.GetDirectoryName(Jpath) + "\\" + Path.GetFileNameWithoutExtension(Jpath) + ".dir" + "\\" + "#comment" + ".json", JsonConvert.SerializeObject(joc, Formatting.Indented));
            //遍历输出分割后的level.json
            foreach (var item in Oja)
            {
                string aliases=null;
                if (((JObject)item).ToString().Contains("aliases"))
                    aliases = ((JObject)item)["aliases"].First.ToString();
                else aliases = "__LevelDefinition__";
                string test = Path.GetDirectoryName(Jpath) + "\\" + Path.GetFileNameWithoutExtension(Jpath) + ".dir" + "\\" + aliases + ".json";
                if(Regex.IsMatch(aliases, @"^Wave.*") && aliases!= "WaveManagerProps")
                    File.WriteAllText(Path.GetDirectoryName(Jpath) + "\\" + Path.GetFileNameWithoutExtension(Jpath) + ".dir" + "\\Waves" +"\\" + aliases + ".json", JsonConvert.SerializeObject(item, Formatting.Indented));
                else 
                    File.WriteAllText(Path.GetDirectoryName(Jpath) + "\\" + Path.GetFileNameWithoutExtension(Jpath) + ".dir" + "\\" + aliases + ".json", JsonConvert.SerializeObject(item, Formatting.Indented));
            }
            //提示分解完成
            Console.WriteLine("LevelSplit Done");
        }
        catch 
        {
            Console.WriteLine("LevelSplit ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
    }
   }
