using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NDAProcesses.Server.Services
{
    public class NotificationEmailStore : INotificationEmailStore
    {
        private readonly ILogger<NotificationEmailStore> _logger;
        private readonly string _filePath;
        private static readonly SemaphoreSlim _mutex = new(1, 1);
        private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

        public NotificationEmailStore(IHostEnvironment environment, ILogger<NotificationEmailStore> logger)
        {
            _logger = logger;
            var dataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataDirectory);
            _filePath = Path.Combine(dataDirectory, "notification-settings.json");
        }

        public async Task<string?> GetAsync()
        {
            await _mutex.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    return null;
                }

                var json = await File.ReadAllTextAsync(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                var payload = JsonSerializer.Deserialize<NotificationEmailPayload>(json, _serializerOptions);
                if (payload == null || string.IsNullOrWhiteSpace(payload.Email))
                {
                    return null;
                }

                return payload.Email;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Notification settings file {File} is invalid JSON.", _filePath);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to read notification settings from {File}.", _filePath);
                return null;
            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task SetAsync(string? email)
        {
            await _mutex.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    if (File.Exists(_filePath))
                    {
                        File.Delete(_filePath);
                    }
                    return;
                }

                var payload = new NotificationEmailPayload
                {
                    Email = email,
                    UpdatedAtUtc = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(payload, _serializerOptions);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save notification settings to {File}.", _filePath);
                throw new InvalidOperationException("Failed to save notification email settings.", ex);
            }
            finally
            {
                _mutex.Release();
            }
        }

        private class NotificationEmailPayload
        {
            public string? Email { get; set; }
            public DateTime UpdatedAtUtc { get; set; }
        }
    }
}
