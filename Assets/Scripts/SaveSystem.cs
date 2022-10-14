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
            writeStream.Close();
        }
    }

    public static T LoadData<T>(string _filePath)
    {
        using (FileStream readStream = new FileStream(_filePath, FileMode.Open))
        {
            if (readStream != null)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                T data = (T)binaryFormatter.Deserialize(readStream);
                readStream.Close();
                return data;
            }
            DeleteFile(_filePath);
            readStream.Close();
            return default(T);
        }
    }

    public static void DeleteFile(string _filePath)
    {
        File.Delete(_filePath);
    }
}
