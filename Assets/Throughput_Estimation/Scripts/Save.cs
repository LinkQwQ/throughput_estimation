using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // 导入System.IO命名空间以进行文件操作
using System.Text;  // 导入System.Text来使用StringBuilder

public class Save : MonoBehaviour
{
    void Start()
    {
        string path = "Assets/coordinates.csv";  // 指定文件路径和文件名

        // 创建一个StringBuilder来构建CSV内容
        StringBuilder csvContent = new StringBuilder();

        // 添加CSV头
        csvContent.AppendLine("Name,X,Y,Type");

        // 添加一些数据，示例数据可以根据实际需要调整
        csvContent.AppendLine("Location1,100,200,TypeA");
        csvContent.AppendLine("Location2,110,220,TypeB");
        csvContent.AppendLine("Location3,120,240,TypeC");

        // 使用StreamWriter来写入文件
        StreamWriter writer = new StreamWriter(path, false);  // false表示覆盖已存在的文件
        writer.Write(csvContent.ToString());
        writer.Close();  // 关闭StreamWriter对象
    }
}
