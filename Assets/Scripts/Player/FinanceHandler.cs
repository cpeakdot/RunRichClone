using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RRC.Core;

namespace RRC.Player
{
    public class FinanceHandler : MonoBehaviour
    {
        [SerializeField] private FinanceSettings[] financeSettings;
        [SerializeField] private TMP_Text financeStatusText;
        [SerializeField] private Slider financeStatusSlider;
        [SerializeField] private float sliderSmootherValue = .2f;
        [SerializeField] private TMP_Text moneyText;
        private FinanceState financeState = FinanceState.Poor;

        private int currentMoneyAmount = 0;
        private int lastMoneyAmount = 0;

        private readonly string MONEY_PREF_KEY = "mn__";

        public FinanceState GetFinanceState => financeState;
        
        /// <summary>
        /// True if player got rich, false if poor
        /// </summary>
        public event Action<FinanceState> OnFinancialStatusChanged;

        private void Start()
        {
            financeStatusSlider.maxValue = financeSettings[^1].minAmountToHave;
            SetSliderValue(0);
        }


        public void UpdateMoney(int amount)
        {
            currentMoneyAmount = Mathf.Max(0, amount + currentMoneyAmount);

            if (currentMoneyAmount == 0)
            {
                GameManager.Instance.HandleGameOver(false);
            }
            
            CheckFinancialStatus();
            SetSliderValue(currentMoneyAmount);
        }

        private void CheckFinancialStatus()
        {
            FinanceState lastState = financeState;
            foreach (FinanceSettings financeSetting in financeSettings)
            {
                if (currentMoneyAmount > financeSetting.minAmountToHave)
                {
                    financeState = financeSetting.financeState;
                    financeStatusText.text = financeSetting.statusName;
                }
                else
                {
                    break;
                }
            }

            if (lastState != financeState)
            {
                OnFinancialStatusChanged?.Invoke(financeState);
            }
        }

        private void SetSliderValue(int currentAmount)
        {
            DOTween.To(() => lastMoneyAmount, x => lastMoneyAmount = x, currentAmount, sliderSmootherValue).OnUpdate(() =>
            {
                financeStatusSlider.value = lastMoneyAmount;
                moneyText.text = "<sprite=0> " + lastMoneyAmount;
            });
        }
    }

    [System.Serializable]
    public class FinanceSettings
    {
        public FinanceState financeState;
        public int minAmountToHave;
        public string statusName;
    }
}