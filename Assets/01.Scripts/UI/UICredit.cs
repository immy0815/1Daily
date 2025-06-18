using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICredit : MonoBehaviour
{
    [SerializeField] private RectTransform creditPivot;
    [SerializeField] private float creditScrollSpeed;
    
    private void Reset()
    {
        creditPivot = GameObject.Find("CreditPivot").GetComponent<RectTransform>();
        creditScrollSpeed = 5f;
    }

    private void Awake()
    {
        Debug.Log("크레딧..여기보다 늦나?");
    }

    private void Start()
    {
        Debug.Log("크레딧 스타트 여기는?");
    }

    private void Update()
    {
        Vector2 position = creditPivot.anchoredPosition;
        position.y += creditScrollSpeed * Time.deltaTime;
        creditPivot.anchoredPosition = position;

        if (creditPivot.anchoredPosition.y >= 1360f)
        {
            OnClickSkip();
        }
    }

    public void OnClickSkip()
    {
        // TODO: 메인으로 - 안전하게 돌아가는 처리가 필요하면 작성해주세요
        UIManager.Instance.EnterScene(SceneType.Start);
    }
}
