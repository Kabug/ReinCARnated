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
    public float healingPF = 0.25f;
    public float damagePF = 0.125f;

    public GameObject youWin;
    public GameObject gameOver;
    private bool isPopupActive = false;

    private CustomInput input = null;
    public bool isDrift;
    public bool isTurnLeft;
    public bool isTurnRight;
    public bool isAccel;
    public bool isDeccel;
    public bool isFlip;

    public float carSpeed;
    public float topSpeed = 37f;

    public GameStates GAMESTATE = GameStates.Menu;

    public Vector3 possibleSpawnSpot;
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
        isFlip = input.Player.Flip.ReadValue<float>() > 0.1f;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth == 0)
        {
            GAMESTATE = GameStates.End;
            gameOver.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (GAMESTATE == GameStates.Playing)
        {
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / topSpeed);
            if ((isDrift && normalizedSpeed > 0.6f && (isTurnLeft || isTurnRight) && (isAccel || isDeccel)))
            {
                currentHealth = Mathf.Min(maxHealth, currentHealth + healingPF);
            }
            else
            {
                currentHealth = Mathf.Max(0, currentHealth - damagePF);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        string colTag = col.gameObject.tag.ToLower();
        if (colTag == "endpoint")
        {
            PlayerPrefs.SetFloat("StartX", possibleSpawnSpot.x);
            PlayerPrefs.SetFloat("StartY", possibleSpawnSpot.y);
            PlayerPrefs.SetFloat("StartZ", possibleSpawnSpot.z);
            Debug.Log("Endpoint reached");
            GAMESTATE = GameStates.End;
            Physics.gravity = new Vector3(0, -4.20f, 0);
            youWin.SetActive(true);
            //Add game end UI stuff here
            
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

    public void setCarSpeed(float speed)
    {
        carSpeed = speed;
    }
}
