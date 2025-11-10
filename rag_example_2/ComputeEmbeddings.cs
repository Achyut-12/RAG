using MathNet.Numerics.LinearAlgebra;

public class ComputeEmbeddings
{
    public static Vector<float> ComputeEmbedding(string text)
    {
        var vec = Vector<float>.Build.Dense(384);
        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            int idx = Math.Abs(word.GetHashCode()) % 384;
            vec[idx] += 1;
        }
        var norm = (float)vec.L2Norm();
        if (norm > 0) vec = vec / norm;
        return vec;
    }
}