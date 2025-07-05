using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> lifes = new List<GameObject>();
    [SerializeField] private Slider lifeBar;
    private int lifesInt = 3;
    [SerializeField] private TextMeshProUGUI coinsTxt;
    [SerializeField] private Button startGame;

    [SerializeField] private GameObject loseScreen;

    private void Start()
    {
        PlayerController.OnPlayerHit += SubstractLife;
        lifeBar.maxValue = lifesInt;
        lifeBar.value = lifesInt;

        //startGame.onClick.AddListener(StartClick);
    }

    public void SubstractLife() 
    {
        GameObject _life = lifes.First(life => life.activeSelf.Equals(true));
        _life.SetActive(false);
        lifesInt--;
        lifeBar.value = lifesInt;
    }

    public void AddCoins(int amount) { coinsTxt.text = $"COINS: {amount}"; }

    public void ShowLoseScreen() 
    {
        loseScreen.SetActive(true);
    }

    public void StartClick() 
    {
        Debug.Log("START GAME CLICKED");
    }
}
