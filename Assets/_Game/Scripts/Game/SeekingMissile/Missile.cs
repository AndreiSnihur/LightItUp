using System.Linq;
using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private Collider2D collider;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private LayerMask obstacleMask;
        
        private Transform target;
        private BlockController[] blocks;

        public float speed = 5f;
        public float rotationSpeed = 200f;
        public float obstacleAvoidanceDistance = 2f;

        private void Awake()
        {
            blocks = FindObjectsOfType<BlockController>();
            GameManager.Instance.currentLevel.player.camFocus.AddTempTarget(collider, 500);
        }

        private void Update()
        {
            target = FindNearestBlock();
            if (target != null)
            {
                var direction = (target.position - transform.position).normalized;
                var steering = AvoidObstacles(direction);
                var angle = Mathf.Atan2(steering.y, steering.x) * Mathf.Rad2Deg - 90f; 
                var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
                rigidbody.MoveRotation(Quaternion.RotateTowards(
                    transform.rotation, 
                    targetRotation, 
                    rotationSpeed * Time.deltaTime));
                
                rigidbody.velocity = transform.up * speed;
            }
        }

        public void Release()
        {
            Debug.Log($"Release missile");
        }
        
        private Vector2 AvoidObstacles(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, 
                transform.up, 
                obstacleAvoidanceDistance);
            if (hit.collider != null && hit.transform.name != target.name)
            {
                Vector2 avoidance = Vector2.Perpendicular(hit.normal) * speed;
                return (direction + avoidance).normalized;
            }
            return direction;
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

        private Transform FindNearestBlock()
        {
            var unlitBlocks = blocks.Where(b => b.IsLit == false).ToList();
            
            return unlitBlocks.Count == 0 
                ? null 
                : blocks
                    .OrderBy(block => Vector2.Distance(transform.position, block.transform.position))
                    .First().transform;
        }
    }
}