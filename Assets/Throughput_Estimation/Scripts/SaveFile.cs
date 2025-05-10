using System.IO;
using System.Text;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    // ✅ 保留你原来的保存路径逻辑
    private static string fileName = "positions.csv";
    private static string wallStatsFileName = "wall_stats.csv";
    private static string screenshotFileName = "screenshot.png";

    // ✅ 仍然使用 BaseDirectory，你之前就是这么保存成功的
    private static string defaultFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);
    private static string wallStatsPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, wallStatsFileName);
    private static string screenshotPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, screenshotFileName);

    private static string[] defaultHeaders = new string[] { "Name", "X", "Y", "Type" };

    public static void WriteCSV(string[][] data)
    {
        Debug.Log("✅ WriteCSV started");

        StringBuilder csvContent = new StringBuilder();
        csvContent.AppendLine(string.Join(",", defaultHeaders));
        foreach (var row in data)
        {
            csvContent.AppendLine(string.Join(",", row));
        }

        File.WriteAllText(defaultFilePath, csvContent.ToString(), Encoding.UTF8);
        Debug.Log("✅ Position CSV saved to: " + defaultFilePath);

        CaptureScreenshot();
    }

    public static void WriteWallStatsCSV(string[][] data)
    {
        Debug.Log("✅ WriteWallStatsCSV started");

        StringBuilder csvContent = new StringBuilder();
        foreach (var row in data)
        {
            csvContent.AppendLine(string.Join(",", row));
        }

        File.WriteAllText(wallStatsPath, csvContent.ToString(), Encoding.UTF8);
        Debug.Log("✅ Wall stats CSV saved to: " + wallStatsPath);

        CaptureScreenshot();
    }

    private static void CaptureScreenshot()
    {
        Debug.Log("📸 Capturing screenshot to: " + screenshotPath);
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }
}