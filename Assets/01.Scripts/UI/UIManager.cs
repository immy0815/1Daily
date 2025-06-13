using System;
using System.Collections.Generic;
using UnityEngine;

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

    // 로딩 할 때, progressBar UI 업데이트 시, 호출
    public Action<float> onUpdateLoadingProgress;
    
    private void Reset()
    {
        // 이후 Initialization할 때 해주기
        uiOption = GetComponentInChildren<UIOption>();
        uiStartScene = GetComponentInChildren<UIStartScene>();
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
    }

    public void OpenOption(Action closeCallback) => uiOption.Open(closeCallback);
}
