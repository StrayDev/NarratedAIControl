using System;
using UnityEngine;


[CreateAssetMenu(fileName = "new VoidChannel", menuName = "Event Channels/void")]
public class VoidChannel : ScriptableObject
{
    public event Action Callback = delegate {  };
    public virtual void Invoke() => Callback();
}
