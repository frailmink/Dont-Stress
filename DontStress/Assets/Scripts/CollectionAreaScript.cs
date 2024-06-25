using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionAreaScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int speed = 1;  

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coins"))
        {
            CoinScript coinScript = collision.gameObject.GetComponent<CoinScript>();
            if (coinScript != null)
            {
                coinScript.player = this.transform;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coins"))
        {
            CoinScript coinScript = collision.gameObject.GetComponent<CoinScript>();
            if (coinScript != null)
            {
                coinScript.player = null;
                Rigidbody2D coinRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (coinRb != null)
                {
                    coinRb.velocity = Vector2.zero;
                }
            }
        } 
    }
}
