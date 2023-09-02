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

public class LoadLuaScript : MonoBehaviour
{
    bool debug = true;
    // Start is called before the first frame update
    void Awake()
    {
        UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
        StartCoroutine(GetRequest());
    }

    private static int Mul(int a, int b)
    {
        return a * b;
    }

    IEnumerator GetRequest()
    {
        var host = debug ? "http://localhost:3000" : "https://firebase.com/";
        var uriScript = host + "/obstacles.lua";
        var uriSignature = host + "/signature";
        string script = "";
        string signature = "";

        using UnityWebRequest webRequestScript = UnityWebRequest.Get(uriScript);
        using UnityWebRequest webRequestSignature = UnityWebRequest.Get(uriSignature);
        // Request and wait for the desired page.
        yield return webRequestScript.SendWebRequest();
        yield return webRequestSignature.SendWebRequest();

        string[] pages = uriScript.Split('/');
        int page = pages.Length - 1;

        switch (webRequestScript.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequestScript.error);
                yield return null;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequestScript.error);
                yield return null;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + webRequestScript.downloadHandler.text);
                script = webRequestScript.downloadHandler.text;
                break;
        }

        switch (webRequestSignature.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + webRequestSignature.error);
                yield return null;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequestSignature.error);
                yield return null;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + webRequestSignature.downloadHandler.text);
                signature = webRequestSignature.downloadHandler.text;
                break;
        }

        string publicKey = ReadString();

        bool verified = VerifyScriptWithSignatureAndPublicKey(signature, publicKey, script);

        if (verified)
        {
            // Load Lua Script
            Debug.Log($"Verified: {verified}");
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
        string path = "Assets/Resources/public_key";
        StreamReader reader = new(path);
        string result = reader.ReadToEnd();
        reader.Close();
        return result;
    }
}
