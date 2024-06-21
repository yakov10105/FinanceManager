namespace FinanceManager.Models.Caclulations;

public class KMeans
{
    public static (int[] clusters, double[][] centroids) Cluster(double[][] data, int k, int maxIterations = 100)
    {
        int n = data.Length;
        int m = data[0].Length;

        // Initialize centroids randomly
        var centroids = InitializeCentroids(data, k);

        int[] clusters = new int[n];
        bool centroidsChanged = true;
        int iterations = 0;

        while (centroidsChanged && iterations < maxIterations)
        {
            centroidsChanged = false;

            // Assign clusters
            for (int i = 0; i < n; i++)
            {
                int nearestCentroid = FindNearestCentroid(data[i], centroids);
                if (clusters[i] != nearestCentroid)
                {
                    clusters[i] = nearestCentroid;
                    centroidsChanged = true;
                }
            }

            // Update centroids
            centroids = UpdateCentroids(data, clusters, k);
            iterations++;
        }

        return (clusters, centroids);
    }

    private static double[][] InitializeCentroids(double[][] data, int k)
    {
        Random random = new Random();
        double[][] centroids = new double[k][];
        HashSet<int> selectedIndices = new HashSet<int>();

        for (int i = 0; i < k; i++)
        {
            int index;
            do
            {
                index = random.Next(data.Length);
            } while (selectedIndices.Contains(index));

            centroids[i] = (double[])data[index].Clone();
            selectedIndices.Add(index);
        }

        return centroids;
    }

    private static int FindNearestCentroid(double[] dataPoint, double[][] centroids)
    {
        int nearestCentroid = -1;
        double minDistance = double.MaxValue;

        for (int i = 0; i < centroids.Length; i++)
        {
            double distance = EuclideanDistance(dataPoint, centroids[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCentroid = i;
            }
        }

        return nearestCentroid;
    }

    private static double[][] UpdateCentroids(double[][] data, int[] clusters, int k)
    {
        int n = data.Length;
        int m = data[0].Length;

        double[][] newCentroids = new double[k][];
        int[] counts = new int[k];

        for (int i = 0; i < k; i++)
        {
            newCentroids[i] = new double[m];
        }

        for (int i = 0; i < n; i++)
        {
            int cluster = clusters[i];
            for (int j = 0; j < m; j++)
            {
                newCentroids[cluster][j] += data[i][j];
            }
            counts[cluster]++;
        }

        for (int i = 0; i < k; i++)
        {
            if (counts[i] == 0) continue;
            for (int j = 0; j < m; j++)
            {
                newCentroids[i][j] /= counts[i];
            }
        }

        return newCentroids;
    }

    private static double EuclideanDistance(double[] point1, double[] point2)
    {
        double sum = 0;
        for (int i = 0; i < point1.Length; i++)
        {
            sum += Math.Pow(point1[i] - point2[i], 2);
        }
        return Math.Sqrt(sum);
    }
}
