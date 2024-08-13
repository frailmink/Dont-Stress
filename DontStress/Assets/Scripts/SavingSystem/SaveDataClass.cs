using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataClass
{
    public List<string> booksUnlocked;

    public SaveDataClass(List<GameObject> listBooks)
    {
        booksUnlocked = new List<string>();
        foreach (GameObject book in listBooks)
        {
            booksUnlocked.Add(book.name);
        }
    }
}
