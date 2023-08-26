using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;
using System.Security.Cryptography;
using TMPro;

public class GameController : MonoBehaviour
{
    public bool gameOver;
    public TextMeshProUGUI timeTracker;
    public TextMeshProUGUI lapsTracker;

    public int laps = -2;
    public bool debug = true;
    readonly int winningLaps = 3;

    private readonly System.Diagnostics.Stopwatch stopwatch = new();

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        stopwatch.Start();
        timeTracker.text = "Timer: 00:00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            PlayerPrefs.SetString("timer", stopwatch.Elapsed.ToString("mm\\:ss\\.ff"));
            stopwatch.Stop();
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
            
        }
        if (laps == winningLaps)
        {
            GameOver("Winning");
        }

        var showLaps = laps < 0 ? 0 : laps;
        lapsTracker.text = $"Laps: {showLaps} / 3";
        timeTracker.text = $"Timer: {stopwatch.Elapsed:mm\\:ss\\.ff}";
    }

    public void GameOver(string reason)
    {
        gameOver = true;
        PlayerPrefs.SetString("game_over_reason", reason);
    }
}
