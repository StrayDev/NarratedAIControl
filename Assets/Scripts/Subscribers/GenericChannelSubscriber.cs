using UnityEngine;
using UnityEngine.Events;

public class GenericChannelSubscriber<T> : MonoBehaviour
{
    [SerializeField] private GenericEventChannel<T> channel;
    [SerializeField] private UnityEvent<T> onReceiveValue;

    private void OnEnable() => channel.Callback += OnCallback;
    private void OnDisable() => channel.Callback -= OnCallback;

    private void OnCallback(T value) => onReceiveValue.Invoke(value);
}
