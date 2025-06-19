using System;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
    [SerializeField] private Button btnClose;

    protected override void Reset()
    {
        base.Reset();
        
        btnClose = transform.FindChildByName<Button>("Btn_Close");
    }

    public override void Initialization()
    {
        canvasGroup.SetAlpha(0);
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(PopupClose);
    }

    public void PopupOpen(Action closeCallback)
    {
        canvasGroup.BlinkAnimation(1);
        
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() => { closeCallback.Invoke(); PopupClose(); });
    }

    private void PopupClose()
    {
        canvasGroup.BlinkAnimation(0,false);
    }
}
