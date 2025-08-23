using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ABMenuItem
{
    [MenuItem("AB����/�������")]
    public static void OnSimgleAB() 
    {
        Object[] objects= Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (objects.Length == 0) 
        {
            Debug.Log("��ѡ���ļ�");
            return;
        }
        string folderPath = EditorUtility.OpenFolderPanel("Select Folder","","");
        if (!string.IsNullOrEmpty(folderPath)) 
        {
            AssetBundleBuild[] builds = new AssetBundleBuild[objects.Length];
            for (int i=0;i< objects.Length;i++) 
            {
                string[] AssetNames = new string[1];
                AssetNames[0] = AssetDatabase.GetAssetPath(objects[i]);
                builds[i].assetNames = AssetNames;
                builds[i].assetBundleName = objects[i].name + ".ab";
            }
            BuildPipeline.BuildAssetBundles(folderPath, builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
    [MenuItem("AB����/������")]
    public static void OnCreateAllBundle() 
    {
        Object[] objects = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);
        if (objects.Length == 0) 
        {
            Debug.Log("��ѡ���ļ�");
            return;
        }
        string FolderPath = EditorUtility.OpenFolderPanel("Save File", "", "");
        if (!string.IsNullOrEmpty(FolderPath)) 
        {
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            string[] AssetNames = new string[objects.Length];
            for (int i=0;i< objects.Length;i++) 
            {
                AssetNames[i] = AssetDatabase.GetAssetPath(objects[i]);
            }
            builds[0].assetNames = AssetNames;
            int index = AssetNames[0].LastIndexOf('/');
            string assetBundleName = AssetNames[0].Substring(0, index);
            index = assetBundleName.LastIndexOf('/');
            assetBundleName = assetBundleName.Substring(index+1);
            builds[0].assetBundleName = assetBundleName + ".ab";
            BuildPipeline.BuildAssetBundles(FolderPath, builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
    //
    [MenuItem("AB����/ֱ�Ӵ��")]
    public static void OnBundleAllAB() 
    {
        string folderPath = EditorUtility.OpenFolderPanel("Select Folder","","");
        if (!string.IsNullOrEmpty(folderPath)) 
        {
            BuildPipeline.BuildAssetBundles(folderPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
    [MenuItem("AB����/����AB/���������ļ�AB��")]
    public static void SetAllAB() 
    {
        string directoryPath = "Assets/Resources";
        ListFilesAndFolders(directoryPath,true);
    }
    [MenuItem("AB����/����AB/ȡ�������ļ�AB��")]
    public static void CancelAllAB() 
    {
        string directoryPath = "Assets/Resources";
        ListFilesAndFolders(directoryPath, false);
    }

    public static void ListFilesAndFolders(string Path,bool IsAB) 
    {
        //�г����е��ļ���
        DirectoryInfo directoryInfo = new DirectoryInfo(Path);
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
        foreach (DirectoryInfo directory in directoryInfos) 
        {
            //Debug.Log("directory.FullName = "+ directory.FullName);
            ListFilesAndFolders(directory.FullName, IsAB);
        }
        //�г����е��ļ�
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files) 
        {
            string extension = GetFileExtension(file.FullName);
            if (extension.Equals("meta")) continue;
            string UnityPath = ConvertToUnityPath(file.FullName);
            Debug.Log("file.FullName = "+ file.FullName + "  UnityPath = " + UnityPath);
            AssetImporter assetImporter = AssetImporter.GetAtPath(UnityPath);
            if (assetImporter != null)
            {
                if (IsAB)
                {
                    int index = file.Name.LastIndexOf('.');
                    string assetBundleName = file.Name.Substring(0, index);
                    assetImporter.assetBundleName = assetBundleName+".ab";
                }
                else 
                {
                    assetImporter.assetBundleName = null;
                }
            }
        }
    }
    //������·��ת��ΪUnity�����·��
    public static string ConvertToUnityPath(string absolutePath) 
    {
        //
        string projectPath = Application.dataPath;
        projectPath = projectPath.Replace('/','\\');
        if (absolutePath.StartsWith(projectPath))
        {
            return "Assets" + absolutePath.Substring(projectPath.Length);
        }
        else 
        {
            return null;
        }
    }
    //����ļ���׺
    public static string GetFileExtension(string fileName)
    {
        return Path.GetExtension(fileName).TrimStart('.');
    }
}
