using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public TextMeshProUGUI reasonText;
    public TextMeshProUGUI timerText;

    void Start(){
        reasonText.text = PlayerPrefs.GetString("game_over_reason");
        timerText.text = PlayerPrefs.GetString("timer");
    }
    public void RestartGame(){
        SceneManager.LoadScene("Car Selection", LoadSceneMode.Single);
    }
}
