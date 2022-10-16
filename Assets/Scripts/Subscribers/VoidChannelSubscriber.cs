using UnityEngine;
using UnityEngine.Events;

public class VoidChannelSubscriber : MonoBehaviour
{
    [SerializeField] private VoidChannel channel;
    [SerializeField] private UnityEvent onReceiveValue;

    private void OnEnable() => channel.Callback += OnCallback;
    private void OnDisable() => channel.Callback -= OnCallback;

    private void OnCallback() => onReceiveValue.Invoke();
}
