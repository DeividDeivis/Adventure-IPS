using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> lifes = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI coinsTxt;

    [SerializeField] private GameObject loseScreen;

    private void Start()
    {
        PlayerController.OnPlayerHit += SubstractLife;
    }

    public void SubstractLife() 
    {
        GameObject _life = lifes.First(life => life.activeSelf.Equals(true));
        _life.SetActive(false);
    }

    public void AddCoins(int amount) { coinsTxt.text = $"COINS: {amount}"; }

    public void ShowLoseScreen() 
    {
        loseScreen.SetActive(true);
    }
}
