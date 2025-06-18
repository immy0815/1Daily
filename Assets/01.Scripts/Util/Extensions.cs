using DG.Tweening;
using UnityEngine;

public static class Extensions
{
    // Extension: Transform
    public static T FindChildByName<T>(this Transform trans, string name) where T : Component
    {
        // 비활성화된 것까지 전부 
        T[] children = trans.GetComponentsInChildren<T>(true);
        foreach (T child in children)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
    
    // Extension: Canvas Group
    public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
    {
        if (alpha > 0.1f)
        {
            canvasGroup.alpha = alpha;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    
    public static void FadeAnimation(this CanvasGroup canvasGroup, float endValue, float duration = 0.2f)
    {
        canvasGroup.DOFade(endValue, duration).OnComplete(() => canvasGroup.SetAlpha(endValue));
    }

    public static void BlinkAnimation(this CanvasGroup canvasGroup, float endValue, bool enterAnim = true, float duration = 0.1f, int count = 2)
    {
        float startValue = canvasGroup.alpha;
        float halfDuration = duration / 2f;
        Sequence blinkSequence = DOTween.Sequence();

        if(enterAnim)
            blinkSequence.Append(canvasGroup.DOFade(endValue * 0.7f, 0.35f));
        
        for (int i = 0; i < count; i++)
        {
            blinkSequence.Append(canvasGroup.DOFade(endValue, halfDuration));
            blinkSequence.Append(canvasGroup.DOFade(startValue, halfDuration));
        }
        
        blinkSequence.Append(canvasGroup.DOFade(endValue, halfDuration));
        
        blinkSequence.OnComplete(() =>
        {
            canvasGroup.SetAlpha(endValue);
        });
    }


    public static Sequence zoomSequence;
    
    // RectTransform 확장 메서드
    public static void ZoomOut(this RectTransform rectTransform, bool endAnimPlay = true, float duration = 0.1f, float waitingDuration = 2f)
    {
        rectTransform.DOKill();
        zoomSequence?.Kill(false);
        UIManager.Instance.LensDistortionController.DOKill(); 
        
        UIManager.Instance.LensDistortionController.DOIntensity(duration);

        zoomSequence = DOTween.Sequence();
        zoomSequence.SetUpdate(true);
        
        zoomSequence.Append(rectTransform.DOScale(Vector3.one, duration).From(new Vector3(5, 5, 5)));
        
        // Waiting Animation
        if(endAnimPlay)
            zoomSequence.Append(rectTransform.DOScale(new Vector3(0.98f, 0.98f, 0.98f), waitingDuration));

        zoomSequence.OnComplete(() =>
        {
            UIBase uiBase = rectTransform.parent.GetComponent<UIBase>();
            uiBase.Close();
        });
    }
}
