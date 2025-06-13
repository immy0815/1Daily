using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Title,
    Game,
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField] private List<string> _stageKeys;

    private SceneLoader _sceneLoader;
    private StageLoader _stageLoader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _sceneLoader = new SceneLoader();
            _stageLoader = new StageLoader();
            _sceneLoader.Init();
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
        LoadAllScenes();

        foreach (string key in _stageKeys)
        {
            yield return StartCoroutine(_stageLoader.LoadStageAsync(key));
        }

        Debug.Log("All Resources Loaded");

        Scene titleScene = _sceneLoader.GetScene(SceneName.Title);
        if (titleScene.IsValid())
        {
            Debug.Log("Try Load Title Scene");
            SceneManager.SetActiveScene(titleScene);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
    }

    public void LoadAllScenes()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(SceneName.Title));
        StartCoroutine(_sceneLoader.LoadSceneAsync(SceneName.Game));

        Debug.Log("All Scenes Loaded");
    }

    public void SwitchScene(SceneName targetScene)
    {
        var target = _sceneLoader.GetScene(targetScene);
        if (target.IsValid())
        {
            SceneManager.SetActiveScene(target);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            Debug.Log($"Switched to scene: {target.name}");
        }
        else
        {
            Debug.LogError($"Scene '{targetScene}' not loaded");
        }
    }

    public void LoadStage(string stageKey)
    {
        GameObject prefab = _stageLoader.GetStagePrefab(stageKey);
        if (prefab != null)
        {
            Instantiate(prefab);
            Debug.Log($"[ResourceManager] Instantiated stage: {stageKey}");
        }
        else
        {
            Debug.LogError($"[ResourceManager] Cannot instantiate stage. '{stageKey}' not found.");
        }
    }
}
