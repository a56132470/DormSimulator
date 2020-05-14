using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;
    public Inventory storeInventory;
    public Inventory bagInventory;
    private string path;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        path = Application.persistentDataPath + "/SaveData";
        Debug.Log(path);
    }

    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        SaveInventory(storeInventory);
        SaveInventory(bagInventory);
        SaveCharacter(GlobalVariable.instance.player, GlobalVariable.instance.SaveID);
        SaveCharacter(GlobalVariable.instance.roommates[0], GlobalVariable.instance.SaveID, 0);
        SaveCharacter(GlobalVariable.instance.roommates[1], GlobalVariable.instance.SaveID, 1);
        SaveCharacter(GlobalVariable.instance.roommates[2], GlobalVariable.instance.SaveID, 2);
    }

    private void SaveInventory(Inventory inventory)
    {
        var inventoryPath = path + "/" + inventory.name + GlobalVariable.instance.SaveID + ".txt";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(inventoryPath);
        var json = JsonUtility.ToJson(inventory);
        formatter.Serialize(file, json);
        file.Close();
    }

    private void LoadInventory(Inventory inventory)
    {
        BinaryFormatter bf = new BinaryFormatter();
        var inventoryPath = path + "/" + inventory.name + ".txt";
        if (File.Exists(inventoryPath))
        {
            FileStream file = File.Open(inventoryPath, FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), inventory);
            file.Close();
        }
    }

    public void LoadGame(ref Player player, ref Roommate[] roommates, int count = 0)
    {
        player = (Player)LoadCharacter(player, count);
        if (player != null && !player.Name.Equals(""))
        {
            roommates[0] = (Roommate)LoadCharacter(roommates[0], count, 0);
            roommates[1] = (Roommate)LoadCharacter(roommates[1], count, 1);
            roommates[2] = (Roommate)LoadCharacter(roommates[2], count, 2);
        }
    }

    public void LoadInventoryOnStart()
    {
        LoadInventory(bagInventory);
        LoadInventory(storeInventory);
    }

    // TODO: 舍友的保存有bug，未完成保存
    private void SaveCharacter(BasePerson person, params int[] count)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string realPath = "";
        if (person is Player)
            realPath = path + "/Player" + count[0].ToString() + ".txt";
        else if (person is Roommate)
        {
            realPath = path + "/Roommate" + count[0].ToString() + count[1].ToString() + ".txt";
        }
        Stream stream = new FileStream(realPath, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, person);
        stream.Close();
    }

    private BasePerson LoadCharacter(BasePerson person, params int[] count)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string realPath = "";
        if (person is Player)
        {
            realPath = path + "/Player" + count[0].ToString() + ".txt";
        }
        else if (person is Roommate)
        {
            realPath = path + "/Roommate" + count[0].ToString() + count[1].ToString() + ".txt";
        }
        if (File.Exists(realPath))
        {
            FileStream fileStream = new FileStream(realPath, FileMode.Open,
                                                   FileAccess.Read, FileShare.Read);
            Stream stream = fileStream;
            person = (BasePerson)formatter.Deserialize(stream);
            stream.Close();
            return person;
        }
        else
            return null;
    }
}