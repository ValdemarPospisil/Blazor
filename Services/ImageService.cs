public class ImageService
{
    // Method for finding images in given folder
    public Task<List<string>> GetImagePathsAsync(string path)
    {
        var images = new List<string>();

        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path, "*.jpg"); // Filtering .jpg Optimization: more formats
            foreach (var file in files)
            {
                images.Add($"images/gallery/{Path.GetFileName(file)}");
            }
        }

        return Task.FromResult(images);
    }
}