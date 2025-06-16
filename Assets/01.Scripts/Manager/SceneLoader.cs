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

    public IEnumerator LoadSceneAsync(SceneName name)
    {
        if (!_sceneKeyMap.TryGetValue(name, out var key))
        {
            Debug.LogError($"[SceneLoader] No key mapped for {name}");
            yield break;
        }

        var handle = Addressables.LoadSceneAsync(key, LoadSceneMode.Single);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _loadedScenes[name] = handle.Result.Scene;
            Debug.Log($"[SceneLoader] Loaded {name} as {key}");
        }
        else
        {
            Debug.LogError($"[SceneLoader] Failed to load {key}");
        }
    }

    public Scene GetScene(SceneName name)
    {
        return _loadedScenes.TryGetValue(name, out var scene) ? scene : default;
    }

    public Dictionary<SceneName, Scene> GetLoadedScenes()
    {
        return _loadedScenes;
    }
}
