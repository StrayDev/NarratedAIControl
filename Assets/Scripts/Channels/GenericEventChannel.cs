using System;
using UnityEngine;

public abstract class GenericEventChannel<T> : ScriptableObject
{
    public event Action<T> Callback = delegate(T value) {  };
    public void Invoke(T value) => Callback(value);
}
