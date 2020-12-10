using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;

    void Start()
    {
        Text text = GetComponent<Text>();
        if (text != null) text.text = LocalizationManager.instance.GetLocalizetedValue(key);
        else
        {
            TextMesh text1 = GetComponent<TextMesh>();
            text1.text = LocalizationManager.instance.GetLocalizetedValue(key);

        }
    }
}
