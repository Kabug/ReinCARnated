using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : MonoBehaviour
{
    public GameObject UIElements;
    public Canvas UICanvas;

    public TMPro.TextMeshPro StartText;
    public TMPro.TextMeshPro EndText;

    public Cinemachine.CinemachineVirtualCamera MainMenuCamera;
    public Cinemachine.CinemachineVirtualCamera CarCamera;

    public float UIElementsTransitionTime;
    public float UIElementsTransitionHeight;

    private CustomInput input = null;

    // Control States
    private bool isEnter;
    private bool isExit;
    
    // other private vars
    public bool isStarted = false;
    private bool menuControl = false;
    private Vector3 startPosition;
    private Vector3 newPosition;
    private float startTime;

    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        UICanvas.enabled = false;
        arrow.SetActive(false);
        input = new CustomInput();
        input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        isEnter = input.Menu.Confirm.ReadValue<float>() == 1;
        isExit = input.Menu.Exit.ReadValue<float>() == 1;

        if (isExit && menuControl) Application.Quit();
        if (isEnter && menuControl) StartGame();
      
        if (isStarted)
        {
            UIElements.GetComponent<Transform>().localPosition = Vector3.Lerp(startPosition, newPosition, (Time.time - startTime) / UIElementsTransitionTime);
        }
    }

    public void startTransition()
    {
        menuControl = true;
        UIElements.GetComponent<Rigidbody>().useGravity = true;
    }

    private void StartGame() {
        input.Disable();
        print("Starting Game...");

        // Enable ui canvas
        UICanvas.enabled = true;

        arrow.SetActive(true);

        // Switch Cameras
        MainMenuCamera.enabled = false;

        startPosition = UIElements.GetComponent<Transform>().localPosition;
        newPosition = new Vector3();
        newPosition.Set(startPosition.x, startPosition.y + UIElementsTransitionHeight, startPosition.z);

        isStarted = true;
        GameTracker.Instance.setGamestate(GameTracker.GameStates.Playing);
        startTime = Time.time;
    }
}
