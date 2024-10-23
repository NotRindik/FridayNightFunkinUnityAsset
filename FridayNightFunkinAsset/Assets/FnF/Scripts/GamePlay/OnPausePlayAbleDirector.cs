using UnityEngine.Playables;


namespace FridayNightFunkin.GamePlay
{
    public class OnPausePlayAbleDirector : OnGameStateChange
    {
        private PlayableDirector mDirector;

        private void Start()
        {
            mDirector = GetComponent<PlayableDirector>();
        }

        protected override void OnGameStateChanged(GameState currenState)
        {
            if (currenState == GameState.Paused)
            {
                mDirector.Pause();
            }
            else
            {
                mDirector.Resume();
            }
        }
    }
}