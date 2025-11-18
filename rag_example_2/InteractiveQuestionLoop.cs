using MathNet.Numerics.LinearAlgebra;

public class InteractiveQuestionLoop
{

    public static async Task<string> InteractiveQuestionLoops(string[] documents, Vector<float>[] embeddings, string question)
    {
             Console.Write("\nEnter your question (or type 'exit' to quit): ");
            // var question = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(question) || question.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("👋 Goodbye!");
                return "";
            }

            var qVec = ComputeEmbeddings.ComputeEmbedding(question);

            // Retrieve top 3 relevant chunks
            var topDocs = documents.Zip(embeddings, (doc, emb) => (doc, CosineSimilarity.CosineSimilarityMethod(qVec, emb)))
                                   .OrderByDescending(x => x.Item2)
                                   .Take(5)
                                   .Select(x => x.doc);

            var context = string.Join("\n\n", topDocs);

            var prompt = $@"
                         You are an expert assistant. Answer the question based ONLY on the context below.
                         Do NOT make up information. If the answer is not in the context, say 'I don't know.'
                         
                         Context:
                         {context}
                         
                         Question:
                         {question}
                         
                         Answer:
                         ";
                         
            var answer = await GenerateAnswer.GenerateAnswerAsync(prompt);
            return answer;
        
    }
}