using System;
using UnityEngine;

namespace RRC.Player
{
    public class FinanceHandler : MonoBehaviour
    {
        [SerializeField] private int minAmountToBeRich = 20;

        private int currentMoneyAmount = 0;
        private bool isRich = false;
        
        /// <summary>
        /// True if player got rich, false if poor
        /// </summary>
        public event Action<bool> OnFinancialStatusChanged;
        

        public void UpdateMoney(int amount)
        {
            currentMoneyAmount += amount;
            CheckFinancialStatus();
        }

        private void CheckFinancialStatus()
        {
            if (!isRich && currentMoneyAmount >= minAmountToBeRich)
            {
                isRich = true;
                OnFinancialStatusChanged?.Invoke(true);
            }
            else if(isRich && currentMoneyAmount < minAmountToBeRich)
            {
                isRich = false;
                OnFinancialStatusChanged?.Invoke(false);
            }
        }
    }
}