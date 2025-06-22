using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game State Info")]
    [SerializeField] private int _Points = 0;

    [SerializeField] private UIController _UI;
    [SerializeField] private List<EnemyController> _EnemiesInLevel = new List<EnemyController>();
   
    void Awake()
    {
        PlayerController.OnPlayerDead += PlayerDead;
        CoinController.OnCoinObtained += PlayerObtainCoin;
    }

    private void Start()
    {
        _EnemiesInLevel = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID).ToList();

        _Points = 0;
        _UI.AddCoins(_Points);
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
}
