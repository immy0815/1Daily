using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageLoader
{
    public Dictionary<string, GameObject> LoadedStages { get; private set; } = new();

    public IEnumerator LoadStageAsync(string stageKey)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(stageKey);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadedStages[stageKey] = handle.Result;
            Debug.Log($"[StageLoader] Stage <{stageKey}> loaded.");
        }
        else
        {
            Debug.LogError($"[StageLoader] Failed to load <{stageKey}>");
        }
    }

    public GameObject GetStagePrefab(string stageKey)
    {
        return LoadedStages.ContainsKey(stageKey) ? LoadedStages[stageKey] : null;
    }

    public void ReleaseStagePrefab()
    {
        foreach (var kvp in LoadedStages)
        {
            Addressables.Release(kvp.Value);
            Debug.Log($"[StageLoader] Stage <{kvp.Key}> released from memory.");
        }

        LoadedStages.Clear();
    }
}
