using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIIntro : UIBase
{
    [SerializeField] RectTransform rectTransform;

    protected override void Reset()
    {
        base.Reset();
        
        rectTransform = transform.FindChildByName<RectTransform>("Group_BorderImage");
    }
    
    public Tween SetScale(float endValue, float duration)
    {
        return rectTransform.DOScale(endValue, duration);
    }
}
