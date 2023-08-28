using UnityEngine;
using UnityEngine.SceneManagement;

namespace RRC.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private GameObject[] levelPrefabs;
        
        private int level = 0;

        private readonly string LEVEL_PREF_KEY = "level__";

        private void Awake()
        {
            GetLevel();
            Instantiate(levelPrefabs[level % levelPrefabs.Length], Vector3.zero, Quaternion.identity);
        }

        public void LoadNextLevel()
        {
            level++;
            SetLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GetLevel()
        {
            level = PlayerPrefs.GetInt(LEVEL_PREF_KEY, 0);
        }

        private void SetLevel()
        {
            PlayerPrefs.SetInt(LEVEL_PREF_KEY, level);
        }
    }
}

