// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.ResourceManagement.ResourceProviders;
// using UnityEngine.SceneManagement;

// public class SceneLoader
// {
//     private Dictionary<SceneName, string> _sceneKeyMap;
//     private AsyncOperationHandle<SceneInstance>? _currentSceneHandle;

//     public void Init()
//     {
//         _sceneKeyMap = new Dictionary<SceneName, string>
//         {
//             { SceneName.Game, "Scene_Game" }
//         };
//     }

//     // ResourceManager에서만 접근하는 메서드입니다!! ResourceManager의 SwitchScene을 호출해주세요!
//     public IEnumerator LoadSceneAsync(SceneName name)
//     {
//         if (_currentSceneHandle.HasValue)
//         {
//             Addressables.Release(_currentSceneHandle.Value);
//             _currentSceneHandle = null;
//         }

//         if (!_sceneKeyMap.TryGetValue(name, out var sceneKey))
//         {
//             Debug.LogError($"[SceneLoader] No scene key mapped for {name}");
//             yield break;
//         }

//         var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single);
//         yield return handle;

//         if (handle.Status == AsyncOperationStatus.Succeeded)
//         {
//             _currentSceneHandle = handle;
//             Debug.Log($"[SceneLoader] Scene <{name}> loaded successfully.");
//         }
//         else
//         {
//             Debug.LogError($"[SceneLoader] Failed to load scene <{name}>.");
//         }
//     }

//     // ResourceManager에서만 접근하는 메서드입니다!! ResourceManager의 SwitchScene을 호출해주세요!
//     public IEnumerator UnloadSceneAsync(SceneName name)
//     {
//         var scene = SceneManager.GetSceneByName(name.ToString());

//         if (scene.isLoaded)
//         {
//             var unloadOp = SceneManager.UnloadSceneAsync(scene);
//             yield return unloadOp;

//             if (unloadOp.isDone)
//             {
//                 Debug.Log($"[SceneLoader] Scene <{name}> unloaded successfully.");
//             }
//             else
//             {
//                 Debug.LogError($"[SceneLoader] Failed to unload scene <{name}>.");
//             }
//         }
//         else
//         {
//             Debug.LogWarning($"[SceneLoader] Scene <{name}> is not currently loaded.");
//             yield return null;
//         }
//     }
// }
