using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class AssetBundleCreateMD5 
{
    [MenuItem("MD5/AB包生成MD5")]
    public static void OnCreateMD5() 
    {
        string folderPath = EditorUtility.OpenFolderPanel("Select Folder","","");
        if (!string.IsNullOrEmpty(folderPath)) 
        {
            string abversion = folderPath + "\\abversion.txt";
            OnDeleteVersion(abversion);
            FileInfo fileInfo = new FileInfo(abversion);
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files) 
            {
                string extension = GetFileExtension(file.FullName);
                if (!extension.Equals("ab")) continue;
                using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate)) 
                {
                    MD5CryptoServiceProvider mD5Crypto = new MD5CryptoServiceProvider();
                    byte[] buffer = mD5Crypto.ComputeHash(fs);
                    string resule = BitConverter.ToString(buffer);
                    resule = resule.Replace("-", "");
                    //resule是最终的MD5码
                    if (File.Exists(abversion))
                    {
                        using (StreamWriter writer = fileInfo.AppendText()) 
                        {
                            string filename = file.Name;
                            string format = string.Format("{0},{1}", filename, resule);
                            writer.WriteLine(format);
                            writer.Flush();
                        }
                    }
                    else 
                    {
                        using (StreamWriter writer = fileInfo.CreateText()) 
                        {
                            string filename = file.Name;
                            string format = string.Format("{0},{1}", filename, resule);
                            writer.WriteLine(format);
                            writer.Flush();
                        }
                    }
                    fs.Close();
                }
            }
        }
    }
    //删除文件
    public static void OnDeleteVersion(string abversion) 
    {
        if (File.Exists(abversion)) 
        {
            File.Delete(abversion);
        }
    }
    //获取后缀
    public static string GetFileExtension(string fileName) 
    {
        return Path.GetExtension(fileName).TrimStart('.');
    }
    [MenuItem("MD5/文件生成MD5")]
    public static void OnFileCreateMD5() 
    {
        string folderPath = EditorUtility.OpenFolderPanel("select folder","","");
        if (!string.IsNullOrEmpty(folderPath)) 
        {
            int index = folderPath.LastIndexOf('/');
            string folder = folderPath.Substring(index);
            string folderVersion = folderPath + "/" + folder + "version.txt";
            OnDeleteVersion(folderVersion);
            FileInfo fileInfo = new FileInfo(folderVersion);
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] files = directoryInfo.GetFiles();
            for (int i=0;i< files.Length;i++) 
            {
                FileInfo file = files[i];
                using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate)) 
                {
                    MD5CryptoServiceProvider mD5Crypto = new MD5CryptoServiceProvider();
                    byte[] buffer = mD5Crypto.ComputeHash(fs);
                    string result = BitConverter.ToString(buffer);
                    result = result.Replace("-", "");//MD5
                    if (File.Exists(folderVersion))
                    {
                        using (StreamWriter writer = fileInfo.AppendText()) 
                        {
                            string filename = file.Name;
                            string format = string.Format("{0},{1}", filename, result);
                            writer.WriteLine(format);
                            writer.Flush();
                        }
                    }
                    else 
                    {
                        using (StreamWriter writer = fileInfo.CreateText()) 
                        {
                            string filename = file.Name;
                            string format = string.Format("{0},{1}", filename, result);
                            writer.WriteLine(format);
                            writer.Flush();
                        }
                    }
                    fs.Close();
                }
            }
        }
    }
}
