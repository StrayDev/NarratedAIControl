using System;
using UnityEngine;

namespace Otherworld.Core
{
    // For providing input from control scheme / AI
    public abstract class InputProvider : ScriptableObject
    {
        public event Action<Vector2> OnMoveEvent = delegate{  };
        protected Action<Vector2> MoveEvent => OnMoveEvent;
        
        public event Action<Vector3> OnMoveToPositionEvent = delegate{  };
        protected Action<Vector3> MoveToPositionEvent => OnMoveToPositionEvent;
        
        public event Action<Vector3[]> OnNavigateWaypointsEvent = delegate{  };
        protected Action<Vector3[]> NavigateWaypointsEvent => OnNavigateWaypointsEvent;
        
        public event Action<bool> OnSelectEvent = delegate{  };
        protected Action<bool> SelectEvent => OnSelectEvent;
    }
}
