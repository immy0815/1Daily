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

    //private SceneLoader _sceneLoader;
    private StageLoader _stageLoader;

    [SerializeField] private GameObject _currentStage;
    private bool isAlreadyLoaded;

    private float progress;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //_sceneLoader = new SceneLoader();
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
        //_sceneLoader.Init();
        
    }
    
    public IEnumerator LoadAllResources()
    {
        if (isAlreadyLoaded)
        {
            StageManager.StartStageStatic();
            yield break;
        }

        yield return Addressables.InitializeAsync();

        int total = _stageKeys.Count;
        int loaded = 0;

        foreach (string key in _stageKeys)
        {
            yield return StartCoroutine(_stageLoader.LoadStageAsync(key));

            loaded++;
            progress = (float)loaded / total;
            UIManager.Instance.onUpdateLoadingProgress.Invoke(progress);
        }

        Debug.Log("[ResourceManager] All Resources Loaded");
        isAlreadyLoaded = true;

        // 로딩을 보여주기 위해 임시로 3초 정지
        yield return new WaitForSeconds(3f);

        StageManager.StartStageStatic();
    }

    public IEnumerator ReleaseAllResources()
    {
        Debug.Log("[ResourceManager] Releasing all resources...");

        _stageLoader.ReleaseStagePrefab();

        if (_currentStage != null)
        {
            Destroy(_currentStage);
            _currentStage = null;
        }

        yield return null;

        Debug.Log("[ResourceManager] All resources released.");
    }

    // public IEnumerator SwitchScene(SceneName targetScene)
    // {
    //     yield return _sceneLoader.LoadSceneAsync(targetScene);
    // }

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
