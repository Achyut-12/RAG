using Microsoft.AspNetCore.Mvc;
using Rag_Api;
using RagEngine;

[ApiController]
[Route("api/[controller]")]
public class PdfRagController : ControllerBase
{
    private MainProgram program;
    public PdfRagController()
    {
        program = new MainProgram();
    }

    [HttpPost("upload")]
    public IActionResult UploadPdf(IFormFile file)
    {
        if (file == null)
            return BadRequest("No file uploaded.");

        var tempPath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempPath))
        {
            file.CopyTo(stream);
        }
         
        var response =  program.LoadPDF(tempPath);
        return Ok(new { response });
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskQuestion([FromBody] QuestionRequest request)
    {
       var response =  await program.AskQuestion(request.Question);
        return Ok(response);    
    }
}
