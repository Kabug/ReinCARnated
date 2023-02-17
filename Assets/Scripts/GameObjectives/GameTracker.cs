using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance { get; private set; }
    public enum GameStates
    {
        Menu,
        Playing,
        End
    }

    public HealthBar healthBar;
    public float currentHealth;
    public float maxHealth = 100;
    public float dmgPerHit = 10;

    public GameObject popupUI;
    private bool isPopupActive = false;

    private CustomInput input = null;
    public bool isDrift;
    public bool isTurnLeft;
    public bool isTurnRight;
    public bool isAccel;
    public bool isDeccel;

    public GameStates GAMESTATE = GameStates.Menu;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        input = new CustomInput();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        input.Enable();
        // healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        isDrift = input.Player.Drift.ReadValue<float>() > 0.1f;
        isTurnLeft = input.Player.TurnLeft.ReadValue<float>() > 0.1f;
        isTurnRight = input.Player.TurnRight.ReadValue<float>() > 0.1f;
        isAccel = input.Player.Accelerate.ReadValue<float>() > 0.1f;
        isDeccel = input.Player.Decelerate.ReadValue<float>() > 0.1f;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth == 0)
        {
            GAMESTATE = GameStates.End;
        }
    }

    private void FixedUpdate()
    {
        if ((isDrift && (isTurnLeft || isTurnRight) && (isAccel || isDeccel)) && GAMESTATE == GameStates.Playing)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + 0.25f);
        }
        else
        {
            currentHealth = Mathf.Max(0, currentHealth - 0.125f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string colTag = col.gameObject.tag.ToLower();
        if (colTag == "endpoint")
        {
            Debug.Log("Endpoint reached");
            GAMESTATE = GameStates.End;
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

    public void setGamestate(GameStates gamestate)
    {
        GAMESTATE = gamestate;
    }
}
