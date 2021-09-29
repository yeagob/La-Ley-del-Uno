using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserController : MyMonoBehaviour
{
    
    [SerializeField] AudioSource laserSound;
    [SerializeField] Laser [] lasers;
    [SerializeField] Laser [] lasersunityMode;

    private Laser laser;


    private int lastPoint = 0;

    private Camera camera;
    private bool drawing;


    void Awake()
    {
        //Init
        camera = Camera.main;
        NewLaser("");
        RefreshLaserScale(zoom.currenZone);
        
        //Listeners
        zoom.OnZoom += RefreshLaserScale;
        levels.OnResultCompleted += NewLaser;

    }

   

    // Update is called once per frame
    void Update()
    {
        if (drawing)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                laser.endParticles.transform.position = hit.point;
                laser.line.SetPosition(lastPoint, hit.point);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (manager.unityMode)
                {
                    if (laser.line.positionCount == 2)
                        ResetLaser();
                    NewLaser("");
                }
                else
                    ResetLaser();
            }
        }
    }

    internal void ResetAllLasers()
    {
        foreach (Laser laser in FindObjectsOfType<Laser>())
            laser.ResetLaset();
    }

    private void NewLaser(string resultName)
    {
        if (manager.unityMode)
            laser = Instantiate(lasersunityMode[(int)Random.Range(0, lasersunityMode.Length)], this.transform);
         else  
            laser = Instantiate(lasers[(int)levels.currentLevel], this.transform);

        ResetLaser();
        RefreshLaserScale(zoom.currenZone);
    }

    private void StartLaser()
    {
        laser.startParticles.gameObject.SetActive(true);
        laser.endParticles.gameObject.SetActive(true);
        drawing = true;
        laserSound.Play();
    }

    internal void ResetLaser()
    {
        laser.transform.position = Vector3.zero;
        lastPoint = 0;
        laser.ResetLaset();
        laserSound.Stop();
        drawing = false;
        levels.ResetSecuence();
    }
    internal void AddLaserPoint(Actor actorSelected)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 position;
            if (actorSelected.actorType == Actors.Cosmos && !manager.unityMode)
                position = camera.transform.position + Vector3.up * 20;
            else
                position = hit.point;

            if (!drawing)
            {
                StartLaser();
                laser.startParticles.transform.position = position;
            }

            laser.line.positionCount++;
            laser.line.SetPosition(lastPoint, position);
            lastPoint++;
        }
    }


    private void RefreshLaserScale(Zones zone)
    {

        switch (zone)
        {
            case Zones.Zone1:
                laser.SetScale(0.1f);
                break;
            case Zones.Zone2:
                laser.SetScale(1f);
                break;
            case Zones.Zone3:
                laser.SetScale(20f);
                break;
            case Zones.Zone4:
                laser.SetScale(5000f);
                break;
        }
    }
}
