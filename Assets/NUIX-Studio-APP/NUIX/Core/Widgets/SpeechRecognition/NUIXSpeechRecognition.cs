using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Threading;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

[System.Serializable]
public class MyIntEvent : UnityEvent<string>
{
}

public class NUIXSpeechRecognition :  MonoBehaviour
{
    public WordAction[] trigger_words;
    public AudioSource _audioSource;
    public Screen_Render _screen;
    public ToolTip _toolTip, _toolTip2;

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
            Debug.Log("lword: "+lword);
            if (lword.EndsWith("beats") || lword.EndsWith("beat") || lword.EndsWith("bits") || lword.EndsWith("bit") || lword.EndsWith("bids") || lword.EndsWith("bid"))
            {
                char[] seps = new char [] { ' ' };
                foreach (var number in lword.Split(seps, StringSplitOptions.RemoveEmptyEntries))
                {
                    Debug.Log("lword in: " + number);
                    if (number.Length > 0)
                    {
                        int n = 0;
                        if (int.TryParse(number, out n))
                        {
                            if (n >= 30 && n <= 200)
                            {
                                if (_audioSource.enabled)
                                {
                                    _audioSource.pitch = n / 132.0f;
                                    _toolTip.ToolTipText = n.ToString() + " beats";
                                }
                            }
                        }
                    }
                    break;
                }
            }
            if (lword.StartsWith("channel") || lword.StartsWith("channel"))
            {
                char[] seps = new char[] { ' ' };
                int ith = -1;
                foreach (var number in lword.Split(seps, StringSplitOptions.RemoveEmptyEntries))
                {
                    ith = ith + 1;
                    if (ith == 0) continue;
                    Debug.Log("lword in: " + number);
                    if (number.Length > 0)
                    {
                        int n = 0;
                        if (int.TryParse(number, out n))
                        {
                            if (_screen.enabled) {
                                _screen.ToChannel(n);
                                _toolTip2.ToolTipText = n.ToString();
                            }
                        }
                    }
                    break;
                }
            }
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
