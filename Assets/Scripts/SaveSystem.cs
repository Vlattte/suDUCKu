using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(string _filePath, object _data)
    {
        using (FileStream writeStream = new FileStream(_filePath, FileMode.Create))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(writeStream, _data);
        }
    }

    public static T LoadData<T>(string _filePath)
    {
        using (FileStream readStream = new FileStream(_filePath, FileMode.Open))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            T data = (T)binaryFormatter.Deserialize(readStream);
            return data;
        }
    }
}
