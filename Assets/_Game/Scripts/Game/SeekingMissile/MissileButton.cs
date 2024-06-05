using LightItUp.UI;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class MissileButton : ButtonBase
    {
        [SerializeField] private MissileController missileController;
        [SerializeField] private MissileConfiguration missileConfiguration;

        private void Start()
        {
            if (missileConfiguration.IsActive)
                return;

            gameObject.SetActive(false);
        }
        
        public override void OnClick()
        {
            missileController.Fire();
        }
    }
}
