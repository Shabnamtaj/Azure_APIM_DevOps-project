using Microsoft.AspNetCore.Mvc;
using YourNamespace.Services;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public IActionResult GetAllFiles()
    {
        var files = _fileService.GetFiles();
        return Ok(files);
    }

    [HttpPost]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is missing");

        _fileService.UploadFile(file);
        return Ok("File uploaded successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteFile(int id)
    {
        _fileService.DeleteFile(id);
        return Ok("File deleted successfully");
    }
}
