using System;
using UnityEngine;

namespace RRC.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameState gameState;
        [SerializeField] private GameObject levelSuccessfulDisplay;
        [SerializeField] private GameObject levelFailedDisplay;

        public static event Action<GameState> OnGameStateUpdated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            if (gameState != GameState.NotStarted)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                gameState = GameState.Started;
                OnGameStateUpdated?.Invoke(gameState);
            }
        }

        public void HandleGameOver(bool hasWon)
        {
            if (hasWon)
            {
                gameState = GameState.FinishedW;
                levelSuccessfulDisplay.SetActive(true);
            }
            else
            {
                gameState = GameState.FinishedL;
                levelFailedDisplay.SetActive(true);
            }
            OnGameStateUpdated?.Invoke(gameState);
        }
    }
}