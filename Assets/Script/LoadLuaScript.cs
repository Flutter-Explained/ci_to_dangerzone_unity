using System;
using System.Collections;
using System.IO;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadLuaScript : MonoBehaviour
{
    bool debug = false;
    public Script luaScript;
    [SerializeField] private GameObject obstacleSpawner;
    [SerializeField] private TextAsset PublicKeyTextAsset;
    ObstacleSpawner obstacleSpawnerScript;

    // Start is called before the first frame update
    void Awake()
    {
        obstacleSpawnerScript = obstacleSpawner.GetComponent<ObstacleSpawner>();
        UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
        StartCoroutine(GetRequest());
    }

    IEnumerator GetRequest()
    {
        var host = debug ? "http://localhost:3000" : "https://ci-cd-dangerzone-unity.web.app";
        var uriScript = host + "/obstacles.lua";
        var uriSignature = host + "/signature";
        string script = "";
        string signature = "";


        using UnityWebRequest webRequestScript = UnityWebRequest.Get(uriScript);
        using UnityWebRequest webRequestSignature = UnityWebRequest.Get(uriSignature);
        // Request and wait for the desired page.
        yield return webRequestScript.SendWebRequest();
        yield return webRequestSignature.SendWebRequest();

        switch (webRequestScript.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("uriScript: Error: " + webRequestScript.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("uriScript: HTTP Error: " + webRequestScript.error);
                Debug.LogError(uriScript);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("uriScript:\nReceived: " + webRequestScript.downloadHandler.text);
                script = webRequestScript.downloadHandler.text;
                break;
        }

        switch (webRequestSignature.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("uriSignature: Error: " + webRequestSignature.error);
                yield return null;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("uriSignature: HTTP Error: " + webRequestSignature.error);
                yield return null;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("uriSignature:\nReceived: " + webRequestSignature.downloadHandler.text);
                signature = webRequestSignature.downloadHandler.text;
                break;
        }

        string publicKey = ReadString();

        bool verified = VerifyScriptWithSignatureAndPublicKey(signature, publicKey, script);

        if (verified)
        {
            // Load Lua Script
            Debug.Log($"Verified: {verified}");

            Script luaScript = new();
            luaScript.DoString(script);
            luaScript.Globals["mkObstacle"] = (Func<int, int, int>)obstacleSpawnerScript.SpawnObstacles;


            DynValue luaDoObstaclesFunction = luaScript.Globals.Get("doObstacles");
            luaScript.Call(luaDoObstaclesFunction);
        }
        else
        {
            Debug.LogError("The Script is not verified and cannot be loaded!");
        }
    }

    private bool VerifyScriptWithSignatureAndPublicKey(string signature, string publicKeyString, string script)
    {
        var publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKeyString));
        var scriptBytes = Encoding.UTF8.GetBytes(script);
        var signatureBytes = Convert.FromBase64String(signature);

        ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");
        signer.Init(false, publicKey);
        signer.BlockUpdate(scriptBytes, 0, scriptBytes.Length);
        var verified = signer.VerifySignature(signatureBytes);
        return verified;
    }

    string ReadString()
    {
        return PublicKeyTextAsset.text;
    }
}
