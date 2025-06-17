using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    [SerializeField] private LensDistortionController lensDistortionController;
    public LensDistortionController LensDistortionController => lensDistortionController;
    
    [SerializeField] private Camera uiCamera;
    public Camera UICamera => uiCamera;

    [SerializeField] private UIOption uiOption;
    [SerializeField] private UIStartScene uiStartScene;
    [SerializeField] private UILoading uiLoading;
    [SerializeField] private UICrosshair uiCrosshair;
    [SerializeField] private UIEffectText uiEffectText;

    private List<UIBase> curUIList;

    // 로딩 할 때, progressBar UI 업데이트 시, 호출
    public Action<float> onUpdateLoadingProgress;
    
    private void Reset()
    {
        lensDistortionController = GetComponentInChildren<LensDistortionController>();
        // uiCamera = GetComponentInChildren<Camera>();
        
        // Resource Manager에서 GetResource와 같이 Resource를 꺼내주는 메서드가 있을 시,
        // Initialization할 때 해주기
        uiOption = GetComponentInChildren<UIOption>();
        uiStartScene = GetComponentInChildren<UIStartScene>();
        uiLoading = GetComponentInChildren<UILoading>();
        uiCrosshair = GetComponentInChildren<UICrosshair>();
        uiEffectText = GetComponentInChildren<UIEffectText>();
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
        curUIList = new List<UIBase>();
        curUIList.Add(uiOption);
        curUIList.Add(uiStartScene);
        curUIList.Add(uiLoading);
        curUIList.Add(uiCrosshair);
        curUIList.Add(uiEffectText);
        
        // 최초 실행
        SetUICamera();
        uiStartScene.Open();
    }

    public void OpenOption(Action closeCallback) => uiOption.PopupOpen(closeCallback);

    public void UpdateGUIByEnterScene(SceneType type)
    {
        // 씬 변경 시, 카메라 재할당
        SetUICamera();
        
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
                SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                uiLoading.Close();
                uiCrosshair.Open();
                break;
            default:
                Debug.Log($"SceneType {type} is not exist.");
                break;
        }
    }

    private void SetUICamera()
    {
        uiCamera = Camera.main;
        
        foreach (var uiBase in curUIList)
        {
            if (uiBase == null)
            {
                Debug.Log("Reset UIManager Script");
            }
            else
            {
                uiBase.Initialization();
            }
        }

    }

#if UNITY_EDITOR
    public void PlayEffectText()
    {
        uiEffectText.Open();
    }
#endif
}
