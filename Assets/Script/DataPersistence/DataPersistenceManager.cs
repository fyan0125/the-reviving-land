using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;

    //可以取用但不能更改這裡的值
    public static DataPersistenceManager instance { get; private set;} 

    private List<IDataPersistence> dataPersistenceObjects;

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogError("在這個場景找到多於一個 Data Persistence Manager");
        }
        instance = this;
    }

    private void Start() 
    {
        this.dataPersistenceObjects = FingAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TO-DO - Load any saved data from a file using the data handler
        // if no data can be loaded, initialize to a new game
        if(this.gameData == null)
        {
            Debug.Log("No data can be found. Initailize data to default.");
            NewGame();
        }

        // TO-DO - push the Loaded data to all the script that need it.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }  
        Debug.Log("Loaded current health = " + gameData.currentHealth);
    }

    public void SaveGame()
    {
        // TO-DO - pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }  
        Debug.Log("Saved current health = " + gameData.currentHealth);
        // TO-DO - save that data to a flie using the data handler 
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FingAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
