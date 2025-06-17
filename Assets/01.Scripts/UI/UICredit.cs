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

    private void Update()
    {
        Vector2 position = creditPivot.anchoredPosition;
        position.y += creditScrollSpeed * Time.deltaTime;
        creditPivot.anchoredPosition = position;
    }

    public void OnClickSkip()
    {
        // TODO: 메인으로 - 안전하게 돌아가는 처리가 필요하면 작성해주세요
        SceneManager.LoadScene("StartScene");
    }
}
