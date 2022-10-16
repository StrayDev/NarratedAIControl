using System;
using Otherworld.Core;
using UnityEngine;

// Windows specific library
using UnityEngine.Windows.Speech;

[CreateAssetMenu(fileName = "new VoiceInputReader", menuName = "Game/Voice Input Reader")]
public class VoiceInputReader : InputProvider
{
    // references
    [SerializeField] private StringChannel updateText;
    //[SerializeField] private TMP_Text _tmpText;
    
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
        //if (!Application.isPlaying) return;
        if (_dictationRecognizer.Status != SpeechSystemStatus.Stopped) return;
        
        _dictationRecognizer.Start();
    }


    private void OnDisable()
    {
        if (_dictationRecognizer == null) return;

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
                updateText.Invoke("...");
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
            case "stop" :
                MoveEvent.Invoke(Vector2.zero);
                updateText.Invoke("Moving to the left.");
                break;
            
            case "face left" :
            case "face west" :
            case "face W" :
                MoveEvent.Invoke(Vector2.left * .005f);
                updateText.Invoke("Facing to the left.");
                break;
            
            case "face right" :
            case "face east" :
            case "face E" :
                MoveEvent.Invoke(Vector2.right * .005f);
                updateText.Invoke("Facing to the right.");
                break;
            
            case "walk left" :
            case "walk west" :
            case "walk W" :
                MoveEvent.Invoke(Vector2.left * .5f);
                updateText.Invoke("Moving to the left.");
                break;
            
            case "walk right" :
            case "walk east" :
            case "walk E" :
                MoveEvent.Invoke(Vector2.right * .5f);
                updateText.Invoke("Moving to the right.");
                break;
            
            case "run left" :
            case "run west" :
            case "run W" :
                MoveEvent.Invoke(Vector2.left);
                updateText.Invoke("Moving to the left.");
                break;
            
            case "run right" :
            case "run east" :
            case "run E" :
                MoveEvent.Invoke(Vector2.right);
                updateText.Invoke("Moving to the right.");
                break;
            
            case "roll initiative" :
                // start combat
                break;
            
            default:
                updateText.Invoke(UppercaseFirst(text) + '.');
                break;
        }
    }
    
    private void SetText(string text)
    {
        updateText.Invoke(UppercaseFirst(text) + ". . .");
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
