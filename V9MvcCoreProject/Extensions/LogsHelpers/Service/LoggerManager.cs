using log4net;
using log4net.Config;
using System.Reflection;
using System.Xml;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;

namespace V9MvcCoreProject.Extensions.LogsHelpers.Service;

//Logs Saved would be bin/Debug/net8.0/Logs/
public class LoggerManager : ILoggerManager
{
    private readonly ILog _logger;
    private readonly IConfiguration _config;
    public LoggerManager(IConfiguration config)
    {
        _config = config;
        try
        {
            _logger = LogManager.GetLogger(typeof(LoggerManager));
            XmlDocument log4netConfig = new XmlDocument();

            var configFile = GetConfigFile();
            //log4netConfig.Load(fs);

            var repo = LogManager.CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy));

            XmlConfigurator.Configure(repo, configFile);
        }
        catch (Exception ex)
        {
            _logger.Error("Error", ex);
        }
    }
    // Logging functionality happens here
    public void LogInformation(string message)
    {
        if (Convert.ToBoolean(_config["EncryptionSettings:EnableLog"]))
        {
            _logger.Info(message);
        }
    }

    public void LogError(string message, Exception exception)
    {
        if (Convert.ToBoolean(_config["EncryptionSettings:EnableLog"]))
        {
            _logger.Error(message, exception);
        }
    }

    private static FileInfo GetConfigFile()
    {
        FileInfo configFile = null;

        // Search config file
        var configFileNames = new[] { "LogConfig/log4net.config", "log4net.config" };

        foreach (var configFileName in configFileNames)
        {
            configFile = new FileInfo(configFileName);

            if (configFile.Exists) break;
        }

        // https://stackoverflow.com/questions/26545919/sql-jobs-or-task-scheduler-call-log4net-does-not-write-log-file/34072145
        if (configFile == null || !configFile.Exists)
        {
            var log4NetConfigDirectory = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var log4NetConfigFilePath = Path.Combine(log4NetConfigDirectory, "LogConfig/log4net.config");
            configFile = new FileInfo(log4NetConfigFilePath);
        }

        if (configFile == null || !configFile.Exists) throw new NullReferenceException("Log4net config file not found.");
        return configFile;
    }
}

