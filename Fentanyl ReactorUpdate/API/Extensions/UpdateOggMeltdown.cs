using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Exiled.API.Features;
using UnityEngine.PlayerLoop;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public class UpdateOggMeltdown
    {
        private static readonly string RepositoryUrl = "https://api.github.com/repos/FentanylReactorGER/FentanylMeltdownAudio/releases/latest";
        private static readonly string AudioFileName = "FentReactorMeltdown.ogg";
        private static readonly string AudioFilePath = Path.Combine(Paths.Plugins, AudioFileName);
        private static readonly HttpClient HttpClient = new HttpClient
        {
            DefaultRequestHeaders = { { "User-Agent", "AudioUpdater" } }
        };
        
        public static void RegisterEvents()
        {
            // Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingForPlayers;
        }

        public static void UnRegisterEvents()
        {
            // Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingForPlayers;
        }

        private static void WaitingForPlayers()
        {
            LogInfo("Checking for audio updates...");
            Task.Run(() => CheckForAudioUpdate());
        }

        private static void LogInfo(string message)
        {
            if (Plugin.Singleton.Config.Debug)
            {
                Log.Info(message);
            }
        }

        private static void LogWarn(string message)
        {
            if (Plugin.Singleton.Config.Debug)
            {
                Log.Warn(message);
            }
        }

        private static void LogError(string message)
        {
            if (Plugin.Singleton.Config.Debug)
            {
                Log.Error(message);
            }
        }

        private static async Task CheckForAudioUpdate()
        {
            try
            {
                // Check if the OGG file already exists
                if (File.Exists(AudioFilePath))
                {
                    LogInfo("Audio file already exists.");
                    return;
                }

                // Fetch the latest release details from GitHub API
                var response = await HttpClient.GetAsync(RepositoryUrl);
                if (!response.IsSuccessStatusCode)
                {
                    LogError($"Failed to check for audio updates: {response.StatusCode} - {response.ReasonPhrase}");
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var downloadUrl = ExtractDownloadUrl(content);

                if (downloadUrl == null)
                {
                    LogError("Failed to parse download URL.");
                    return;
                }

                // Download the audio file and save it
                LogInfo("Downloading audio file...");
                await DownloadAudioFileAsync(downloadUrl);
            }
            catch (Exception ex)
            {
                LogError($"Error while checking for audio updates: {ex.Message}");
            }
        }

        private static string ExtractDownloadUrl(string json)
        {
            try
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                return obj["assets"]?[0]?["browser_download_url"]?.ToString();
            }
            catch (Exception ex)
            {
                LogError($"Failed to extract download URL: {ex.Message}");
                return null;
            }
        }

        private static async Task DownloadAudioFileAsync(string downloadUrl)
        {
            try
            {
                var audioData = await HttpClient.GetByteArrayAsync(downloadUrl);
                File.WriteAllBytes(AudioFilePath, audioData);
                LogInfo($"Audio file downloaded successfully: {AudioFilePath}");
            }
            catch (Exception ex)
            {
                LogError($"Error during audio file download: {ex.Message}");
            }
        }
    }
}
