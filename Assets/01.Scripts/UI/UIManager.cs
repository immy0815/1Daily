using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Start,
    Loading,
    Game,
    Credit
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
    [SerializeField] private UIDeathScreenFX uiDeathScreenFX;
    [SerializeField] private UIIntro uiIntro;

    private List<UIBase> curUIList;

    // 로딩 할 때, progressBar UI 업데이트 시, 호출
    public Action<float> onUpdateLoadingProgress;
    
    // 플레이어 죽었을 때, 실행
    public Action onUpdateDeathAnimation;
    
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
        uiDeathScreenFX = GetComponentInChildren<UIDeathScreenFX>();
        uiIntro = transform.FindChildByName<UIIntro>("Canvas_Intro");
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
        curUIList.Add(uiDeathScreenFX);
        curUIList.Add(uiIntro);
        
        // 최초 실행
        SetUICamera();
        uiStartScene.Open();
    }

    public void OpenOption(Action closeCallback) => uiOption.PopupOpen(closeCallback);

    public void EnterScene(SceneType type)
    {
        SceneManager.sceneLoaded += UpdateGUIAfterSceneLoad;

        switch (type)
        {
            case SceneType.Start:
                SceneManager.LoadScene("StartScene");
                break;
            case SceneType.Loading:
                SceneManager.LoadScene("LoadingScene");
                break;
            case SceneType.Game:
                SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                break;
            case SceneType.Credit:
                SceneManager.LoadScene("CreditScene");
                break;
            default:
                Debug.Log($"SceneType {type} is not exist.");
                break;
        }
    }
    
    private void UpdateGUIAfterSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= UpdateGUIAfterSceneLoad;
        
        SetUICamera();

        switch (scene.name)
        {
            case "StartScene":
                uiStartScene.Open();
                break;
            case "LoadingScene":
                uiLoading.Open();
                break;
            case "GameScene":
                uiCrosshair.Open();
                break;
            case "CreditScene":
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

    public void PlayEffectText(string text)
    {
        uiEffectText.Open(text);
    }

    public void IntroAnimation()
    {
        Sequence introSequence = DOTween.Sequence();
        
        // intensity 0.5, Scale 0.8
        float duration = 0.5f;
        float initIntensity = 0;
        float endIntensity = 0.5f;
        float initScaleValue = 1;
        float endScaleValue = 0.8f;
        
        // 1번 애니메이션 시퀀스: 화면 축소 및 노이즈
        introSequence.Append(lensDistortionController.DOSetIntensity(endIntensity, duration));
        introSequence.JoinCallback(uiIntro.Open);
        introSequence.JoinCallback(uiStartScene.Open);
        introSequence.JoinCallback(() => uiDeathScreenFX.Open(0));
        introSequence.Join(lensDistortionController.DOSetScale(endScaleValue, duration));
        introSequence.Join(uiIntro.SetScale(endScaleValue, duration));
        introSequence.Join(uiDeathScreenFX.SetScale(endScaleValue, duration));
        introSequence.Join(uiStartScene.SetScale(endScaleValue, duration));
        
        // 대기: uiDeathScreenFX의 애니메이션이 끝날 때까지
        introSequence.AppendInterval(3f);
        
        // 2번째 애니메이션 시퀀스: 노이즈 Fade out
        introSequence.AppendCallback(() => uiDeathScreenFX.Close(0));
        
        // 대기: 2번째 애니메이션
        introSequence.AppendInterval(1f);
        
        // 3번째 애니메니션 시퀀스: 초기값으로 돌리기
        introSequence.Append(lensDistortionController.DOSetIntensity(initIntensity));
        introSequence.Join(lensDistortionController.DOSetScale(initScaleValue));
        introSequence.Join(uiIntro.SetScale(initScaleValue, 0.1f));
        introSequence.Join(uiDeathScreenFX.SetScale(initScaleValue, 0.1f));
        introSequence.Join(uiStartScene.SetScale(initScaleValue, 0.1f));
        
        // 대기: 3번째 애니메이션
        introSequence.AppendInterval(0.5f);
        
        introSequence.AppendCallback(uiIntro.Close);
        
        // 끝나면 텍스트 애니메이션 재생 SUPER, COOL // 2.1초 뒤 끝남..
        introSequence.AppendCallback(() => PlayEffectText("SUPER"));
        introSequence.AppendInterval(1f); // 그냥 1초로 끊기
        introSequence.AppendCallback(() => PlayEffectText("COOL"));
        
        // 전부 재생 후 시퀀스 삭제
        introSequence.SetAutoKill(true);
    }
}
