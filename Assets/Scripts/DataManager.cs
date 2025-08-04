using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameData gameData;

    #region Singleton
    private static DataManager _instance;
    public static DataManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DataManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    #endregion

    public void SaveGameData() 
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        gameData.playerLifes = player._lifes;
        gameData.playerPosition = player.transform.position;
        gameData.playerGold = FindFirstObjectByType<GameController>()._playerPoints;


        //SaveSystem.SaveDataPP(gameData);
        //SaveSystem.SaveData(gameData);
        SaveSystem.SaveDataBinary(gameData);
    }

    public void LoadGameData() 
    {
        //gameData = SaveSystem.LoadDataPP();
        //gameData = SaveSystem.LoadData();
        gameData = SaveSystem.LoadDataBinary();
    }

    public bool ExistSaveData() 
    {
        return SaveSystem.HaveData();
    }
}
