using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class StartMenuUIHandler : MonoBehaviour
{
    public TMP_InputField playerInput;
    public TMP_Text HighScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadHighScore();
    }

    // Update is called once per frame
    public void StartNew()
    {
        GameManager.Instance.playerName = playerInput.text;
        SceneManager.LoadScene(1);
        Debug.Log("Player Name: " + GameManager.Instance.playerName);
    }
    public void LoadHighScore()
    {
        GameManager.Instance.LoadHighScores();
        if (GameManager.Instance.highScoreName != null)
        {
            HighScoreText.text = "Best Scores:\n";
            for (int i = 0; i < GameManager.Instance.highScore.Length; i++)
            {
                HighScoreText.text += $"{i + 1}. {GameManager.Instance.highScoreName[i]}: {GameManager.Instance.highScore[i]}\n";
            }
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif 
    }
}
