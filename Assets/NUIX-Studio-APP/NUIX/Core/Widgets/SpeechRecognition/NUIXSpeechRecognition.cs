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
    private bool playing;

    //public UnityEvent<string> unityEvent;
    // Start is called before the first frame update
    void Start()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayAudio(WordAction action, int cnt)
    {
        Thread.Sleep(cnt * 2000);
        action.wordRecognized?.Invoke();
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

        if (playing) return;
        playing = true;
        char[] separators = new char[] { ' ', '.' };
        List<Thread> threads = new List<Thread>();
        int cnt = 0;
        foreach (var word in result.Split(separators, StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (WordAction wordAction in trigger_words)
            {
                foreach (var actionword in wordAction.words)
                {
                    if (word.Contains(actionword))
                    {
                        var thread = new Thread(() => PlayAudio(wordAction, cnt));
                        thread.Start();
                        threads.Add(thread);
                        cnt = cnt + 1;
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < cnt; ++i)
            threads[i].Join();
        playing = false;
    }
}
