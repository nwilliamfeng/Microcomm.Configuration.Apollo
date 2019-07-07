using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Enums;
using Com.Ctrip.Framework.Apollo.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microcomm.Configuration.Apollo
{
    public class ConfigUtil : IApolloOptions
    {
        public static NameValueCollection AppSettings { get; set; }
        private static Func<HttpMessageHandler> _httpMessageHandlerFactory;

        //private static readonly Func<Action<LogLevel, string, Exception>> Logger = () => LogManager.CreateLogger(typeof(ConfigUtil));
        private int _refreshInterval = 5 * 60 * 1000; //5 minutes
        private int _timeout = 5000; //5 seconds, c# has no connectTimeout but response timeout

        public ConfigUtil()
        {
            if (AppSettings == null) AppSettings = ConfigurationManager.AppSettings;

            InitRefreshInterval();
            InitTimeout();
            InitCluster();
        }

       
        public static string GetAppConfig(string key)
        {
            var key1 = "Apollo." + key;
            var key2 = "Apollo:" + key;

            var value = AppSettings[key1];
            if (string.IsNullOrEmpty(value))
                value = AppSettings[key2];

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(key1);

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(key2);

            return string.IsNullOrEmpty(value) ? null : value;
        }

        
        public string AppId
        {
            get
            {
                var appId = GetAppConfig("AppId");
                if (string.IsNullOrWhiteSpace(appId))
                {
                    appId = ConfigConsts.NoAppidPlaceholder;
                  //  Logger().Warn("Apollo.AppId is not set, apollo will only load public namespace configurations!");
                }

                return appId;
            }
        }

       
        public string DataCenter => GetAppConfig("DataCenter");

        private void InitCluster()
        {
            //Load data center from app.config
            Cluster = GetAppConfig("Cluster");

            //Use data center as cluster
            if (string.IsNullOrWhiteSpace(Cluster))
                Cluster = DataCenter;

            //Use default cluster
            if (string.IsNullOrWhiteSpace(Cluster))
                Cluster = ConfigConsts.ClusterNameDefault;
        }

       
        public string Cluster { get; private set; }

        /// <summary>
        /// Get the current environment.
        /// </summary>
        /// <returns> the env </returns>
        public Env Env => Enum.TryParse(GetAppConfig("Env"), true, out Env env) ? env : Env.Dev;

        public string LocalIp { get; set; } = NetworkInterfaceManager.HostIp;

        public string MetaServer => GetAppConfig("MetaServer");

        public IReadOnlyCollection<string> ConfigServer => GetAppConfig("ConfigServer")?.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

        private void InitTimeout()
        {
            var customizedTimeout = GetAppConfig("Timeout");

            if (int.TryParse(customizedTimeout, out _timeout)) return;

            _timeout = 5000;

           // Logger().Error($"Config for Apollo.Timeout is invalid: {customizedTimeout}");
        }

        public int Timeout => _timeout;

        private static readonly string DefaultAuthorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("user:"));
    //    public string Authorization => GetAppConfig("Authorization") ?? DefaultAuthorization;

        private void InitRefreshInterval()
        {
            var customizedRefreshInterval = GetAppConfig("RefreshInterval");

            if (int.TryParse(GetAppConfig("RefreshInterval"), out _refreshInterval)) return;

            _refreshInterval = 5 * 60 * 1000;

           // Logger().Error($"Config for Apollo.RefreshInterval is invalid: {customizedRefreshInterval}");
        }

        public int RefreshInterval => _refreshInterval;

        public string LocalCacheDir => GetAppConfig("LocalCacheDir") ?? Path.Combine(ConfigConsts.DefaultLocalCacheDir, AppId);

        public Func<HttpMessageHandler> HttpMessageHandlerFactory => _httpMessageHandlerFactory;

      //  public static void UseHttpMessageHandlerFactory(Func<HttpMessageHandler> factory) => Interlocked.CompareExchange(ref _httpMessageHandlerFactory, factory, null);
    }
}
