using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mirror.EX_A
{
    public static partial class GameHelper_Common
    {
        public static void UILog(string str)
        {
            EX_A_TextLog.instance.AppendLog(str);
            FileLog(GameFacade.normalLogName, str);
        }

        public static void UIErr(string str)
        {
            UILog(str);
            FileLog(GameFacade.exceptionLogName, str);
            throw new Exception(str);
        }

        // TODO: 避免频繁写入，文件频繁IO影响程序性能，缓存Append日志数据，根据数量策略定时存储
        public static void FileLog(string filePath, string str)
        {
            var path = Application.persistentDataPath + "//" + $"{filePath}-{GameFacade.startTime.ToString("yyyy-MM-dd-hh-mm-ss")}.log";

            StreamWriter sw;
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                //CreateText方法限定了文本文件，只能创建文本文件
                //Create应该可以创建任何类型的文件
                sw = fileInfo.CreateText();
            }
            else
            {
                sw = fileInfo.AppendText();
            }
 
            // 写入一行
            sw.WriteLine(str);
 
            // 关闭流
            sw.Close();
 
            // 销毁
            sw.Dispose();

        }
    }
}