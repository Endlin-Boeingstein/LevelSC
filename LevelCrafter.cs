using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

//建立Res合成类
class LevelCrafter
{
    //合成level.json
    public void LevelCraft(string Fpath)
    {
        try
        {
            //新功能更新而停用//读取resource.json头（A.json）
            //新功能更新而停用///string res = File.ReadAllText(Fpath+"/A.json");
            //新功能更新而停用//将resource.json头（A.json）转换为JSON对象
            //新功能更新而停用///JObject rss = JObject.Parse(res);
            //创建路径文件夹level.dir实例
            DirectoryInfo TheFolder = new DirectoryInfo(Fpath);
            //建立json数组
            JArray aa = new JArray();
            //创建Waves文件夹实例
            DirectoryInfo Waves = new DirectoryInfo(Fpath + "/Waves");
            //创建文件数组
            FileInfo[] wavefiles = Waves.GetFiles();
            //为文件数组排序
            Array.Sort(wavefiles, new FileNameSort());
            //读取WaveManagerProps文本
            string wmp = File.ReadAllText(Fpath + "/" + "WaveManagerProps.json");
            //建立WaveManagerProps对象
            JObject jwmp = JObject.Parse(wmp);
            //建立数组wm
            JArray wm = new JArray();
            //遍历Waves文件夹以修改WaveManagerProps
            foreach (FileInfo NextFile in wavefiles)
            {
                //删除.json字符串
                char[] jstr = { '.', 'j', 's', 'o', 'n' };

                if (Regex.IsMatch(NextFile.Name, @"^Wave\d*\.json$"))
                {
                    //建立数组wz
                    JArray wz = new JArray();
                    //取前位以搜索
                    string wname = NextFile.Name.Substring(0, NextFile.Name.IndexOf("."));
                    foreach (FileInfo nextfile in wavefiles)
                    {
                        //记录去头字符串
                        string tnf = nextfile.Name.Trim(wname.ToCharArray());
                        string test = nextfile.Name.Substring(0, NextFile.Name.IndexOf("."));
                        if (nextfile.Name.Substring(0, NextFile.Name.IndexOf(".")) == wname)
                        {
                            if(Regex.IsMatch(tnf.Substring(0,1), @"^\d*$")) { }
                            else
                            {
                                //事件名称
                                string th = "RTID(" + nextfile.Name.TrimEnd(jstr) + "@CurrentLevel)";
                                //废弃代码，用了报错//将读取文本转换为json对象
                                //废弃代码，用了报错///JObject wobject = JObject.Parse(th);
                                //添加进子数组
                                wz.Add(th);
                            }
                        }
                        else { }
                    }
                    //添加进Waves数组
                    wm.Add(wz);
                }
                else { }
            }
            //修改原来Waves值
            jwmp["objdata"]["Waves"] = wm;
            //json数据字符串化
            string outputwmp = Newtonsoft.Json.JsonConvert.SerializeObject(jwmp, Newtonsoft.Json.Formatting.Indented);
            //输出文本
            File.WriteAllText(Fpath + "/WaveManagerProps.json", outputwmp);
            //遍历文件夹内文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if(NextFile.Name!= "#comment.json")
                {
                    //读取文件夹内文本
                    string json = File.ReadAllText(Fpath + "/" + NextFile.Name);
                    //将读取文本转换为json对象
                    JObject lobject = JObject.Parse(json);
                    //将内容放进json数组
                    aa.Add(lobject);
                }
                else { }
            }
            foreach (FileInfo NextFile in wavefiles)
            {
                //读取文件夹内文本
                string json = File.ReadAllText(Fpath + "/" +"Waves/"+ NextFile.Name);
                //将读取文本转换为json对象
                JObject lobject = JObject.Parse(json);
                //将内容放进json数组
                aa.Add(lobject);
            }
            //读取comment文本
            string jcomment = File.ReadAllText(Fpath + "/" + "#comment.json");
            //将读取文本转换为json对象
            JObject rss = JObject.Parse(jcomment);
            //废弃代码，用了报错//将读取文本转换为json对象
            //废弃代码，用了报错///JObject cobject = JObject.Parse(jcomment);
            //废弃代码，用了报错//创建jpcomment的JProperty类型
            //废弃代码，用了报错///JProperty jpcomment = (JProperty)jcomment;
            rss.Property("#comment").AddAfterSelf(new JProperty("objects", aa));
            //json数据字符串化
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(rss, Newtonsoft.Json.Formatting.Indented);
            //删除dir字符串
            char[] dirstr = {'.', 'd', 'i', 'r' };
            //输出文本
            File.WriteAllText(Path.GetDirectoryName(Fpath) + "/"+ TheFolder.Name.TrimEnd(dirstr) + ".json", output);
            //提示合成完成
            Console.WriteLine("LevelCraft Done");
        }
        catch
        {
            Console.WriteLine("LevelCraft ERROR");
            //提示按任意键继续
            Console.WriteLine("Press any key to continue...");
            //输入任意键退出
            Console.ReadLine();
        }
        
    }
}
