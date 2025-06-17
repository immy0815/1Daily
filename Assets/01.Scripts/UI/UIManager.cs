using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Start,
    Loading,
    Game
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance 
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    [SerializeField] private UIOption uiOption;
    [SerializeField] private UIStartScene uiStartScene;
    [SerializeField] private UILoading uiLoading;
    [SerializeField] private UICrosshair uiCrosshair;

    // 로딩 할 때, progressBar UI 업데이트 시, 호출
    public Action<float> onUpdateLoadingProgress;
    
    private void Reset()
    {
        // Resource Manager에서 GetResource와 같이 Resource를 꺼내주는 메서드가 있을 시,
        // Initialization할 때 해주기
        uiOption = GetComponentInChildren<UIOption>();
        uiStartScene = GetComponentInChildren<UIStartScene>();
        uiLoading = GetComponentInChildren<UILoading>();
        uiCrosshair = GetComponentInChildren<UICrosshair>();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        Initialization();
    }

    private void Initialization()
    {
        if (uiOption == null)
        {
            Debug.Log("UI Option is null");
        }
        else
        {
            uiOption.Initialization();
        }
        
        if (uiOption == null)
        {
            Debug.Log("UI Start Scene is null");
        }
        else
        {
            uiStartScene.Initialization();
        }

        if (uiLoading == null)
        {
            Debug.Log("UI Loading is null");
        }
        else
        {
            uiLoading.Initialization();
        }

        if (uiCrosshair == null)
        {
            Debug.Log("UI Crosshair is null");
        }
        else
        {
            uiCrosshair.Initialization();
        }
    }

    public void OpenOption(Action closeCallback) => uiOption.Open(closeCallback);

    public void UpdateGUIByEnterScene(SceneType type)
    {
        switch (type)
        {
            case SceneType.Start:
                SceneManager.LoadScene("StartScene");
                uiStartScene.Open();
                uiCrosshair.Close();
                break;
            case SceneType.Loading:
                SceneManager.LoadScene("LoadingScene");
                uiStartScene.Close();
                uiLoading.Open();
                break;
            case SceneType.Game:
                uiLoading.Close();
                uiCrosshair.Open();
                break;
            default:
                Debug.Log($"SceneType {type} is not exist.");
                break;
        }
    }
}
