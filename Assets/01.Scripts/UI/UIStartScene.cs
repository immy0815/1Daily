using UnityEngine;
using UnityEngine.UI;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnOption;
    [SerializeField] private Button btnExit;
    
    [SerializeField] private RectTransform rectTrBtnStart;
    [SerializeField] private RectTransform rectTrBtnOption;
    [SerializeField] private RectTransform rectTrBtnExit;
    
    [SerializeField] private Button btnYes;
    [SerializeField] private Button btnNo;
    
    [SerializeField] private CanvasGroup canvasGroupExitPopup;
    
    private void Reset()
    {
        btnStart = transform.FindChildByName<Button>("Btn_Start");
        btnOption = transform.FindChildByName<Button>("Btn_Option");
        btnExit = transform.FindChildByName<Button>("Btn_Exit");

        rectTrBtnStart = btnStart.gameObject.GetComponent<RectTransform>();
        rectTrBtnOption = btnOption.gameObject.GetComponent<RectTransform>();
        rectTrBtnExit = btnExit.gameObject.GetComponent<RectTransform>();
        
        btnYes = transform.FindChildByName<Button>("Btn_Yes");
        btnNo = transform.FindChildByName<Button>("Btn_No");
        
        canvasGroupExitPopup = transform.FindChildByName<CanvasGroup>("Group_ExitPopup");
    }

    public void Initialization()
    {
        canvasGroupExitPopup.SetAlpha(0);
        
        // Start
        btnStart.onClick.RemoveAllListeners();
        // btnStart.onClick.AddListener();
        
        // Option
        btnOption.onClick.RemoveAllListeners();
        btnOption.onClick.AddListener(UIManager.Instance.OpenOption);
        
        // Exit
        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(ExitPopupActive);
        
        btnNo.onClick.RemoveAllListeners();
        btnNo.onClick.AddListener(ExitPopupActive);
        
        btnYes.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(ExitGame);
    }

    void ButtonAnimation()
    {
        
    }
    
    private void ExitPopupActive()
    {
        float endValue = canvasGroupExitPopup.alpha > 0.5f ? 0 : 1;
        canvasGroupExitPopup.BlinkAnimation(endValue, false);
    }
    
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
