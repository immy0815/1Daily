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

    [SerializeField] private SceneName _currentScene;
    [SerializeField] private GameObject _currentStage;

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
        _sceneLoader.Init();
        StartCoroutine(LoadAllResources());
    }

    private IEnumerator LoadAllResources()
    {
        yield return Addressables.InitializeAsync();

        foreach (string key in _stageKeys)
        {
            yield return StartCoroutine(_stageLoader.LoadStageAsync(key));
        }

        Debug.Log("[ResourceManager] All Resources Loaded");

        // 임시
        yield return StartCoroutine(_sceneLoader.LoadSceneAsync(SceneName.Game));
        InstantiateStage("Stage1", out _currentStage);
    }

    public IEnumerator ReleaseAllResources()
    {
        Debug.Log("[ResourceManager] Releasing all resources...");

        _sceneLoader.ReleaseScenes();
        _stageLoader.ReleaseStagePrefab();

        if (_currentStage != null)
        {
            Destroy(_currentStage);
            _currentStage = null;
        }

        yield return null;

        Debug.Log("[ResourceManager] All resources released.");
    }

    public void SwitchScene(SceneName targetScene)
    {
        _currentScene = targetScene;
        StartCoroutine(_sceneLoader.LoadSceneAsync(targetScene));
    }

    public bool InstantiateStage(string stageKey, out GameObject stagePrefab)
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
            stagePrefab = _currentStage;

            Debug.Log($"[ResourceManager] Instantiated stage: {stageKey}");
            return true;
        }
        else
        {
            Debug.LogError($"[ResourceManager] Cannot instantiate stage. <{stageKey}> not found.");
            stagePrefab = null;
            return false;
        }
    }
}
