using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Game,
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField] private List<string> _stageKeys;

    private SceneLoader _sceneLoader;
    private StageLoader _stageLoader;

    private SceneName _currentScene;
    private GameObject _currentStage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _sceneLoader = new SceneLoader();
            _stageLoader = new StageLoader();
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(LoadAllResources());
    }

    private IEnumerator LoadAllResources()
    {
        yield return Addressables.InitializeAsync();

        StartCoroutine(_sceneLoader.LoadSceneAsync(SceneName.Game));

        foreach (string key in _stageKeys)
        {
            yield return StartCoroutine(_stageLoader.LoadStageAsync(key));
        }

        Debug.Log("All Resources Loaded");

        // SwitchScene(SceneName.Title);
    }

    // public void SwitchScene(SceneName targetScene)
    // {
    //     Scene current = _sceneLoader.GetScene(_currentScene);
    //     Scene target = _sceneLoader.GetScene(targetScene);
    //
    //     SceneManager.SetActiveScene(target);
    //
    //     if (current.IsValid() && current != target)
    //     {
    //         SceneManager.UnloadSceneAsync(current);
    //     }
    //
    //     _currentScene = targetScene;
    //     Debug.Log($"[ResourceManager] Switched to scene: {targetScene}");
    // }

    public void InstantiateStage(string stageKey)
    {
        if (_currentStage != null)
        {
            Destroy(_currentStage);
            _currentStage = null;
        }

        GameObject prefab = _stageLoader.GetStagePrefab(stageKey);
        if (prefab != null)
        {
            _currentStage = Instantiate(prefab);

            Debug.Log($"[ResourceManager] Instantiated stage: {stageKey}");
        }
        else
        {
            Debug.LogError($"[ResourceManager] Cannot instantiate stage. <{stageKey}> not found.");
        }
    }
}
