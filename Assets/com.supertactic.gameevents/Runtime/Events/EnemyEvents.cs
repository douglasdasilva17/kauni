using System;

namespace Supertactic.GameEvents
{
    public class EnemyEvents
    {
        public event Action onEnemyDefeated;
        public void EnemyDefeated()
        {
            if (onEnemyDefeated != null)
            {
                onEnemyDefeated();
            }
        }
    }
}
