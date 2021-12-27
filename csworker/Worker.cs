using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using csprjclib.ApiModels;
using csprjclib.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace csworker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        Api _api = new Api();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        List<MonitoredFile> mfList = new List<MonitoredFile>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            List<Task> monitors = new List<Task>();

            List<CancellationTokenSource> cancelTokens = new List<CancellationTokenSource>();
            

            while (!stoppingToken.IsCancellationRequested)
            {
                
                //_logger.LogInformation("{time}: Worker: Getting list of files ...", DateTimeOffset.Now);

                var newList = _api.GetMonitors();

                //_logger.LogInformation("{time}: Worker: {n} files returned", DateTimeOffset.Now, newList.Count);

                int newCount = 0;

                // check for new items
                foreach (var mf in newList)
                {
                    bool addMF = true;
                    
                    for (int i = 0; i < mfList.Count; i++)
                    {
                        if (mfList[i].Id == mf.Id)
                        {
                            mfList[i] = mf;
                            addMF = false;
                            break;
                        }
                    }
                    if (addMF)
                    {
                        mfList.Add(mf);
                        newCount++;
                        var tokenSource = new CancellationTokenSource();
                        cancelTokens.Add(tokenSource);
                        monitors.Add(MonitorFile(monitors.Count + 1, mf.Id, tokenSource.Token));
                    }
                }

                // check for removed items
                int oldCount = 0;
                List<int> tobeRemoved = new List<int>();
                for (int i = 0; i < mfList.Count; i++)
                {
                    bool found = false;
                    foreach (var mf in newList)
                    {
                        if (mf.Id == mfList[i].Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // cancel the tasks
                        cancelTokens[i].Cancel();
                        tobeRemoved.Add(i);
                        oldCount++;
                        
                    }
                }
                foreach (var item in tobeRemoved)
                {
                    //remove from list
                    mfList.Remove(mfList[item]);
                    cancelTokens.Remove(cancelTokens[item]);
                }

                // await Task.WhenAll(monitors.ToArray());
                _logger.LogInformation("{time}: Worker: Added {n} entries, removed {m} entries, {p} current entries", DateTimeOffset.Now, newCount, oldCount, mfList.Count);

                //_logger.LogInformation("{time}: Worker: Waiting to rety", DateTimeOffset.Now);


                await Task.Delay(1 * 60 * 1_000, stoppingToken);
            }

            if (stoppingToken.IsCancellationRequested)
            {
                foreach (var cT in cancelTokens)
                {
                    cT.Cancel();
                }
            }

        }

        private async Task MonitorFile(int taskID, int mfID, CancellationToken stoppingToken)
        {
            int wait = 5;

            while (!stoppingToken.IsCancellationRequested)
            {
                MonitoredFile monitoredFile = mfList.FirstOrDefault(x => x.Id == mfID);

                if (monitoredFile != null)
                {
                    string fileProp = "";
                    string filePropHash = "";
                    string fileHash = "FILE_DOES_NOT_EXIST";
                    byte[] fileBytes = Encoding.Default.GetBytes("FILE_DOES_NOT_EXIST");

                    bool fileE = false;
                    // Check if file exists
                    if (File.Exists(monitoredFile.Location))
                    {
                        fileE = true;

                        _logger.LogInformation("{time}: Task {taskID}: File: {path} - computing hash...", DateTimeOffset.Now,
                        taskID, monitoredFile.Name);

                        fileHash = HashFile(monitoredFile.Location);

                        

                        if (monitoredFile.MonitorProperties)
                        {
                            var fileP = FileProps(monitoredFile.Location);
                            filePropHash = fileP[0];
                            fileProp = fileP[1];
                        }

                    }
                    else
                    {
                        _logger.LogError("{time}: Task {taskID}: File: {path} does not exist", DateTimeOffset.Now,
                        taskID, monitoredFile.Location, monitoredFile.Delay.Name);
                    }

                    apiMonitoredFileContent model = new apiMonitoredFileContent
                    {
                        Hash = fileHash,
                        Id = monitoredFile.Id,
                        Properties = fileProp,
                        PropertiesHash = filePropHash
                    };

                    var hashCheck = _api.HashCheck(model);

                    if (hashCheck == 2)
                    {
                        _logger.LogInformation("{time}: Task {taskID}: File: {path} - uploading content...", DateTimeOffset.Now,
                            taskID, monitoredFile.Name);
                        // get file contents
                        if (fileE)
                        {
                            fileBytes = File.ReadAllBytes(monitoredFile.Location);
                        }
                        
                        model.Content = fileBytes;
                        model.Param = "H+C";
                        _api.HashCheck(model);
                    }

                    wait = monitoredFile.Delay.DelayInSecs;
                }

                _logger.LogDebug("{time}: Task {taskID}: Waiting {delay} seconds ...", DateTimeOffset.Now,
                        taskID, wait);

                await Task.Delay(wait * 1000, stoppingToken);
            }
        }

        private string HashFile(string file)
        {
            using (var hasher = HashAlgorithm.Create("SHA512"))
            {
                using (var stream = File.OpenRead(file))
                {
                    var hash = hasher.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "");
                }
            }
        }

        private string[] FileProps(string file)
        {
            FileInfo oFileInfo = new FileInfo(file);
            string cT = "Created: " + oFileInfo.CreationTime.ToString();
            string uT = "Modified: " + oFileInfo.LastWriteTime.ToString();

            string propFull = cT + "\n" + uT;

            string propHash = "";
            using (var hasher = HashAlgorithm.Create("SHA512"))
            {
                propHash = BitConverter.ToString(
                    hasher.ComputeHash(
                        Encoding.Default.GetBytes(propFull)
                        )
                    )
                    .Replace("-", "");
            }
            string[] resp = { propHash, propFull };
            return resp;
        }
    }
}
