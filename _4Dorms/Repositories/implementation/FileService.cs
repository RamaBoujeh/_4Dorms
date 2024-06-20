using _4Dorms.Repositories.Interfaces;

public class FileService : IFileService
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
            if (!Directory.Exists(_uploadsFolderPath))
            {
                Directory.CreateDirectory(_uploadsFolderPath);
                _logger.LogInformation($"Created uploads folder at {_uploadsFolderPath}");
            }

            var filePath = Path.Combine(_uploadsFolderPath, fileName);
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

    public bool DeleteFile(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_uploadsFolderPath, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation($"File {fileName} deleted successfully.");
                return true;
            }
            else
            {
                _logger.LogWarning($"File {fileName} not found.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting file {fileName}.");
            return false;
        }
    }
}
