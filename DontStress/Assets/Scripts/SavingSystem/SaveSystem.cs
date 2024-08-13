using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public static class SaveSystem
{
    public static void SaveGame(List<GameObject> listBooks)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GlobalVariables.saveDataPath, FileMode.Create);

        SaveDataClass saveData = new SaveDataClass(listBooks);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }
}
