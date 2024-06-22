using System.IO;
using System.Text;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    // 设定默认的文件名
    private static string fileName = "coordinates_Temp.csv";

    // 获取可执行文件所在目录
    private static string defaultFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);

    private static string[] defaultHeaders = new string[] { "Name", "X", "Y", "Type" };

    // 一个公开的静态方法，只需要外部提供数据
    public static void WriteCSV(string[][] data)
    {
        StringBuilder csvContent = new StringBuilder();

        // 添加固定的头部到CSV中
        csvContent.AppendLine(string.Join(",", defaultHeaders));

        // 遍历数据并添加到CSV内容中
        foreach (var row in data)
        {
            csvContent.AppendLine(string.Join(",", row));
        }

        // 写入固定的文件路径
        using (StreamWriter writer = new StreamWriter(defaultFilePath, false)) // 使用using语句自动关闭StreamWriter
        {
            writer.Write(csvContent.ToString());
        }

        Debug.Log("CSV file saved to: " + defaultFilePath);
    }
}