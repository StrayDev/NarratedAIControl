// Unity Engine
using UnityEngine;
using UnityEngine.SceneManagement;

// Otherworld
namespace Otherworld.Core
{
    /// <summary>
    /// This List will take the name of a Scene and try to populate
    /// itself with type T components found on the root GameObjects  
    /// </summary>
    
    public abstract class SceneRootRuntimeList<T> : GenericRuntimeList<T>
    {
        [SerializeField] private string[] sceneNames = default;
        
        private void OnEnable()
        {
            foreach (var value in sceneNames)
            {
                var scene = SceneManager.GetSceneByName(value);

                if (!scene.isLoaded) return;

                foreach (var obj in scene.GetRootGameObjects())
                {
                    if (!obj.TryGetComponent<T>(out var component)) continue;
                    _list.Add(component);
                }
            }
        }
    }
}