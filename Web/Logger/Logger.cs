using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;
using log4net.Config;

namespace Web
{
    public static class Logger
    {
        private static readonly object _locker = new object();
        private static volatile ILog _log;

        public static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    lock (_locker)
                    {
                        if (_log == null)
                        {
                            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                        }
                    }
                }

                return _log;
            }
        }
        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}