using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button btnClose;
    
    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        btnClose = transform.FindChildByName<Button>("Btn_Close");
    }

    public void Initialization()
    {
        canvasGroup.SetAlpha(0);
        
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(Close);
    }

    public void Open()
    {
        canvasGroup.BlinkAnimation(1);
    }

    private void Close()
    {
        canvasGroup.BlinkAnimation(0,false);
    }
}
