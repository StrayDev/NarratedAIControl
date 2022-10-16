// Systems
using System;
using Otherworld.Combat;

// Unity Engine
using UnityEngine;

// Windows specific library
using UnityEngine.Windows.Speech;

// Otherworld
using Otherworld.Command;
using Otherworld.Events;

namespace Otherworld.Narration
{
    /// <summary>
    /// Narration System uses windows dictation and will implement NLP
    /// </summary>
    
    public class NarrationSystem : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private CommandChannel commandChannel;
        [SerializeField] private StringChannel narrationText;
        
        private DictationRecognizer _dictationRecognizer;

        // Move to Position 
        private void DoThing()
        {   
            // Undo last Command
            commandChannel.Invoke(new UndoCommand());
            
            // Move to position
            commandChannel.Invoke(new MoveToPositionCommand(new Vector3(0, 0, 0)));
            
            /*// Move to cell
            commandChannel.Invoke(new MoveToCellCommand(selected, new Vector2(0, 0, 0)));
            
            // Follow Path
            commandChannel.Invoke(new FollowWaypointsCommand(Waypoints, direction, speed));*/
        }
        
        // Follow Path
        // Begin Combat 
        // Stop moving 
        // Move to Cell 
        
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

            _dictationRecognizer.DictationComplete += OnDictationComplete;
            _dictationRecognizer.DictationError += OnDictationError;
            _dictationRecognizer.DictationHypothesis += OnDictationHypothesis;
            _dictationRecognizer.DictationResult += OnDictationResult;

            _dictationRecognizer.Start();
        }
        
        private void OnDisable()
        {
            if (_dictationRecognizer == null) return;

            _dictationRecognizer.DictationComplete -= OnDictationComplete;
            _dictationRecognizer.DictationError -= OnDictationError;
            _dictationRecognizer.DictationHypothesis -= OnDictationHypothesis;
            _dictationRecognizer.DictationResult -= OnDictationResult;
            
            _dictationRecognizer.Stop();
            _dictationRecognizer.Dispose();
        }
        
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
                    narrationText.Invoke("...");
                    Debug.Log($"Confidence : <color=orange>Rejected</color>");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(confidence), confidence, null);
            }

            Debug.Log($"Result : <color=orange>{text}</color>");
            SetText(text, ".");
            ParseDictationResult(text, confidence);
        }
        
        private void OnDictationError(string error, int hresult)
        {
            Debug.LogError(error, this);
        }

        private void OnDictationComplete(DictationCompletionCause cause)
        {
            switch (cause)
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
            if (stopped) _dictationRecognizer.Start();
        }

        private void OnDictationHypothesis(string text)
        {
            SetText(text, "...");
        }
        
        private void OnStatusChanged(SpeechSystemStatus status)
        {
            Debug.Log($"{GetType()} : SystemOnOnStatusChanged() => </color=orange>status = {status}</color>", this);
        }

        private void SystemOnOnError(SpeechError errorcode)
        {
            Debug.LogError($"{GetType()} : SystemOnOnError() => </color=orange>error = {errorcode}</color>", this);
        }
        
        private void ParseDictationResult(string text, ConfidenceLevel confidence)
        {
            switch (text)
            {
                case "roll initiative":
                    commandChannel.Invoke(new StartCombatCommand());
                    break;
                
                case "skip turn":
                    commandChannel.Invoke(new StartCombatCommand());
                    break;
                
                case "your party leave the forest":
                    commandChannel.Invoke(new LeaveTheForestCommand());
                    break;
                
                case "your party follow the path":
                    commandChannel.Invoke(new TravelPathCommand());
                    break;
                
                case "stop":
                    //MoveEvent.Invoke(Vector2.zero);
                    narrationText.Invoke("Moving to the left.");
                    break;

                case "face left":
                case "face west":
                case "face W":
                    //MoveEvent.Invoke(Vector2.left * .005f);
                    narrationText.Invoke("Facing to the left.");
                    break;

                case "face right":
                case "face east":
                case "face E":
                    //MoveEvent.Invoke(Vector2.right * .005f);
                    narrationText.Invoke("Facing to the right.");
                    break;

                case "walk left":
                case "walk west":
                case "walk W":
                    //MoveEvent.Invoke(Vector2.left * .5f);
                    narrationText.Invoke("Moving to the left.");
                    break;

                case "walk right":
                case "walk east":
                case "walk E":
                    //MoveEvent.Invoke(Vector2.right * .5f);
                    narrationText.Invoke("Moving to the right.");
                    break;

                case "run left":
                case "run west":
                case "run W":
                    //MoveEvent.Invoke(Vector2.left);
                    narrationText.Invoke("Moving to the left.");
                    break;

                case "run right":
                case "run east":
                case "run E":
                    //MoveEvent.Invoke(Vector2.right);
                    narrationText.Invoke("Moving to the right.");
                    break;
            }
        }

        private void SetText(string text, string ending = "")
        {
            narrationText.Invoke(UppercaseFirst(text) + ending);
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
}