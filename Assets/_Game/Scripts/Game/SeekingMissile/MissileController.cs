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

            var missile = Instantiate(missilePrefab, player.transform.position, player.transform.rotation);
        }
    }
}
