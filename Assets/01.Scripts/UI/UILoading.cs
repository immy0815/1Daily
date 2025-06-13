using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LoadType
{
    Scene,
    GUI,
    Resource
}

public class UILoading : MonoBehaviour
{
    [SerializeField] private Slider sliderLoading;
    [SerializeField] private TextMeshProUGUI tmpLoadingTitle;
    [SerializeField] private TextMeshProUGUI tmpLoadingStatus;

    private void Reset()
    {
        sliderLoading = transform.FindChildByName<Slider>("Slider_Loading");
        tmpLoadingTitle = transform.FindChildByName<TextMeshProUGUI>("Tmp_SliderTitle");
        tmpLoadingStatus = transform.FindChildByName<TextMeshProUGUI>("Tmp_SliderStatus");
    }

    public void Initialization()
    {
        SetProgressBar(0);
        tmpLoadingTitle.text = string.Empty;
        tmpLoadingStatus.text = string.Empty;

        UIManager.Instance.onUpdateLoadingProgress -= SetProgressBar;
        UIManager.Instance.onUpdateLoadingProgress += SetProgressBar;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    public void SetProgressTitle(LoadType type)
    {
        string text = string.Empty;
        switch (type)
        {
            case LoadType.GUI:
                text = "UI 로드 중...";
                break;
            case LoadType.Resource:
                text = "리소스 로드 중...";
                break;
            case LoadType.Scene:
                text = "씬 전환 중...";
                break;
            default:
                text = "뭐하는 지 모름...";
                break;
        }

        tmpLoadingTitle.text = text;
    }

    public void SetProgressStatus(float curProgress, float maxProgressCount = 0)
    {
        tmpLoadingStatus.text = maxProgressCount <= 0 ? $"{curProgress}%" : $"{curProgress}/{maxProgressCount}";
    }
    
    public void SetProgressBar(float normalizedValue)
    {
        float value = Mathf.Clamp01(normalizedValue);

        if (sliderLoading != null)
        {
            sliderLoading.value = value;
        }
    }
}
