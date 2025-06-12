using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LoadHighScores();

        if (GameManager.Instance.highScoreName != null && GameManager.Instance.highScoreName.Length > 0 && !string.IsNullOrEmpty(GameManager.Instance.highScoreName[0]))
        {
            HighScoreText.text = "Highest Score by: " + GameManager.Instance.highScoreName[0] + ": " + GameManager.Instance.highScore[0];
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        int newScore = m_Points;
        string newName = GameManager.Instance.playerName;

        if (GameManager.Instance.highScore == null || GameManager.Instance.highScore.Length < 5)
        GameManager.Instance.highScore = new int[5];

        if (GameManager.Instance.highScoreName == null || GameManager.Instance.highScoreName.Length < 5)
        GameManager.Instance.highScoreName = new string[5];

        // Insert new score if it's in the top 5
        for (int i = 0; i < GameManager.Instance.highScore.Length; i++)
        {
            if (newScore > GameManager.Instance.highScore[i])
            {
                // Shift lower scores down
                for (int j = GameManager.Instance.highScore.Length - 1; j > i; j--)
                {
                    GameManager.Instance.highScore[j] = GameManager.Instance.highScore[j - 1];
                    GameManager.Instance.highScoreName[j] = GameManager.Instance.highScoreName[j - 1];
                }

                // Insert new score
                GameManager.Instance.highScore[i] = newScore;
                GameManager.Instance.highScoreName[i] = newName;
                break; // done inserting
            }
        }

        // Show top 1 score
        if (GameManager.Instance.highScoreName[0] != null)
        {
            HighScoreText.text = "Highest Score by: " + GameManager.Instance.highScoreName[0] + ": " + GameManager.Instance.highScore[0];
        }

        // Save to file
        GameManager.Instance.SaveHighScores(GameManager.Instance.highScore, GameManager.Instance.highScoreName);

        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
