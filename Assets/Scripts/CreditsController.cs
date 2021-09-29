using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsController : MyMonoBehaviour
{
    [TextArea]
    [SerializeField] string credits;
    TextMeshProUGUI creditsTxt;

    // Start is called before the first frame update
    void Start()
    {
        creditsTxt = GetComponent<TextMeshProUGUI>();    
    }

    internal IEnumerator StartCreditsSecuence()
    {
        audio.CancelVoices();
        manager.textUI.gameObject.SetActive(false);
        string [] lines = credits.Split('\n');
        creditsTxt.text = lines[0];

        List<Actor> travel = levels.fullActorsList;
        float pauseTime = 90 / lines.Length;
        for (int i = 0, j = 1; j < lines.Length; i++, j++)
        {
            yield return new WaitForSeconds(pauseTime);
            creditsTxt.text = lines[i];
            zoom.DoZoomTo(travel[i].transform, pauseTime);
        }
        yield return new WaitForSeconds(pauseTime);
        creditsTxt.text = "";
        manager.textUI.gameObject.SetActive(true);

        manager.unityMode = true;
        audio.PlayFinalVoice();
        levels.ChangeLevel(Levels.UnityMode);
        laser.ResetAllLasers();
    }
}
