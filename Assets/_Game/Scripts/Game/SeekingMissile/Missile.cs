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

        private string ignoreLayer = "Ignore Raycast";
        private float speed = 10f;
        private float rotationSpeed = 50000f;
        private float obstacleAvoidanceDistance = 200f; 
        private float[] angles = { 0f, 30f, 45f, 60f, 75f, 90f, -30f, -45f, -60f, -75f, -90f };
        
        private void Awake() => 
            GameManager.Instance.currentLevel.player.camFocus.AddTempTarget(collider, Mathf.Infinity);

        public void Initialize(BlockController block) => 
            target = block.transform;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(ignoreLayer))
                return;
            
            GameManager.Instance.currentLevel.player.camFocus.RemoveTempTarget(collider);
            Destroy(gameObject);
        }

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

        private Vector2 AvoidObstacles(Vector2 direction)
        {
            var bestDistance = float.MaxValue;
            var bestDirection = direction;

            foreach (var angle in angles)
            {
                var rayDirection = Quaternion.Euler(0, 0, angle) * direction;
                var hit = Physics2D.Raycast(transform.position, rayDirection, obstacleAvoidanceDistance);

                if (hit.collider != null && hit.transform.name != target.name)
                {
                    var avoidance = Vector2.Perpendicular(hit.normal) * speed;
                    var newDirection = (direction + avoidance).normalized;
                    var distanceToTarget = Vector2.Distance((Vector2)transform.position + newDirection, target.position);

                    if (distanceToTarget < bestDistance)
                    {
                        bestDistance = distanceToTarget;
                        bestDirection = newDirection;
                    }
                }
                else
                    return rayDirection.normalized;
            }
            
            return bestDirection;
        }
    }
}