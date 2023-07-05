using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Game
{
    public interface IOptionController
    {
        public void SetOptionData(OptionData data);
        public IOptionsUIController SetOptionsUI { set; }
        public Transform transform { get; }
    }
}