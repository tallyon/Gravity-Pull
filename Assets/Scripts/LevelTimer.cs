using System.Diagnostics;

namespace Assets.Scripts
{
    public class LevelTimer
    {
        private Stopwatch watch;

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

        private long timeMSElapsedWhenStopped;
        public long MSElapsed
        {
            get
            {
                if (IsRunning)
                    return watch.ElapsedMilliseconds;
                else
                    return timeMSElapsedWhenStopped;
            }
        }

        public bool isRunning;
        public bool IsRunning
        {
            get { return isRunning; }
            protected set
            {
                isRunning = value;
                if (value == false)
                    timeMSElapsedWhenStopped = watch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Create new LevelTimer object.
        /// </summary>
        private LevelTimer()
        {
            watch = new Stopwatch();
            //Force the timer to be explicitly started so it won't stay stopped when level is restarted
            //Start();
        }

        /// <summary>
        /// Starts the watch without reseting the time passed.
        /// </summary>
        public void Start()
        {
            watch.Start();
            IsRunning = true;

            // Subscribe to the GameOver event in PlayerController and stop timer when it fires
            PlayerController.CallOnGameOver += OnGameOver;
        }

        /// <summary>
        /// Pauses the watch without reseting the time passed.
        /// </summary>
        public void Pause()
        {
            watch.Stop();
            IsRunning = false;
        }

        /// <summary>
        /// Stops and resets the watch.
        /// </summary>
        public void Reset()
        {
            watch.Stop();
            watch.Reset();
            IsRunning = false;
        }

        /// <summary>
        /// Stops, resets and starts the watch.
        /// </summary>
        public void Restart()
        {
            Reset();
            watch.Start();
            IsRunning = true;
        }

        /// <summary>
        /// Stops and resets the watch and returns time in milliseconds that elapsed.
        /// </summary>
        /// <returns>Time elapsed (ms)</returns>
        public long Stop()
        {
            watch.Stop();
            IsRunning = false;
            long timeMSElapsed = watch.ElapsedMilliseconds;
            watch.Reset();
            return timeMSElapsed;
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
