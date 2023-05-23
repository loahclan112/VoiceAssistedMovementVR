using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecognitionResult
{
    public const string AlternativesKey = "alternatives";
    public const string ResultKey = "result";
    public const string PartialKey = "partial";

    public RecognizedPhrase[] Phrases;
    public bool Partial;

    public ResultController resultController;
    public RecognitionResult(string json)
    {
        JSONObject resultJson = JSONNode.Parse(json).AsObject;
        
        if (resultJson.HasKey(AlternativesKey))
        {
            var alternatives = resultJson[AlternativesKey].AsArray;
            Phrases = new RecognizedPhrase[alternatives.Count];

            for (int i = 0; i < Phrases.Length; i++)
            {
                Phrases[i] = new RecognizedPhrase(alternatives[i].AsObject);
            }

        }

        else if(resultJson.HasKey(ResultKey))
        {
            Phrases = new RecognizedPhrase[] { new RecognizedPhrase(resultJson.AsObject) };
        }
        
        else if (resultJson.HasKey(PartialKey))
        {
            Partial = true;
            Phrases = new RecognizedPhrase[] { new RecognizedPhrase() { Text = resultJson[PartialKey] } };
        }
        else
        {
            Phrases = new[] { new RecognizedPhrase() { } };
        }

        resultController = GameObject.Find(ResultController.controllerName).GetComponent<ResultController>();

        List<string> textList = Phrases.Select(x => x.Text).ToList();
        Debug.LogWarning("Sima:"+String.Join(" ", textList));

        List<string> textListDistinct = textList.Distinct().ToList();
        Debug.LogWarning("Dist:"+String.Join(" ", textListDistinct));

        int j = textListDistinct.Count;
        while (j>2)
        {
            textListDistinct.RemoveAt(j - 1);
            j--;
        }


        resultController.DoAction(String.Join(" ", textListDistinct));
    }
}

public class RecognizedPhrase
{
    public const string ConfidenceKey = "confidence";
    public const string TextKey = "text";

    public string Text = "";
    public float Confidence = 0.0f;


    public RecognizedPhrase()
    {
    }

    public RecognizedPhrase(JSONObject json)
    {
        if (json.HasKey(ConfidenceKey))
        {
            Confidence = json[ConfidenceKey].AsFloat;
        }

        if (json.HasKey(TextKey))
        {
            //Vosk adds an extra space at the start of the string.
            Text = json[TextKey].Value.Trim();
            Debug.LogWarning(Text);
        }
    }
}