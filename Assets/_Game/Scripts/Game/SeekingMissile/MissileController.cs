using System;
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
        [SerializeField] private int missileCount;

        private void Awake() => 
            GameManager.Instance.OnCleanScene += DestroyMissiles;

        private void OnDestroy() => 
            GameManager.Instance.OnCleanScene -= DestroyMissiles;

        public void Fire()
        {
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

                Debug.Log($"Target index {targetIndex}");
                
                if (missile != null) 
                    missile.Initialize(blocks[targetIndex]);
            }
        }
        
        private List<BlockController> Blocks(Transform missilePoint)
        {
            return FindObjectsOfType<BlockController>()
                .Where(b => b.IsLit == false)
                .OrderBy(block => Vector2.Distance(missilePoint.position, block.transform.position))
                .ToList();
        }

        private void DestroyMissiles()
        {
            var missiles = FindObjectsOfType<Missile>();
            foreach (var missile in missiles) 
                Destroy(missile);
        }
    }
}
