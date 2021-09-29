using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MyMonoBehaviour
{
    public Actors actorType;
    public static event Action<Actor> OnActorSelected;
    public bool zoomActor;
    public Zones zoneToZoom;

    private void Start()
    {
    }

    private void OnMouseUp()
    {
        if (OnActorSelected != null && levels.currentLevel != Levels.Credits && Time.timeScale > 0)
            OnActorSelected(this);
    }

    private void OnMouseEnter()
    {
        label.SetText(actorType.ToString());
    }
    private void OnMouseExit()
    {
        label.SetText("");
    }
}
