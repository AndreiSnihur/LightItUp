using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class MissileController : MonoBehaviour
    {
        [SerializeField] private GameObject missilePrefab;

        public void Fire()
        {
            var player = FindObjectOfType<PlayerController>();

            var missilePoint = player.MissilePoint.transform;
            var missile = Instantiate(missilePrefab, missilePoint.position, missilePoint.rotation);
        }
    }
}
