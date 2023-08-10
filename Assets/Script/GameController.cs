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
        UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
        var host = debug ? "http://localhost:3000" : "https://firebase.com/";
        StartCoroutine(GetRequest(host + "/obstacles.lua"));
    }

    // Start is called before the first frame update
    void Start()
    {
        stopwatch.Start();
        timeTracker.text = "Timer: 00:00:00";
    }


    private static int Mul(int a, int b)
    {
        return a * b;
    }

    IEnumerator GetRequest(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                Script script = new();
                script.Globals["Mul"] = (Func<int, int, int>)Mul;
                script.DoString(webRequest.downloadHandler.text);
                DynValue luaHelloWorldFunction = script.Globals.Get("helloWorld");
                DynValue res = script.Call(luaHelloWorldFunction);
                Debug.Log(res.Number);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                break;
        }
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
