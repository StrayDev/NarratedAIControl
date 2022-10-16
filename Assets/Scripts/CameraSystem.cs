using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    //[SerializeField] private TransformChannel transformChannel;
    [SerializeField] private CharacterList party;
    
    [SerializeField] private Transform mainCamera;
    //[SerializeField] private Transform target;
    
    private void Start()
    {
        mainCamera.parent = party[0];
        
        var c = mainCamera.localPosition;
        mainCamera.localPosition = new Vector3(0, c.y, c.z);
    }
}
