using LightItUp.Data;
using LightItUp.Game;
using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private Collider2D collider;
        [SerializeField] private Rigidbody2D rigidbody;
        
        private Transform target;
        
        private float speed = 10f;
        private float rotationSpeed = 50000f;
        private float obstacleAvoidanceDistance = 200f; 
        
        private void Awake() => 
            GameManager.Instance.currentLevel.player.camFocus.AddTempTarget(collider, 500);

        private void Update()
        {
            if (target is null)
                return;
            
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
        
        public void Initialize(BlockController block)
        {
            Debug.Log($"Initialize {block.gameObject.name}");
            target = block.transform;
        }

        public void Release()
        {
            Debug.Log($"Release missile");
        }
        
        private Vector2 AvoidObstacles(Vector2 direction)
        {
            float[] angles = { 0f, 30f, 45f, 60f, 75f, 90f, -30f, -45f, -60f, -75f, -90f };
   
            float bestDistance = float.MaxValue;
            Vector2 bestDirection = direction;

            foreach (float angle in angles)
            {
                Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, obstacleAvoidanceDistance);

                if (hit.collider != null && hit.transform.name != target.name)
                {
                    Vector2 avoidance = Vector2.Perpendicular(hit.normal) * speed;
                    Vector2 newDirection = (direction + avoidance).normalized;
                    float distanceToTarget = Vector2.Distance((Vector2)transform.position + newDirection, target.position);

                    if (distanceToTarget < bestDistance)
                    {
                        bestDistance = distanceToTarget;
                        bestDirection = newDirection;
                    }
                }
                else
                {
                    return rayDirection.normalized;
                }
            }

            Debug.Log($"Should avoid {bestDirection}");
            return bestDirection;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Missile collided with {collision.gameObject.name}");

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
                return;
            
            GameManager.Instance.currentLevel.player.camFocus.RemoveTempTarget(collider);
            Destroy(gameObject);
        }
    }
}