using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    [Header("Game State Info")]
    [SerializeField] private int _Points = 0;
    public int _playerPoints => _Points;

    [SerializeField] private UIController _UI;
    [SerializeField] private List<EnemyController> _EnemiesInLevel = new List<EnemyController>();

    [Header("Cut Scene")]
    [SerializeField] private bool showCutScene = false;
    [SerializeField] private int enemiesToKill = 0;
    [SerializeField] private PlayableDirector _Director;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private List<ParticleSystem> destroyRocksParticles;
    private int currentRock = 0;
   
    void Awake()
    {
        PlayerController.OnPlayerDead += PlayerDead;
        CoinController.OnCoinObtained += PlayerObtainCoin;
        EnemyController.OnEnemyDead += CheckCompleteQuest;
    }

    private void Start()
    {
        _EnemiesInLevel = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID).ToList();
        enemiesToKill = _EnemiesInLevel.Count;

        _Points = 0;
        _UI.AddCoins(_Points);

        SetLoadData();
    }

    private void PlayerDead()
    {      
        foreach (var enemy in _EnemiesInLevel)
        {
            enemy.StopChaise();
        }

        //_UI.ShowLoseScreen();    
    }

    private void PlayerObtainCoin() 
    {
        _Points += 25;
        _UI.AddCoins(_Points);
    }

    private void CheckCompleteQuest() 
    {
        if (!showCutScene) return;

        enemiesToKill--;
        if (enemiesToKill == 0)
            _Director.Play();
    }

    public void DestroyRocks() 
    {
        obstacles[currentRock].SetActive(false);
        destroyRocksParticles[currentRock].Play();
        currentRock++;
    }

    private void SetLoadData() 
    {
        if (DataManager.instance.ExistSaveData()) 
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();

            _Points = DataManager.instance.gameData.playerGold;
            player.SetData(DataManager.instance.gameData.playerPosition, DataManager.instance.gameData.playerLifes);
        }
    }
}
