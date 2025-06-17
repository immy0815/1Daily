using TMPro;
using UnityEngine;

public class UIEffectText : UIBase
{
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private RectTransform textRectTr;
    
    protected override void Reset()
    {
        base.Reset();
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        textRectTr = tmpText.GetComponent<RectTransform>();
    }

    public override void Open()
    {
        base.Open();
        textRectTr.ZoomOut();
    }
}
