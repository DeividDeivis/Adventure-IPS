using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static Action OnCoinObtained;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            OnCoinObtained?.Invoke();
            Destroy(gameObject);
        }           
    }
}
