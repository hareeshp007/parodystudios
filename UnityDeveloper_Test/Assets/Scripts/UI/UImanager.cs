
using GravityGuy.essential;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GravityGuy.UI
{
    public class UImanager : MonoBehaviour
    {


        public GameObject GameOver;
        public GameObject GameWon;
        public GameObject InGame;
        public GameObject GameStart;

        [Header("Score board")]
        public float totalTime = 120.0f;
        public TextMeshProUGUI countdownText;
        public TextMeshProUGUI ScoreText;
        [SerializeField]
        private int currentScore;
        [SerializeField]
        private int MaxScore;
        [SerializeField]
        private float currentTime;

        public void UnPause()
        {
            Time.timeScale = 1.0f;
        }
        public void Pause()
        {
            Time.timeScale = 0f;
        }


        void Start()
        {
            Init();
            UpdateTimerDisplay();
            InvokeRepeating(nameof(UpdateTimer), 1.0f, 1.0f);
            
        }

        private void Init()
        {
            currentScore = 0;
            ScoreText.text = "Score : " + currentScore.ToString();
            currentTime = totalTime;
            GameWon.SetActive(false);
            GameStart.SetActive(true);
            GameOver.SetActive(false);
            InGame.SetActive(false);
            
            Pause();
        }

        private void UpdateTimer()
        {
            if (currentTime > 0)
            {
                currentTime -= 1.0f;
                UpdateTimerDisplay();
            }
            else
            {
                Debug.Log("Timer expired!");
                CancelInvoke(nameof(UpdateTimer));
                GameOverUI();
            }
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(currentTime / 60.0f);
            int seconds = Mathf.FloorToInt(currentTime % 60.0f);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        public void Play()
        {
            GameStart.SetActive(false);
            InGame.SetActive(true);
            UnPause();
        }
        public void GameOverUI()
        {
            GameOver.SetActive(true);
            InGame.SetActive(false);
            Pause();
        }
        public void GameWonUI()
        {
            GameWon.SetActive(true);
            InGame.SetActive(false);
            Pause();
        }

        public void Collect()
        {
            currentScore++;
            ScoreText.text="Score : " + currentScore.ToString();
            if(currentScore>=MaxScore)
            {
                GameWonUI();
            }
        }
        public void Restart()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }

}