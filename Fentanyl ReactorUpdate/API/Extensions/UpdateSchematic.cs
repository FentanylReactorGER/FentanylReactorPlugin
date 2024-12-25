using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace Fentanyl_ReactorUpdate.API.Extensions
{
    public class UpdateSchematicChecker
    {
        private static readonly string RepositoryUrl = "https://api.github.com/repos/FentanylReactorGER/FentanylReactorSchematic/releases/latest";
        private static readonly string SchematicsPath = Path.Combine(Paths.Configs, "MapEditorReborn", "Schematics");
        private static readonly string FentanylReactorPath = Path.Combine(SchematicsPath, "FentanylReactor");
        private static readonly string VersionFilePath = Path.Combine(FentanylReactorPath, "version.txt");
        private static readonly string DownloadPath = Path.Combine(Paths.Configs, "MapEditorReborn", "Schematics");
        private static readonly HttpClient HttpClient = new HttpClient
        {
            DefaultRequestHeaders = { { "User-Agent", "UpdateSchematicChecker" } }
        };

        public UpdateSchematicChecker()
        {
            EnsureSchematicDirectoryExists();
        }

        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingForPlayers;
        }

        public static void UnRegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingForPlayers;
        }

        private static void WaitingForPlayers()
        {
            Log.Info("Searching for Schematic Updates...");
            Task.Run(CheckForSchematicUpdates);
        }

        private static void EnsureSchematicDirectoryExists()
        {
            if (!Directory.Exists(SchematicsPath))
            {
                Directory.CreateDirectory(SchematicsPath);
                Log.Info($"The schematic directory '{SchematicsPath}' did not exist and was created.");
            }
            else
            {
                Log.Info($"Schematic directory '{SchematicsPath}' verified.");
            }
        }

        private static async Task CheckForSchematicUpdates()
        {
            try
            {
                // Check if the version file exists, if not, download the latest schematic
                if (!File.Exists(VersionFilePath) || !Directory.Exists(FentanylReactorPath))
                {
                    Log.Warn("Version file or schematic not found. Downloading the latest schematic...");
                    await DownloadAndReplaceFiles(await GetLatestReleaseDownloadUrl());
                    return;
                }

                // If version file exists, check the version and update if necessary
                string currentVersion = File.ReadAllText(VersionFilePath).Trim();
                if (string.IsNullOrEmpty(currentVersion))
                {
                    Log.Warn("Version file is empty. Skipping update check.");
                    return;
                }

                Log.Info($"Current schematic version: {currentVersion}");

                // Fetch GitHub latest release
                var response = await HttpClient.GetAsync(RepositoryUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Warn($"Failed to check GitHub releases: {response.StatusCode}");
                    return;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                string latestVersion = ExtractLatestVersion(jsonResponse);
                string downloadUrl = ExtractDownloadUrl(jsonResponse);

                if (latestVersion == null || downloadUrl == null)
                {
                    Log.Warn("Failed to parse release information.");
                    return;
                }

                if (IsNewerVersion(currentVersion, latestVersion))
                {
                    Log.Warn($"A new schematic version is available: {latestVersion} (current: {currentVersion})");
                    await DownloadAndReplaceFiles(downloadUrl);
                    UpdateVersionFile(latestVersion);
                    Log.Info("Schematic updated successfully.");
                }
                else
                {
                    Log.Info("You are using the latest schematic version.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error while checking for schematic updates: {ex.Message}");
            }
        }

        private static string ExtractLatestVersion(string json)
        {
            try
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
                return obj["tag_name"]?.ToString();
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to extract the latest version: {ex.Message}");
                return null;
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
                Log.Error($"Failed to extract download URL: {ex.Message}");
                return null;
            }
        }

        private static bool IsNewerVersion(string currentVersion, string latestVersion)
        {
            if (Version.TryParse(currentVersion, out var current) && Version.TryParse(latestVersion, out var latest))
            {
                return latest > current;
            }

            Log.Warn("Failed to compare versions. Using current version as the latest.");
            return false;
        }

        private static async Task<string> GetLatestReleaseDownloadUrl()
        {
            try
            {
                var response = await HttpClient.GetAsync(RepositoryUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Warn($"Failed to check GitHub releases: {response.StatusCode}");
                    return null;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return ExtractDownloadUrl(jsonResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching download URL: {ex.Message}");
                return null;
            }
        }

        private static async Task DownloadAndReplaceFiles(string downloadUrl)
        {
            try
            {
                if (downloadUrl == null)
                {
                    Log.Warn("Download URL is null. Cannot proceed with download.");
                    return;
                }

                string zipFilePath = Path.Combine(DownloadPath, "schematic.zip");

                // Download the schematic zip file from GitHub directly into the DownloadPath
                byte[] fileData = await HttpClient.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(zipFilePath, fileData);
                Log.Info("Downloaded new schematic version (schematic.zip).");

                // Delete the existing FentanylReactor schematic folder if it exists
                if (Directory.Exists(FentanylReactorPath))
                {
                    Directory.Delete(FentanylReactorPath, true);
                    Log.Info("Deleted the old FentanylReactor schematic folder.");
                }

                // Extract the zip directly into the Schematics directory
                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, SchematicsPath);
                Log.Info("Extracted the schematic files directly into the Schematics directory.");

                // Clean up the downloaded zip file after extraction
                File.Delete(zipFilePath);
                Log.Info("Deleted the schematic.zip file after extraction.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error during schematic update: {ex.Message}");
            }
        }

        private static void UpdateVersionFile(string latestVersion)
        {
            try
            {
                File.WriteAllText(VersionFilePath, latestVersion);
                Log.Info($"Updated version file to: {latestVersion}");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to update version file: {ex.Message}");
            }
        }
    }
}


