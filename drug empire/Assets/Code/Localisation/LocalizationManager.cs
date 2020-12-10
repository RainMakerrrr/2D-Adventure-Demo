using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    public TextAsset[] libLang;

    public Dictionary<string, string> localizedText;
    private bool isReady;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        //LoadLocalizatedText(libEN);
        //PlayerPrefs.SetInt("Language", 0);


        Load();
    }

    public void Load()
    {
        LoadLocalizatedText(libLang[PlayerPrefs.GetInt("Language")]);
    }

    public string GetLocalizetedValue(string key)
    {
        string result;
        result = "Error";
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public void LoadLocalizatedText(TextAsset lib)
    {
        localizedText = new Dictionary<string, string>();
        string dataAsJson = lib.ToString();
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        isReady = true;

    }

    public bool GetIsReady()
    {
        return isReady;
    }
}
