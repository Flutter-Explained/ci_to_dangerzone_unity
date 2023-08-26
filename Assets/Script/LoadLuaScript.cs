using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
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
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequestScript.error);
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
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + webRequestSignature.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(pages[page] + ":\nReceived: " + webRequestSignature.downloadHandler.text);
                signature = webRequestSignature.downloadHandler.text;
                break;
        }
        
        string publicKey = ReadString();

        VerifyScriptWithSignatureAndPublicKey(signature, publicKey, script);

    }

    private void VerifyScriptWithSignatureAndPublicKey(string signature, string publicKey, string script)
    {
        var tPublicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
        var scriptBytes = Encoding.UTF8.GetBytes(script);
        var signatureBytes =  Convert.FromBase64String(signature);
        ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");
        signer.Init(false, tPublicKey);
        signer.BlockUpdate(scriptBytes, 0, scriptBytes.Length);
        var verified = signer.VerifySignature(signatureBytes);
        Debug.Log($"Verified: {verified}");
        Debug.Log(script);
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
