using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Base.Inventory;
using UnityEngine;
using DSD.KernalTool;
public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;
    public Inventory storeInventory;
    public Inventory bagInventory;
    private string m_Path;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        m_Path = Application.persistentDataPath + "/SaveData";
    }

    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(m_Path))
        {
            Directory.CreateDirectory(m_Path);
        }

        SaveInventory(storeInventory);
        SaveInventory(bagInventory);
        SaveCharacter(GlobalManager.Instance.player, GlobalManager.Instance.saveId);
        SaveCharacter(GlobalManager.Instance.roommates[0], GlobalManager.Instance.saveId, 0);
        SaveCharacter(GlobalManager.Instance.roommates[1], GlobalManager.Instance.saveId, 1);
        SaveCharacter(GlobalManager.Instance.roommates[2], GlobalManager.Instance.saveId, 2);
    }

    private void SaveInventory(Inventory inventory)
    {
        var inventoryPath = m_Path + "/" + inventory.name + GlobalManager.Instance.saveId + ".txt";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(inventoryPath);
        var json = JsonUtility.ToJson(inventory);
        formatter.Serialize(file, json);
        file.Close();
    }

    private void LoadInventory(Inventory inventory)
    {
        BinaryFormatter bf = new BinaryFormatter();
        var inventoryPath = m_Path + "/" + inventory.name + ".txt";
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

    private void SaveCharacter(BasePerson person, params int[] count)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string realPath = "";
        if (person is Player)
            realPath = m_Path + "/Player" + count[0].ToString() + ".txt";
        else if (person is Roommate)
        {
            realPath = m_Path + "/Roommate" + count[0].ToString() + count[1].ToString() + ".txt";
        }
        Stream stream = new FileStream(realPath, FileMode.Create, FileAccess.Write, FileShare.None);
        SaveTools.Instance.SaveDic(ref person.stateKeys, ref person.stateValues, person.stateDic);
        formatter.Serialize(stream, person);
        
        stream.Close();
    }

    private BasePerson LoadCharacter(BasePerson person, params int[] count)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string realPath = "";
        if (person is Player)
        {
            realPath = m_Path + "/Player" + count[0].ToString() + ".txt";
        }
        else if (person is Roommate)
        {
            realPath = m_Path + "/Roommate" + count[0].ToString() + count[1].ToString() + ".txt";
        }
        if (File.Exists(realPath))
        {
            FileStream fileStream = new FileStream(realPath, FileMode.Open,
                                                   FileAccess.Read, FileShare.Read);
            Stream stream = fileStream;
            person = (BasePerson)formatter.Deserialize(stream);
            stream.Close();
            SaveTools.Instance.LoadDic(person.stateKeys, person.stateValues, ref person.stateDic);
            return person;
        }

        return null;
    }
}