using UnityEngine;

namespace _Game.Scripts.Game.SeekingMissile
{
    [CreateAssetMenu(fileName = "MissileConfiguration", menuName = "[HyperCasual]/MissileConfiguration")]
    public class MissileConfiguration : ScriptableObject
    {
        public bool IsActive;
        public int MissileCount;
    }
}
