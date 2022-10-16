using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Windows specific library
using UnityEngine.Windows.Speech;

public class PhraseParser : MonoBehaviour
{
    // references
    [SerializeField] private TMP_Text _tmpText;
    
    // variables
    [SerializeField] private int pauseTimeout = 3;
    
    private DictationRecognizer _dictationRecognizer;

    // Setup / Clean-up
    private void OnEnable()
    {
        // check for system support 
        if (!PhraseRecognitionSystem.isSupported)
        {
            Debug.LogError($"{GetType()} : System is not supported", this);
            return;
        }
        
        PhraseRecognitionSystem.OnError += SystemOnOnError;
        PhraseRecognitionSystem.OnStatusChanged += OnStatusChanged;

        // create a dictation recogniser to get mic input
        _dictationRecognizer = new DictationRecognizer(); // good to go
        
        _dictationRecognizer.DictationComplete   += OnDictationComplete;
        _dictationRecognizer.DictationError      += OnDictationError;
        _dictationRecognizer.DictationHypothesis += OnDictationHypothesis;
        _dictationRecognizer.DictationResult     += OnDictationResult;
        
        _dictationRecognizer.Start();
        
        // focus / de-focus management
        Application.focusChanged += ApplicationOnfocusChanged;
    }

    private void ApplicationOnfocusChanged(bool obj)
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Stopped) return;
        
        _dictationRecognizer.Start();
    }


    private void OnDisable()
    {
        if (!PhraseRecognitionSystem.isSupported) return;
        
        PhraseRecognitionSystem.OnError -= SystemOnOnError;
        PhraseRecognitionSystem.OnStatusChanged -= OnStatusChanged;
        
        _dictationRecognizer.DictationComplete   -= OnDictationComplete;
        _dictationRecognizer.DictationError      -= OnDictationError;
        _dictationRecognizer.DictationHypothesis -= OnDictationHypothesis;
        _dictationRecognizer.DictationResult     -= OnDictationResult;
        
        Application.focusChanged -= ApplicationOnfocusChanged;
    }
    
    // DictationRecogniser Callbacks 
    
    private void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        switch (confidence)
        {
            case ConfidenceLevel.High:
                Debug.Log($"Confidence : <color=orange>High</color>");
                break;
            case ConfidenceLevel.Medium:
                Debug.Log($"Confidence : <color=orange>Medium</color>");
                break;
            case ConfidenceLevel.Low:
                Debug.Log($"Confidence : <color=orange>Low</color>");
                // should query action here
                break;
            case ConfidenceLevel.Rejected:
                _tmpText.text = "...";
                Debug.Log($"Confidence : <color=orange>Rejected</color>");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(confidence), confidence, null);
        }
        
        Debug.Log($"Result : {text}</color>");
        ParseDictationResult(text, confidence);
    }


    private void OnDictationError(string error, int hresult)
    {
        Debug.LogError(error, this);
    }

    private void OnDictationComplete(DictationCompletionCause cause)
    {
        switch(cause)
        {
            case DictationCompletionCause.Complete:
                Debug.Log("Dictation : <color=green>Complete</color>");
                break;
            case DictationCompletionCause.AudioQualityFailure:
                Debug.Log("Dictation : <color=red>Audio Quality Failure</color>");
                break;
            case DictationCompletionCause.Canceled:
                Debug.Log("Dictation : <color=blue>Canceled</color>");
                break;
            case DictationCompletionCause.TimeoutExceeded:
                Debug.Log("Dictation : <color=yellow>Timed Out</color>");
                break;
            case DictationCompletionCause.PauseLimitExceeded:
                Debug.Log("Dictation : <color=orange>Pause Limit Exceeded</color>");
                break;
            case DictationCompletionCause.NetworkFailure:
                Debug.Log("Dictation : <color=red>Network Failure</color>");
                break;
            case DictationCompletionCause.MicrophoneUnavailable:
                Debug.Log("Dictation : <color=red>Microphone Unavailable</color>");
                break;
            case DictationCompletionCause.UnknownError:
                Debug.Log("Dictation : <color=red>Unknown Error</color>");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cause), cause, null);
        }

        var stopped = _dictationRecognizer.Status == SpeechSystemStatus.Stopped;
        if(stopped) _dictationRecognizer.Start();
    }

    private void OnDictationHypothesis(string text)
    {
        SetText(text);
    }


    // PhraseRecognitionSystem Callbacks

    private void OnStatusChanged(SpeechSystemStatus status)
    {
        Debug.Log($"{GetType()} : SystemOnOnStatusChanged() => </color=orange>status = {status}</color>", this);
    }

    private void SystemOnOnError(SpeechError errorcode)
    {
        Debug.LogError($"{GetType()} : SystemOnOnError() => </color=orange>error = {errorcode}</color>", this);
    }
    
    // Member Functions
    
    private void ParseDictationResult(string text, ConfidenceLevel confidence)
    {
        switch (text)
        {
            case "move left" :
                _tmpText.text = "Moving to the left.";
                _tmpText.color = Color.gray;
            break;
            
            default:
                _tmpText.text = UppercaseFirst(text) + '.';
                break;
        }
    }
    
    private void SetText(string text)
    {
        _tmpText.color = Color.white;
        
        _tmpText.text = text;

        _tmpText.text = UppercaseFirst(text) + "...";
        
    }
    
    private string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        
        var a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        
        return new string(a);
    }
}
