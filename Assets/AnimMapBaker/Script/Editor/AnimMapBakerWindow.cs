/*
 * Created by jiadong chen
 * http://www.chenjd.me
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimMapBakerWindow : EditorWindow {

    private enum SaveStrategy
    {
        AnimMap,//only anim map
        Mat,//with shader
        Prefab//prefab with mat
    }

    #region 字段

    public static GameObject targetGo;
    private static AnimMapBaker baker;
    private static string path = "DefaultPath";
    private static string subPath = "SubPath";
    private static SaveStrategy stratege = SaveStrategy.AnimMap;
    private static Shader animMapShader;

    #endregion


    #region  方法

    [MenuItem("Window/AnimMapBaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimMapBakerWindow));
        baker = new AnimMapBaker();
        animMapShader = Shader.Find("chenjd/AnimMapShader");
    }

    void OnGUI()
    {
        targetGo = (GameObject)EditorGUILayout.ObjectField(targetGo, typeof(GameObject), true);
        subPath = targetGo == null ? subPath : targetGo.name;
        EditorGUILayout.LabelField(string.Format("保存路径output path:{0}", Path.Combine(path, subPath)));
        path = EditorGUILayout.TextField(path);
        subPath = EditorGUILayout.TextField(subPath);

        stratege = (SaveStrategy)EditorGUILayout.EnumPopup("保存策略output type:", stratege);


        if (GUILayout.Button("Bake"))
        {
            if(targetGo == null)
            {
                EditorUtility.DisplayDialog("err", "targetGo is null！", "OK");
                return;
            }

            if(baker == null)
            {
                baker = new AnimMapBaker();
            }

            baker.SetAnimData(targetGo);

            List<BakedData> list = baker.Bake();

            if(list != null)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    BakedData data = list[i];
                    Save(ref data);
                }
            }
        }
    }


    private void Save(ref BakedData data)
    {
        switch(stratege)
        {
            case SaveStrategy.AnimMap:
                SaveAsAsset(ref data);
                break;
            case SaveStrategy.Mat:
                SaveAsMat(ref data);
                break;
            case SaveStrategy.Prefab:
                SaveAsPrefab(ref data);
                break;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Texture2D SaveAsAsset(ref BakedData data)
    {
        string folderPath = CreateFolder();
        Texture2D animMap = new Texture2D(data.animMapWidth, data.animMapHeight, TextureFormat.RGBAHalf, false);
        animMap.LoadRawTextureData(data.rawAnimMap);
        AssetDatabase.CreateAsset(animMap, Path.Combine(folderPath, data.name + ".asset"));
        return animMap;
    }

    private Material SaveAsMat(ref BakedData data)
    {
        if(animMapShader == null)
        {
            EditorUtility.DisplayDialog("err", "shader is null!!", "OK");
            return null;
        }

        if(targetGo == null || !targetGo.GetComponentInChildren<SkinnedMeshRenderer>())
        {
            EditorUtility.DisplayDialog("err", "SkinnedMeshRender is null!!", "OK");
            return null;
        }

        SkinnedMeshRenderer smr = targetGo.GetComponentInChildren<SkinnedMeshRenderer>();
        Material mat = new Material(animMapShader);
        Texture2D animMap = SaveAsAsset(ref data);
        mat.SetTexture("_MainTex", smr.sharedMaterial.mainTexture);
        mat.SetTexture("_AnimMap", animMap);
        mat.SetFloat("_AnimLen", data.animLen);

        string folderPath = CreateFolder();
        AssetDatabase.CreateAsset(mat, Path.Combine(folderPath, data.name + ".mat"));

        return mat;
    }

    private void SaveAsPrefab(ref BakedData data)
    {
        Material mat = SaveAsMat(ref data);

        if(mat == null)
        {
            EditorUtility.DisplayDialog("err", "mat is null!!", "OK");
            return;
        }

        GameObject go = new GameObject();
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        go.AddComponent<MeshFilter>().sharedMesh = targetGo.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

        string folderPath = CreateFolder();
        PrefabUtility.CreatePrefab(Path.Combine(folderPath, data.name + ".prefab").Replace("\\", "/"), go);
    }

    private string CreateFolder()
    {
        string folderPath = Path.Combine("Assets/" + path,  subPath);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/" + path, subPath);
        }
        return folderPath;
    }

    #endregion


}
