using UnityEngine;

namespace Scripts.Game
{
    public interface IDropController
    {
        public Transform SetPlayerTransform { set; }
        public IAttributeHandle SetAttributeHandle { set; }
    }
}