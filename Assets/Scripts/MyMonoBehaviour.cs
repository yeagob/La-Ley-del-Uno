
using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    protected GameManager manager => GameManager.instance;
    protected LevelManager levels => manager.levelManager;
    protected ZoomManager zoom => manager.zoomManager;
    protected LaserController laser => manager.laserController;
    protected AudioManager audio => manager.audioManager;
    protected CreditsController credits => manager.creditsController;

    protected TextUI label => manager.textUI;
}
