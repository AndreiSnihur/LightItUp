using LightItUp.UI;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class MissileButton : ButtonBase
    {
        [SerializeField] private MissileController missileController;
        
        public override void OnClick()
        {
            missileController.Fire();
        }
    }
}
