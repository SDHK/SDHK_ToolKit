namespace SDHK
{
    public static class TimerExtension
    {
        public static AsyncTask AsyncYield(this Entity self, int count = 0)
        {
            AsyncTask asyncTask = self.AddChildren<AsyncTask>();
            var counter = asyncTask.AddComponent<CounterCall>();
            counter.countOut = count;
            counter.callback = asyncTask.SetResult;
            return asyncTask;
        }

        public static AsyncTask AsyncDelay(this Entity self, float time)
        {
            AsyncTask asyncTask = self.AddChildren<AsyncTask>();
            var timer = asyncTask.AddComponent<TimerCall>();
            timer.timeOutTime = time;
            timer.callback = asyncTask.SetResult;
            return asyncTask;
        }
    }
}
