using System;

namespace Supertactic.GameEvents
{
    public class WeaponEvents
    {
        public event Action onBowCollected;
        public void BowCollected()
        {
            if (onBowCollected != null)
            {
                onBowCollected();
            }
        }

        public event Action<int> onBowGained;
        public void BowGained(int bow)
        {
            if (onBowGained != null)
            {
                onBowGained(bow);
            }
        }
    }
}
