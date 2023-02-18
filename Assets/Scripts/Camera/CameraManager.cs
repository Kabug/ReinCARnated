using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    public CinemachineVirtualCamera MainMenuCamera;
    public CinemachineVirtualCamera CarCamera;
    public CinemachineVirtualCamera TargetCamera;

    private int lowPrio = 10;
    private int hiPrio = 11;

    private GameTracker.GameStates previousState;
    // Start is called before the first frame update
    void Start()
    {
        previousState = GameTracker.Instance.GAMESTATE;
        MainMenuCamera.Priority = hiPrio;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousState != GameTracker.Instance.GAMESTATE)
        {
            switch (GameTracker.Instance.GAMESTATE)
            {
                case GameTracker.GameStates.Menu:
                    MainMenuCamera.Priority = hiPrio;
                    CarCamera.Priority = lowPrio;
                    TargetCamera.Priority = lowPrio;
                    break;
                case GameTracker.GameStates.Playing:
                    MainMenuCamera.Priority = lowPrio;
                    CarCamera.Priority = hiPrio;
                    TargetCamera.Priority = lowPrio;
                    break;
                case GameTracker.GameStates.End:
                    MainMenuCamera.Priority = lowPrio;
                    CarCamera.Priority = lowPrio;
                    TargetCamera.Priority = hiPrio;
                    break;
            }
        }
        else
        {
            previousState = GameTracker.Instance.GAMESTATE;
        }

    }
}
