using System.Collections.Generic;
using System.Linq;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class MissileController : MonoBehaviour
    {
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private MissileConfiguration missileConfiguration;
        
        private int missileCount;
        private bool isActive;

        private void Awake()
        {
            isActive = missileConfiguration.IsActive;
            missileCount = missileConfiguration.MissileCount;
        }

        public void DestroyMissiles()
        {
            var missiles = FindObjectsOfType<Missile>();
            foreach (var missile in missiles) 
                Destroy(missile.gameObject);
        }

        public void Fire()
        {
            if (isActive == false)
                return;
            
            var player = FindObjectOfType<PlayerController>();
            var missilePoint = player.MissilePoint.transform;
            var blocks = Blocks(missilePoint);
            
            if (blocks.Count == 0)
                return;
            
            for (var i = 0; i < missileCount; i++)
            {
                var missileGo = Instantiate(missilePrefab, missilePoint.position, missilePoint.rotation);
                missileGo.TryGetComponent(out Missile missile);

                var targetIndex = i;
                if (blocks.Count <= i) 
                    targetIndex = Random.Range(0, blocks.Count);
                
                if (missile != null) 
                    missile.Initialize(blocks[targetIndex]);
            }
        }
        
        private List<BlockController> Blocks(Transform missilePoint) =>
            FindObjectsOfType<BlockController>()
                .Where(b => b.IsLit == false)
                .OrderBy(block => Vector2.Distance(missilePoint.position, block.transform.position))
                .ToList();


    }
}
