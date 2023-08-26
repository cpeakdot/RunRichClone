using System;
using TMPro;
using UnityEngine;

public class MoneyTextHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text plusText, minusText;
    [SerializeField] private float textFloatAmount = 2f;
    [SerializeField] private float minTextScale = .3f;

    private int currentPlusAmount = 0;
    private int currentMinusAmount = 0;
    private bool isPlusActive = false;
    private bool isMinusActive = false;

    private Vector3 plusTextInitialPosition;
    private Vector3 minusTextInitialPosition;

    private void Start()
    {
        plusTextInitialPosition = plusText.transform.localPosition;
        minusTextInitialPosition = minusText.transform.localPosition;
    }

    public void ShowMoneyText(int amount)
    {
        if (amount < 0)
        {
            ResetMinusTextPositionScale();
            currentMinusAmount += amount;
            if (!isMinusActive)
            {
                isMinusActive = true;
                minusText.gameObject.SetActive(true);
            }
            minusText.text = currentMinusAmount.ToString();
        }
        else if (amount > 0)
        {
            ResetPlusTextPositionAndScale();
            currentPlusAmount += amount;
            if (!isPlusActive)
            {
                isPlusActive = true;
                plusText.gameObject.SetActive(true);
            }
            plusText.text = "+" + currentPlusAmount;
        }
    }

    private void Update()
    {
        if (isPlusActive)
        {
            plusText.transform.position += Vector3.up * (Time.deltaTime * textFloatAmount);
            plusText.transform.localScale -= Vector3.one * (Time.deltaTime * textFloatAmount);

            if (plusText.transform.localScale.x <= minTextScale)
            {
                isPlusActive = false;
                currentPlusAmount = 0;
                plusText.gameObject.SetActive(false);
            }
        }

        if (isMinusActive)
        {
            minusText.transform.position += Vector3.up * (Time.deltaTime * textFloatAmount);
            minusText.transform.localScale -= Vector3.one * (Time.deltaTime * textFloatAmount);

            if (minusText.transform.localScale.x <= minTextScale)
            {
                isMinusActive = false;
                currentMinusAmount = 0;
                minusText.gameObject.SetActive(false);
            }
        }
    }

    private void ResetPlusTextPositionAndScale()
    {
        plusText.transform.localPosition = plusTextInitialPosition;
        plusText.transform.localScale = Vector3.one;
    }

    private void ResetMinusTextPositionScale()
    {
        minusText.transform.localPosition = minusTextInitialPosition;
        minusText.transform.localScale = Vector3.one;
    }
}
