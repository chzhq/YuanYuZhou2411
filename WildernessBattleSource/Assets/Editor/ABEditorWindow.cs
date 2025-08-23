using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ABEditorWindow : EditorWindow
{
    [MenuItem("AB����/���ڴ��")]
    public static void Init() 
    {
        ABEditorWindow window = (ABEditorWindow)EditorWindow.GetWindow(typeof(ABEditorWindow));
        window.Show();
    }
    public ABEditorWindow() 
    {
        this.titleContent = new GUIContent("���ڴ��");
    }
    private void OnDestroy()
    {
        texts.Clear();
    }
    //��Դ�ĸ�Ŀ¼
    public string directoryPath = "Assets/Resources";
    private List<string> texts = new List<string>();
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(20));
        string[] subfolderNames = Directory.GetDirectories(directoryPath);
        foreach (string fileName in subfolderNames) 
        {
            int index = fileName.LastIndexOf('\\');
            string Name = fileName.Substring(index+1);
            if (GUILayout.Button(Name)) 
            {
                texts.Clear();
                string[] files = Directory.GetFiles(fileName);
                for (int i=0;i< files.Length;i++) 
                {
                    string extension = GetFileExtension(files[i]);
                    if (!extension.Equals("meta")) 
                    {
                        texts.Add(files[i]);
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        for (int i=0;i< texts.Count;i++) 
        {
            int index = texts[i].LastIndexOf('\\');
            string fileName = texts[i].Substring(index+1);
            EditorGUILayout.LabelField(fileName,GUILayout.ExpandWidth(true),GUILayout.Height(20));
        }

        if (GUILayout.Button("�������")) 
        {
            OnSingleAssetBundle();
        }
        if (GUILayout.Button("������")) 
        {
            OnCreateAllBundle();
        }
        EditorGUILayout.EndVertical();
    }
    //�������
    private void OnSingleAssetBundle() 
    {
        //���·��
        string folder = EditorUtility.OpenFolderPanel("Select Folder","","");
        if (!string.IsNullOrEmpty(folder)) 
        {
            Debug.Log("folder = "+ folder);
            //����Ҫ���������
            AssetBundleBuild[] builds = new AssetBundleBuild[texts.Count];
            for (int i=0;i< texts.Count;i++) 
            {
                string[] TestAssets = new string[1];
                TestAssets[0] = texts[i];
                builds[i].assetNames = TestAssets;//ÿ���ļ���·��
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(texts[i]);
                builds[i].assetBundleName = obj.name + ".ab";//������ļ�����
            }
            //�������
            BuildPipeline.BuildAssetBundles
                (folder, builds,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
        }
    }
    //������
    private void OnCreateAllBundle() 
    {
        string folder = EditorUtility.OpenFolderPanel("Select Folder","","");
        if (!string.IsNullOrEmpty(folder)) 
        {
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            string[] TestAssets = new string[texts.Count];
            for (int i=0;i< texts.Count;i++) 
            {
                TestAssets[i] = texts[i];
            }
            int index = texts[0].LastIndexOf('\\');
            string textName = texts[0].Substring(0, index);
            index = textName.LastIndexOf('\\');
            textName = textName.Substring(index+1);
            builds[0].assetNames = TestAssets;
            builds[0].assetBundleName = textName + ".ab";
            BuildPipeline.BuildAssetBundles(folder, builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }




    //��ȡ�ļ���׺
    public static string GetFileExtension(string fileName) 
    {
        return Path.GetExtension(fileName).TrimStart('.');
    }
}
