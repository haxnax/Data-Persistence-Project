using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public string playerName;

    public Text ScoreText;
    public Text topScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string savePath;
    [SerializeField] private int highScore;
    [SerializeField] public string TopPlayer;
    public HighScoreData highScoreData = new HighScoreData();



    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + "/highscore.json";

        LoadHighScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            TrySetHighScore();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{Menu.userText} Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

[System.Serializable]
    public class HighScoreData
    {
        public int highScore;
        public string topPlayer;
    }

    public void TrySetHighScore()
    {
        if (m_Points > highScoreData.highScore)
        {
            highScoreData.highScore = m_Points;
            highScoreData.topPlayer = Menu.userText;
            topScoreText.text = $" High Score: {highScoreData.topPlayer} : {highScoreData.highScore}";

            SaveHighScore();
        }
    }
    private void SaveHighScore()
    {
        string json = JsonUtility.ToJson(highScoreData);
        File.WriteAllText(savePath, json);
    }


    private void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            highScoreData= JsonUtility.FromJson<HighScoreData>(json);
            topScoreText.text =  $"Loaded High Score: {highScoreData.topPlayer} : {highScoreData.highScore}";

        }
        else
        {
            Debug.Log("No high score saved yet.");
        }
    }
}
