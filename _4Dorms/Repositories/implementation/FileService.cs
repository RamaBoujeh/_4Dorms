using System;
using System.IO;
using Microsoft.Extensions.Logging;

public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly string _uploadsFolderPath;

    public FileService(ILogger<FileService> logger, string uploadsFolderPath)
    {
        _logger = logger;
        _uploadsFolderPath = uploadsFolderPath;
    }

    public bool SaveFile(byte[] fileBytes, string fileName)
    {
        try
        {
            // Ensure the uploads folder exists
            if (!Directory.Exists(_uploadsFolderPath))
            {
                Directory.CreateDirectory(_uploadsFolderPath);
                _logger.LogInformation($"Created uploads folder at {_uploadsFolderPath}");
            }

            // Combine the folder path and file name
            var filePath = Path.Combine(_uploadsFolderPath, fileName);

            // Save the file
            File.WriteAllBytes(filePath, fileBytes);
            _logger.LogInformation($"File saved successfully: {filePath}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving file: {ex.Message}");
            return false;
        }
    }
}
