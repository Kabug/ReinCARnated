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

    public List<TireSuspension> playerController;

    // Control States
    private bool isEnter;
    private bool isExit;
    
    // other private vars
    private bool isStarted = false;
    private Vector3 startPosition;
    private Vector3 newPosition;
    private float startTime;
   

    // Start is called before the first frame update
    void Start()
    {
        UICanvas.enabled = false;
        input = new CustomInput();
        input.Enable();

        for(var i = 0; i < playerController.Count; i++)
        {
            playerController[i].disableControl();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isEnter = input.Menu.Confirm.ReadValue<float>() == 1;
        isExit = input.Menu.Exit.ReadValue<float>() == 1;

        if (isExit) Application.Quit();
        if (isEnter) StartGame();
      
        if (isStarted)
        {
            UIElements.GetComponent<Transform>().localPosition = Vector3.Lerp(startPosition, newPosition, (Time.time - startTime) / UIElementsTransitionTime);
        }
    }

    private void StartGame() {
        input.Disable();
        print("Starting Game...");

        // enable controls
        for (var i = 0; i < playerController.Count; i++)
        {
            playerController[i].enableControl();
        }

        // Enable ui canvas
        UICanvas.enabled = true;

        // Switch Cameras
        MainMenuCamera.enabled = false;

        startPosition = UIElements.GetComponent<Transform>().localPosition;
        newPosition = new Vector3();
        newPosition.Set(startPosition.x, startPosition.y + UIElementsTransitionHeight, startPosition.z);

        isStarted = true;
        startTime = Time.time;
    }
}
