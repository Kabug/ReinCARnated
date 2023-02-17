using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{

    public HealthBar healthBar;
    public float currentHealth;
    public float maxHealth = 100;
    public float dmgPerHit = 10;

    public GameObject popupUI;
    private bool isPopupActive = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
    }

    void OnCollisionEnter(Collision col)
    {
        string colTag = col.gameObject.tag.ToLower();
        if (colTag == "endpoint")
        {
            Debug.Log("Endpoint reached");
            //Add game end UI stuff here
            /*if (!isPopupActive)
            {
                //popupUI.SetActive(true);
                isPopupActive = true;
            }*/
        }
        else if (colTag == "enemy")
        {
            Debug.Log("You got schmecked");

            currentHealth -= dmgPerHit;
            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}
