using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    public class Missile : MonoBehaviour
    {
        public void Release()
        {
            Debug.Log($"Release missile");
            Destroy(gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Missile collided with {collision.gameObject.name}");
        }
    }
}