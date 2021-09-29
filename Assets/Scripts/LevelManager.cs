using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Result
[System.Serializable]
public class Result
{
    public string nameResult;
    public List<Actors> actors = new List<Actors>();
}
#endregion

public class LevelManager : MyMonoBehaviour
{
    [SerializeField] AudioSource wellDoneSound;

    [SerializeField] List<Result> resultsLevel1;
    [SerializeField] List<Result> resultsLevel2;
    [SerializeField] List<Result> resultsLevel3;
    [SerializeField] List<Result> resultsLevel4;


    //TODO!!<<<<<<<<<<<<<<<<<
    int countOfSameActor;

    //Current Try
    List <Actor> currentActorsList = new List<Actor>();

    //Historial...
    internal List<Actor> fullActorsList = new List<Actor>();

    public event Action<string> OnResultCompleted;

    int completedResultsCount = 0;

    //Levels
    public Levels currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        Actor.OnActorSelected += EvaluateActor;

        //Init Zoom
        zoom.DoZoomTo(GetZoneLevel(currentLevel));
    }
    private void OnDestroy()
    {
        Actor.OnActorSelected -= EvaluateActor;
        
    }


    //TODO: Mover a ResultsController

    #region ResultsController
    private void EvaluateActor(Actor newActor)
    {
        //First Element Result
        if (currentActorsList.Count == 0 || manager.unityMode)
        {
            currentActorsList.Add(newActor);
            laser.AddLaserPoint(newActor);
         
            //Zoom Actor
            if (newActor.zoomActor && !manager.unityMode)
                zoom.DoZoomTo(newActor.zoneToZoom);

            return;
        }


        //Next Elements Results
        List<Result> results = GetResultsCurrentLevel();


        if (IsValidResult(newActor, results))
        {

            //Add actor to the pile
            currentActorsList.Add(newActor);

            //ZoomBack
            if (currentActorsList.Where(x => x.zoomActor).ToList().Count > 0 && newActor.actorType != Actors.Nube)
                zoom.ZoomBack();

            //Zoom Actor
            if (newActor.zoomActor)
                zoom.DoZoomTo(newActor.zoneToZoom);

            laser.AddLaserPoint(newActor);
        }
        else
        {
            //ZoomBack
            if (currentActorsList.Where(x => x.zoomActor).ToList().Count > 0 && newActor.actorType != Actors.Nube)
                zoom.ZoomBack();

            laser.ResetLaser();
        }
    }

    private bool IsValidResult(Actor newActor, List<Result>results)
    {
        if (currentActorsList.Contains(newActor) && newActor.actorType == Actors.Tierra)
            return false;

        if (currentActorsList.Contains(newActor) )
            return true;

        #region Still Valid results
        List<Result> stillValidResults = new List<Result>();

        foreach (Result result in results)
        {
            bool validResult = true;

            foreach(Actor actor in currentActorsList)
            {
                if (!result.actors.Contains(actor.actorType))
                    validResult = false;
            }


            if (validResult)
                stillValidResults.Add(result);
        }
        #endregion

        #region Valid Actor in current Actors result && RESULT COMPLETED control
        foreach (Result result in stillValidResults)
        {
            //Valid Actor
            if (!result.actors.Contains(newActor.actorType))
                continue;

            #region Result Completed
            List<Actors> actorsInResult = new List<Actors>();
            actorsInResult.Add(newActor.actorType);
            foreach (Actor currentActor in currentActorsList)
            {
                if (result.actors.Contains(currentActor.actorType) && !actorsInResult.Contains(currentActor.actorType))
                {
                    actorsInResult.Add(currentActor.actorType);
                }
            }

            if (actorsInResult.Count == result.actors.Count)
            {
                ResultCompleted(result.nameResult);
                RemoveResult(result);
                return false;
            }

            #endregion

            return true;

        }
        #endregion

        return false;
    }

    internal void ResetSecuence()
    {

        currentActorsList.Clear();
    }

    private void RemoveResult(Result result)
    {
        switch (currentLevel)
        {
            case Levels.Level1:
                resultsLevel1.Remove(result);
                break;
            case Levels.Level2:
                resultsLevel2.Remove(result);
                break;
            case Levels.Level3:
                resultsLevel3.Remove(result);
                break;
            case Levels.Level4:
                resultsLevel4.Remove(result);
                break;
        }
    }

    void ResultCompleted(string resultName)
    {
        
        completedResultsCount++;
        
        //Historial...
        fullActorsList.AddRange(currentActorsList);

        //ZoomBack
        if (currentActorsList.Where(x => x.zoomActor).ToList().Count > 0)
        {
            zoom.ZoomBack();
            //PARCHEEEE
            if (currentLevel == Levels.Level2)
                foreach (Actor actor in currentActorsList)
                    actor.zoomActor = false;
        }

        //Event
        if (OnResultCompleted != null)
            OnResultCompleted(resultName);


        //CHANGE LEVEL!
        ChangeLevelControl(resultName);

        currentActorsList.Clear();
        countOfSameActor = 0;

        //Like con n IconMouse!!!!s
        manager.textUI.SendLike();

        //Sound!!
        wellDoneSound.Play();

    }
    #endregion

    private void ChangeLevelControl(string resultName)
    {
        if (currentLevel == Levels.Level1 && completedResultsCount == 4)
            ChangeLevel(Levels.Level2);

        if (currentLevel == Levels.Level2 && completedResultsCount == 6)
            ChangeLevel(Levels.Level3);

        if (currentLevel == Levels.Level3 && completedResultsCount == 7)
            ChangeLevel(Levels.Level4);

        if (currentLevel == Levels.Level4 && completedResultsCount == 9)
            ChangeLevel(Levels.Credits);
    }

    internal void ChangeLevel(Levels newLevel)
    {
        currentLevel = newLevel;

        audio.PlayMusic(newLevel);

        if (newLevel == Levels.Credits)
            StartCoroutine(credits.StartCreditsSecuence());
        else
            zoom.DoZoomTo(GetZoneLevel(newLevel));

        if (newLevel == Levels.Level1 || newLevel == Levels.Level4)
            audio.PlayVoice(newLevel);
    }

    private List<Result> GetResultsCurrentLevel()
    {
        switch (currentLevel)
        {
            case Levels.Level1:
                return resultsLevel1;
            case Levels.Level2:
                return resultsLevel2;
            case Levels.Level3:
                return resultsLevel3;
        }
        return resultsLevel4;
    }



    private Zones GetZoneLevel(Levels currentLevel)
    {

        switch (currentLevel)
        {
            case Levels.Level2:
                return Zones.Zone2;
            case Levels.Level3:
                return Zones.Zone3;
            case Levels.Level4:
                return Zones.Zone4;
        }

        return Zones.Zone1;
    }
}
