using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DMGasPrice.Service.Helpers
{
    public class TimerProcess
    {
        private Timer _timer;

        #region ctor

        public TimerProcess()
        {
            _timer = new Timer(new TimerCallback(TimerProcessCallback));
        }

        #endregion

        #region methods

        public void Start()
        {
            _timer.Change(600000, 600000);
        }

        #endregion

        #region logic

        private void TimerProcessCallback(object state)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            if (currentTime.Hour == 17)
            {
                GasPriceCache.Instance.Refresh();
            }
        }

        #endregion
    }
}