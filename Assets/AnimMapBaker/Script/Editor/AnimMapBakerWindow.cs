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

    #region FIELDS

    private static GameObject _targetGo;
    private static AnimMapBaker _baker;
    private static string _path = "DefaultPath";
    private static string _subPath = "SubPath";
    private static SaveStrategy _stratege = SaveStrategy.AnimMap;
    private static Shader _animMapShader;

    #endregion


    #region  METHODS

    [MenuItem("Window/AnimMapBaker")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimMapBakerWindow));
        _baker = new AnimMapBaker();
        _animMapShader = Shader.Find("chenjd/AnimMapShader");
    }

    private void OnGUI()
    {
        _targetGo = (GameObject)EditorGUILayout.ObjectField(_targetGo, typeof(GameObject), true);
        _subPath = _targetGo == null ? _subPath : _targetGo.name;
        EditorGUILayout.LabelField(string.Format($"output path:{Path.Combine(_path, _subPath)}"));
        _path = EditorGUILayout.TextField(_path);
        _subPath = EditorGUILayout.TextField(_subPath);

        _stratege = (SaveStrategy)EditorGUILayout.EnumPopup("output type:", _stratege);


        if (!GUILayout.Button("Bake")) return;
        if(_targetGo == null)
        {
            EditorUtility.DisplayDialog("err", "targetGo is null！", "OK");
            return;
        }

        if(_baker == null)
        {
            _baker = new AnimMapBaker();
        }

        _baker.SetAnimData(_targetGo);

        var list = _baker.Bake();

        if (list == null) return;
        foreach (var t in list)
        {
            var data = t;
            Save(ref data);
        }
    }

    private void Save(ref BakedData data)
    {
        switch(_stratege)
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
        var folderPath = CreateFolder();
        var animMap = new Texture2D(data.AnimMapWidth, data.AnimMapHeight, TextureFormat.RGBAHalf, false);
        animMap.LoadRawTextureData(data.RawAnimMap);
        AssetDatabase.CreateAsset(animMap, Path.Combine(folderPath, data.Name + ".asset"));
        return animMap;
    }

    private Material SaveAsMat(ref BakedData data)
    {
        if(_animMapShader == null)
        {
            EditorUtility.DisplayDialog("err", "shader is null!!", "OK");
            return null;
        }

        if(_targetGo == null || !_targetGo.GetComponentInChildren<SkinnedMeshRenderer>())
        {
            EditorUtility.DisplayDialog("err", "SkinnedMeshRender is null!!", "OK");
            return null;
        }

        var smr = _targetGo.GetComponentInChildren<SkinnedMeshRenderer>();
        var mat = new Material(_animMapShader);
        var animMap = SaveAsAsset(ref data);
        mat.SetTexture("_MainTex", smr.sharedMaterial.mainTexture);
        mat.SetTexture("_AnimMap", animMap);
        mat.SetFloat("_AnimLen", data.AnimLen);

        var folderPath = CreateFolder();
        AssetDatabase.CreateAsset(mat, Path.Combine(folderPath, data.Name + ".mat"));

        return mat;
    }

    private void SaveAsPrefab(ref BakedData data)
    {
        var mat = SaveAsMat(ref data);

        if(mat == null)
        {
            EditorUtility.DisplayDialog("err", "mat is null!!", "OK");
            return;
        }

        var go = new GameObject();
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        go.AddComponent<MeshFilter>().sharedMesh = _targetGo.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

        var folderPath = CreateFolder();
        PrefabUtility.SaveAsPrefabAsset(go, Path.Combine(folderPath, data.Name + ".prefab")
            .Replace("\\", "/"));
    }

    private static string CreateFolder()
    {
        var folderPath = Path.Combine("Assets/" + _path,  _subPath);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/" + _path, _subPath);
        }
        return folderPath;
    }

    #endregion


}
