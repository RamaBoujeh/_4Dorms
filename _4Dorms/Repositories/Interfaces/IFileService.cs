namespace _4Dorms.Repositories.Interfaces
{
    public interface IFileService
    {
        bool SaveFile(byte[] fileBytes, string fileName);
        bool DeleteFile(string filePath);
    }
}
