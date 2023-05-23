using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataLoader : MonoBehaviour
{
    [SerializeField]
    private TextAsset keyPhrasesData = null;


    public static string objectName = "DataLoader";

    public List<string> LoadKeyPhrases()
    {
        if (keyPhrasesData != null)
        {
            string dataAsJson = keyPhrasesData.text;
            List<string> keyPhrases = JsonConvert.DeserializeObject<List<string>>(dataAsJson);
            return keyPhrases;
        }
        return null;
    }
}
