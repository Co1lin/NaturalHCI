using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Threading;


[System.Serializable]
public class MyIntEvent : UnityEvent<string>
{
}

public class NUIXSpeechRecognition :  MonoBehaviour
{
    public WordAction[] trigger_words;

    //public UnityEvent<string> unityEvent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnalyzeSpeechRecognized(string result)
    {
        // Example: compare speech result to a set of words

        //foreach (var ch in result)
        //{
        //    foreach (WordAction wordAction in trigger_words)
        //    {
        //        if (wordAction.words.Contains(ch.ToString().ToLower()))
        //        {
        //            wordAction.wordRecognized?.Invoke();
        //            Thread.Sleep(1000);
        //        }
        //    }
        //}

        char[] separators = new char[] { '.' };
        foreach (var word in result.Split(separators, StringSplitOptions.RemoveEmptyEntries))
        {
            string lword = word.ToString().ToLower();
            foreach (WordAction wordAction in trigger_words)
            {
                foreach (var actionword in wordAction.words)
                {
                    Debug.Log("recognized: "+lword+" expected: "+actionword);
                    if (actionword.Contains(lword))
                    {
                        Debug.Log("invoked: " + ( wordAction.wordRecognized != null ).ToString() );
                        wordAction.wordRecognized?.Invoke();
                    }
                }
            }
        }
    }
}
