using System;

namespace Supertactic.GameEvents
{
    public class MiscEvents
    {
        public event Action onCoinCollected;
        public void CoinCollected()
        {
            if (onCoinCollected != null)
            {
                onCoinCollected();
            }
        }

        public event Action onGemCollected;
        public void GemCollected()
        {
            if (onGemCollected != null)
            {
                onGemCollected();
            }
        }

        public event Action onGameCompleted;
        public void GameCompleted()
        {
            if (onGameCompleted != null)
            {
                onGameCompleted();
            }
        }
    }
}