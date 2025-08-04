using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    #region Jason Save & Load
    public static void SaveData(GameData dataToSave)
    {
        string dataToJson = JsonUtility.ToJson(dataToSave);
        File.WriteAllText(GetPath(), dataToJson);
        Debug.Log("DATA SAVED IN" + GetPath());
    }

    public static GameData LoadData()
    {
        if (File.Exists(GetPath()))
        {
            string json = File.ReadAllText(GetPath());
            GameData dataLoaded = JsonUtility.FromJson<GameData>(json);
            Debug.Log("DATA LOADED");
            return dataLoaded;
        }
        else 
        {
            Debug.LogError("Data not founded in path" + GetPath());
            return null;
        }
    }
    #endregion

    #region PlayerPref Save & Load
    public static void SaveDataPP(GameData dataToSave)
    {
        string dataToJson = JsonUtility.ToJson(dataToSave);
        PlayerPrefs.SetString("Game Data", dataToJson);
        PlayerPrefs.SetInt("Player Life", dataToSave.playerLifes);
        Debug.Log("DATA SAVED");

        //PlayerPrefs.DeleteKey("Game Data");
    }

    public static GameData LoadDataPP()
    {
        if (PlayerPrefs.HasKey("Game Data"))
        {
            string json = PlayerPrefs.GetString("Game Data");
            GameData dataLoaded = JsonUtility.FromJson<GameData>(json);
            Debug.Log("DATA LOADED");
            return dataLoaded;
        }
        else
        {
            Debug.LogError("Data not founded in path" + GetPath());
            return null;
        }
    }
    #endregion

    #region Binary Save & Load
    public static void SaveDataBinary(GameData dataToSave) 
    {
        BinaryFormatter formatter = new BinaryFormatter(); // Clase utilizada para generar un formato binario.

        FileStream stream = new FileStream(GetPath(true), FileMode.Create); // Inicio una transmision de archivos para guardar.

        GameDataPrimitive primitive = new GameDataPrimitive(dataToSave);

        formatter.Serialize(stream, primitive); // Serializamos nuestra data.
        stream.Close(); // Una vez concluida la tarea cerramos la transmision de archivos.

        Debug.Log("DATA SAVED IN" + GetPath(true));
    }

    public static GameData LoadDataBinary()
    {
        if (File.Exists(GetPath(true)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(true), FileMode.Open);

            GameDataPrimitive loadedData = formatter.Deserialize(stream) as GameDataPrimitive;

            GameData data = GameDataPrimitive.PrimitiveToGameData(loadedData);
            stream.Close();

            return data;
        }
        else 
        {
            Debug.LogError("Data not founded in path" + GetPath(true));
            return null;
        }
    }
    #endregion

    private static string GetPath(bool binary = false) 
    {
        //Windows => Application.persistentDataPath = C:\Users\Nuestra PC\AppData\LocalLow\Carpeta Empresa (Trayecto VJ en este caso)

        string path;
        if (binary) 
            path = Application.persistentDataPath + "/Save_Data.save";
        else
            path = Application.persistentDataPath + "/Save_Data.json";

        return path;
    }

    public static bool HaveData() 
    {
        bool data = false;
        if (File.Exists(GetPath()) ||
            PlayerPrefs.HasKey("Game Data") ||
            File.Exists(GetPath(true)))
            data = true;

        return data;
    } 
}

[Serializable]
public class GameData 
{
    public Vector3 playerPosition;
    public int playerLifes;
    public int playerGold;
}

[Serializable]
public class GameDataPrimitive // Use primitive data for binary file
{
    public float[] playerPosition;
    public int playerLifes;
    public int playerGold;

    public GameDataPrimitive(GameData data) 
    {
        playerPosition = new float[3];
        playerPosition[0] = data.playerPosition.x;
        playerPosition[1] = data.playerPosition.y;
        playerPosition[2] = data.playerPosition.z;

        playerLifes = data.playerLifes;
        playerGold = data.playerGold;
    }

    public static GameData PrimitiveToGameData(GameDataPrimitive primitive) 
    {
        GameData data = new GameData();
        data.playerPosition = new Vector3(primitive.playerPosition[0], primitive.playerPosition[1], primitive.playerPosition[2]);
        data.playerLifes = primitive.playerLifes;
        data.playerGold = primitive.playerGold;
        return data;
    }
}
