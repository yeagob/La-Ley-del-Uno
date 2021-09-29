using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMonoBehaviour
{
    public LevelManager levelManager;
    public LaserController laserController;
    public ZoomManager zoomManager;
    public AudioManager audioManager;
    //TODO UI Controller
    public CreditsController creditsController;
    public GameObject unityModeText;
    public GameObject instruccionesText;
    public TextUI textUI;
    public static GameManager instance;


    //Modo Unidad
#if UNITY_EDITOR
    public  bool unityMode = false;
#else
    internal bool unityMode = false;
#endif

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (unityMode && !unityModeText.activeSelf )
            unityModeText.SetActive(true);
        if (levels.currentLevel != Levels.Level1 && instruccionesText.activeSelf)
            instruccionesText.SetActive(false);
    }

}
