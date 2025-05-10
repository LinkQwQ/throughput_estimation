using System.IO;
using System.Text;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    private static string[] defaultHeaders = new string[] { "Name", "X", "Y", "Type" };

    public static void WriteCSV(string[][] data)
    {
        string folder = Path.Combine(Application.dataPath, "Throughput_Estimation/Csv");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, "coordinates_Temp.csv");

        StringBuilder csv = new StringBuilder();
        csv.AppendLine(string.Join(",", defaultHeaders));
        foreach (var row in data) csv.AppendLine(string.Join(",", row));

        File.WriteAllText(path, csv.ToString(), Encoding.UTF8);
        Debug.Log("✅ Position CSV saved to: " + path);
    }

    public static void WriteWallStatsCSV(string[][] data)
    {
        string folder = Path.Combine(Application.dataPath, "Throughput_Estimation/Csv");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, "wall_stats.csv");

        StringBuilder csv = new StringBuilder();
        foreach (var row in data) csv.AppendLine(string.Join(",", row));

        File.WriteAllText(path, csv.ToString(), Encoding.UTF8);
        Debug.Log("✅ Wall stats CSV saved to: " + path);
    }
}