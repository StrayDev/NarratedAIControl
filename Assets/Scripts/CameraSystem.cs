using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    //[SerializeField] private TransformChannel transformChannel;
    [SerializeField] private CharacterList party;
    
    [SerializeField] private Transform camera;
    //[SerializeField] private Transform target;
    
    private void Start()
    {
        camera.parent = party[0];
        
        var c = camera.localPosition;
        camera.localPosition = new Vector3(0, c.y, c.z);
    }
}
