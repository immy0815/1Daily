using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private Dictionary<SceneName, string> _sceneKeyMap;
    private Dictionary<SceneName, Scene> _loadedScenes = new();

    public void Init()
    {
        _sceneKeyMap = new Dictionary<SceneName, string>
        {
            { SceneName.Game, "Scene_Game" }
        };
    }

    // ResourceManager에서만 접근하는 메서드입니다!! ResourceManager의 SwitchScene을 호출해주세요!
    public IEnumerator LoadSceneAsync(SceneName name)
    {
        if (!_sceneKeyMap.TryGetValue(name, out var sceneKey))
        {
            Debug.LogError($"[SceneLoader] No scene key mapped for {name}");
            yield break;
        }

        var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Scene loadedScene = handle.Result.Scene;
            _loadedScenes[name] = loadedScene;

            Debug.Log($"[SceneLoader] Scene <{name}> loaded successfully with LoadSceneMode.Single.");
        }
        else
        {
            Debug.LogError($"[SceneLoader] Failed to load scene <{name}>.");
        }
    }

    // ResourceManager에서만 접근하는 메서드입니다!! ResourceManager의 ReleaseAllResources를 호출해주세요!
    public void ReleaseScenes()
    {
        foreach (var scene in _loadedScenes)
        {
            SceneManager.UnloadSceneAsync(scene.Value);
            Debug.Log($"[SceneLoader] Unloaded scene <{scene.Key}>");
        }

        _loadedScenes.Clear();
    }
}
