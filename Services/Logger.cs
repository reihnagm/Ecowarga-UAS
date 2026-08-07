namespace EcoWarga.Services;

public class Logger
{
    private readonly string _path;

    public Logger(string path)
    {
        _path = path;
    }

    public void Tulis(string level, string pesan)
    {
        try
        {
            string? folder = Path.GetDirectoryName(_path);
            if (!string.IsNullOrWhiteSpace(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string baris = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToUpperInvariant()}] {pesan}";
            File.AppendAllText(_path, baris + Environment.NewLine);
        }
        catch (IOException)
        {
            // Logging tidak boleh membuat aplikasi utama berhenti.
        }
        catch (UnauthorizedAccessException)
        {
            // Logging tidak boleh membuat aplikasi utama berhenti.
        }
    }
}
