using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static List<Vector3> All;
    
    void Start()
    {
        if (All == null)
        {
            All = new List<Vector3>();
        }
        
        All.Add(transform.position);
    }

    void OnDisable()
    {
        All.Remove(transform.position);
    }
}
