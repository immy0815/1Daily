using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    [SerializeField] private Slider sliderLoading;
    [SerializeField] private TextMeshProUGUI tmpLoadingTitle;
    [SerializeField] private TextMeshProUGUI tmpLoadingStatus;

    protected override void Reset()
    {
        base.Reset();
        
        sliderLoading = transform.FindChildByName<Slider>("Slider_Loading");
        tmpLoadingTitle = transform.FindChildByName<TextMeshProUGUI>("Tmp_SliderTitle");
        tmpLoadingStatus = transform.FindChildByName<TextMeshProUGUI>("Tmp_SliderStatus");
    }

    public override void Initialization()
    {
        base.Initialization();
        
        SetProgressBar(0);
        tmpLoadingTitle.text = string.Empty;
        tmpLoadingStatus.text = string.Empty;

        UIManager.Instance.onUpdateLoadingProgress -= SetProgressBar;
        UIManager.Instance.onUpdateLoadingProgress += SetProgressBar;
        UIManager.Instance.onUpdateLoadingProgress -= SetProgressStatus;
        UIManager.Instance.onUpdateLoadingProgress += SetProgressStatus;
    }

    private void SetProgressStatus(float progress)
    {
        float progress0To100 = progress * 100;
        tmpLoadingStatus.text = $"{progress0To100:N0}%";
    }
    
    private void SetProgressBar(float normalizedValue)
    {
        float value = Mathf.Clamp01(normalizedValue);

        if (sliderLoading != null)
        {
            sliderLoading.value = value;
        }
    }
}
