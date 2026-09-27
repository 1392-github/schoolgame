// 출처: https://wlsdn629.tistory.com/entry/유니티-원하는-씬으로-실행되게-하기

using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorStartInit
{
    static EditorStartInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}