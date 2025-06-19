using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Canvas canvas;
    
    protected virtual void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
    }
    
    public virtual void Initialization()
    {
        canvasGroup.SetAlpha(0);

        if (canvas != null)
        {
            canvas.worldCamera = UIManager.Instance.UICamera;
            canvas.planeDistance = 1;
        }
    }
    
    public virtual void Open()
    {
        canvasGroup.SetAlpha(1);
    }
    
    public virtual void Close()
    {
        canvasGroup.SetAlpha(0);
    }
}
