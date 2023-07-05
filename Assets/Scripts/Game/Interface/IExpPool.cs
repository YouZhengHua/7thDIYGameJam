using UnityEngine;

namespace Scripts.Game
{
    public interface IExpPool
    {
        public GameObject[] GetExpPrefabs(int exp);
    }
}