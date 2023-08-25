using System;
using UnityEngine;

namespace RRC.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameState gameState;

        public static event Action<GameState> OnGameStateUpdated;

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
    }
}