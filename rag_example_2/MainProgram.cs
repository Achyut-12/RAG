
using MathNet.Numerics.LinearAlgebra;
using UglyToad.PdfPig;

namespace RagEngine
{
    public class MainProgram
    {
        // How to run
        //cd C:\Users\E40019404\AppData\Local\Programs\Ollama\
        //ollama serve

        // PEF path
        //C:\Users\E40019404\Downloads\10101023630352_Oct2025.pdf

        private static string[] Documents { get; set; }
        private static Vector<float>[] Embeddings { get; set; }
        private static bool IsLoaded => Documents != null && Embeddings != null;
        public async Task LoadPDF(string args)
        {
            // -----------------------------
            // STEP 1: Load PDF and extract text
            // -----------------------------
            // Console.Write("Enter PDF path: ");
            // var pdfPath = Console.ReadLine();

            //var pdfPath = $"C:\\Users\\E40019404\\Downloads\\10101023630352_Oct2025.pdf";
            var paragraphs = new List<string>();
            using (var document = PdfDocument.Open(args))
            {
                foreach (var page in document.GetPages())
                {
                    paragraphs.Add(page.Text);
                }
            }

            // chunking stratgy 
            // Split into chunks (~500 characters each)
            //static IEnumerable<string> ChunkText(string text, int chunkSize = 500)
            //{
            //    for (int i = 0; i < text.Length; i += chunkSize)
            //        yield return text.Substring(i, Math.Min(chunkSize, text.Length - i));
            //}

            Documents = paragraphs
                .SelectMany(p => TextChunker.ChunkText(p))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();

            Embeddings = Documents.Select(d => ComputeEmbeddings.ComputeEmbedding(d)).ToArray();
            
        }

        public async Task<string> AskQuestion(string question)
        {
            if (!IsLoaded)
                return "❌ No PDF loaded yet. Please upload a file first.";
            // -----------------------------
            // STEP 4: Interactive Question Loop
            // -----------------------------
            return await InteractiveQuestionLoop.InteractiveQuestionLoops(Documents, Embeddings, question);
        }
    }
}