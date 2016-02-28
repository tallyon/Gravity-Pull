using UnityEngine;

namespace Assets.Scripts
{
    public class LevelTimer
    {
        // Singleton
        private static LevelTimer instance;
        public static LevelTimer Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelTimer();
                return instance;
            }
        }

        public long MSElapsed { get; set; }

        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Create new LevelTimer object.
        /// </summary>
        private LevelTimer()
        {
            // Subscribe to the event that is called every frame in UITimerController.Update() function due to not inheriting from MonoBehaviour
            UITimerController.UpdateThis += Update;
            // Subscribe to the GameOver event in PlayerController and stop the timer when it fires
            PlayerController.CallOnGameOver += OnGameOver;
        }

        /// <summary>
        /// Gets called in and Update() function in another script that inherits from MonoBehaviour
        /// </summary>
        private void Update()
        {
            if (IsRunning)
                MSElapsed += (long)(Time.deltaTime * 1000);
        }

        /// <summary>
        /// Starts the timer without resetting the time passed.
        /// </summary>
        public void Start()
        {
            Reset();
            IsRunning = true;
        }

        /// <summary>
        /// Pauses the timer without resetting the time passed.
        /// </summary>
        public void Pause()
        {
            //watch.Stop();
            IsRunning = false;
        }

        /// <summary>
        /// Stops and resets the timer.
        /// </summary>
        public void Reset()
        {
            MSElapsed = 0;
            IsRunning = false;
        }

        /// <summary>
        /// Stops, resets and starts the timer.
        /// </summary>
        public void Restart()
        {
            Reset();
            IsRunning = true;
        }

        /// <summary>
        /// Stops, resets the timer and returns time in milliseconds that elapsed.
        /// </summary>
        /// <returns>Time elapsed (ms)</returns>
        public long Stop()
        {
            IsRunning = false;
            return MSElapsed;
        }

        private void OnGameOver()
        {
            Stop();
        }

        public override string ToString()
        {
            return (MSElapsed / 60000).ToString("D2") + ":" + ((MSElapsed / 1000) % 60).ToString("D2") + ":" + (MSElapsed % 1000).ToString("D3");
        }
    }
}
