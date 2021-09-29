using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ZoomManager : MyMonoBehaviour
{

    public event Action<Zones> OnZoom;
    public Zones currenZone;
    private Zones prevZone;

    [SerializeField] Transform[] cameraPivotZones = new Transform[4];
    [SerializeField] AudioSource zoomSound;
    [SerializeField] Ease easeTypeMove;
    [SerializeField] Ease easeTypeRotation;
    [SerializeField] float zoomDuration;

    private Transform cameraTransform;


    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        prevZone = currenZone;
    }

    private void Update()
    {
        if (manager.unityMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                DoZoomTo(Zones.Zone1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                DoZoomTo(Zones.Zone2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                DoZoomTo(Zones.Zone3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                DoZoomTo(Zones.Zone4);
        }
    }
    public void DoZoomTo (Zones newZone)
    {
        if (newZone == currenZone)
            return;

        prevZone = currenZone;
        currenZone = newZone;
        zoomSound.Play();
        cameraTransform.DOMove(cameraPivotZones[(int)newZone].transform.position, zoomDuration).SetEase(easeTypeMove);
        cameraTransform.DORotate(cameraPivotZones[(int)newZone].transform.rotation.eulerAngles, zoomDuration).SetEase(easeTypeRotation);

        if (OnZoom != null)
            Invoke("ZoomEvent", zoomDuration);
    }

    public void DoZoomTo(Transform newPosition, float time)
    {
        cameraTransform.DOMove(newPosition.position+Vector3.up, time).SetEase(Ease.Linear);
        cameraTransform.DORotate(newPosition.rotation.eulerAngles, time).SetEase(Ease.InExpo);
    }

        public void ZoomBack()
    {
        currenZone = prevZone;
        zoomSound.Play();
        cameraTransform.DOMove(cameraPivotZones[(int)currenZone].transform.position, zoomDuration).SetEase(easeTypeMove);
        cameraTransform.DORotate(cameraPivotZones[(int)currenZone].transform.rotation.eulerAngles, zoomDuration).SetEase(easeTypeRotation);

        if (OnZoom != null)
            Invoke("ZoomEvent", zoomDuration);
    }

    private void ZoomEvent()
    {
        OnZoom(currenZone);
    }
}
