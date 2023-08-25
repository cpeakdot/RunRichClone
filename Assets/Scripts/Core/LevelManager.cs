using UnityEngine;
using UnityEngine.SceneManagement;

namespace RRC.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private GameObject[] levelPrefabs;
        
        private int level = 0;

        private void Awake()
        {
            Instantiate(levelPrefabs[level % levelPrefabs.Length], Vector3.zero, Quaternion.identity);
        }

        public void LoadNextLevel()
        {
            level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

