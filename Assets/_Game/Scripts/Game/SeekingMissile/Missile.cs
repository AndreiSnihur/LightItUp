using System;
using System.Linq;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private Collider2D collider;
        
        private void Awake()
        {
            GameManager.Instance.currentLevel.player.camFocus.AddTempTarget(collider, 500);

            var nearestBlock = FindNearestBlock();
            if (nearestBlock != null)
                LeanTween.move(gameObject, nearestBlock.transform.position, 3)
                    .setEase(LeanTweenType.easeInOutQuad);
        }

        public void Release()
        {
            Debug.Log($"Release missile");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Missile collided with {collision.gameObject.name}");

            if (collision.gameObject.TryGetComponent(out BlockController blockController) &&
                blockController.IsLit == false)
            {
                GameManager.Instance.currentLevel.player.camFocus.RemoveTempTarget(collider);
                Destroy(gameObject);
            }
        }

        private BlockController FindNearestBlock()
        {
            var blocks = FindObjectsOfType<BlockController>()
                .Where(b => b.IsLit ==false)
                .ToList();
            
            if (blocks.Count == 0)
                return null;
            
            var minDistance = Mathf.Infinity;
            BlockController nearestBlock = null;
            foreach (var block in blocks)
            {
                var distance = Vector3.Distance(transform.position, block.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBlock = block;
                }
            }

            return nearestBlock;
        }
    }
}