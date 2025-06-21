using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController _UI;
    [SerializeField] private List<EnemyController> _EnemiesInLevel = new List<EnemyController>();
   
    void Awake()
    {
        PlayerController.OnPlayerDead += PlayerDead;
    }

    private void Start()
    {
        _EnemiesInLevel = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID).ToList();
    }

    private void PlayerDead()
    {      
        foreach (var enemy in _EnemiesInLevel)
        {
            enemy.StopChaise();
        }

        //_UI.ShowLoseScreen();    
    }
}
