using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private static readonly int OutlineActive = Shader.PropertyToID("_OutlineActive");

#if UNITY_EDITOR
    [ContextMenu("Disable Material Outline")]
    public void DisableOutline() => OnSelected(false);
#endif
    
    public void OnSelected(bool value)
    {
        spriteRenderer.material.SetInt(OutlineActive, value ? 1 : 0);
    }
    
}
