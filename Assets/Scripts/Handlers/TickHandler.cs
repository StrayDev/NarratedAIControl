using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class manages business rules for the game tick 
    /// </summary>
    
    public class TickHandler : MonoBehaviour
    { 
        [SerializeField] private TickChannel tickChannel;
        [SerializeField] private TickData tickData; 
        
        // data about tick rate??
        private const float MaxTickTime = 0.2f;
        private float _timer = 0;
        
        private void Update()
        {
            // increment tick time
            _timer += Time.deltaTime * tickData.Rate;

            // increase tick & reset timer
            if (_timer < MaxTickTime) return;
            _timer -= MaxTickTime;
            tickData.Count++;
            
            // invoke tick
            tickChannel.Tick();

            // invoke on major tick
            if (tickData.Count % 5 != 0) return;
            tickChannel.TickMajor();
        }
    }
    
}
