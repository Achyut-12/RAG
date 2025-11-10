using MathNet.Numerics.LinearAlgebra;

public static class CosineSimilarity
{

    public static float CosineSimilarityMethod(Vector<float> a, Vector<float> b) => (float)(a.DotProduct(b) / (a.L2Norm() * b.L2Norm()));
}