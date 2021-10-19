using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
public class Startup : MonoBehaviour
{
    [SerializeField] public float DelayStartup = 20f;
    [SerializeField] public float Timer = 60f;
    [SerializeField] public GameObject Camera;
    [SerializeField] public Character Character;
    [SerializeField] public TMP_Text TimerText;
    [SerializeField] public TMP_Text Win;
    [SerializeField] public TMP_Text Loss;
    [SerializeField] public TMP_Text GunsLeft;
    [SerializeField] public TMP_Text Objective;


    public bool isStarted { get; private set; }

    void Start()
    {
        Win.gameObject.SetActive(false);
        Loss.gameObject.SetActive(false);
        TimerText.gameObject.SetActive(false);
        GunsLeft.gameObject.SetActive(false);
        Objective.gameObject.SetActive(true);
    //    TimerText.
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > DelayStartup && !isStarted)
        {
            isStarted = true;
            Camera.GetComponent<Camera>().orthographicSize = 5;
            Camera.GetComponent<CameraView>()._target = Character.transform;
            Camera.GetComponent<PostProcessVolume>().enabled = true;
            Objective.gameObject.SetActive(false);
            TimerText.gameObject.SetActive(true);
            GunsLeft.gameObject.SetActive(true);
            

            GunsLeft.text = "Guns left: " + System.Convert.ToString(Character.GunsLeft());
            return;
        }

        float timeLeft = Timer - Time.realtimeSinceStartup + DelayStartup;

        string minutes = System.Convert.ToString(Mathf.RoundToInt(timeLeft) / 60);
        string seconds = System.Convert.ToString(Mathf.RoundToInt(timeLeft) % 60);
        string millis = System.Convert.ToString(Mathf.RoundToInt(timeLeft * 1000) % 1000);
        // Add following zeroes
        for (int i = 0; i < 2 - minutes.Length; i++)
        {
            minutes = "0" + minutes;
        }
        for (int i = 0; i < 2 - seconds.Length; i++)
        {
            seconds = "0" + seconds;
        }



        if (timeLeft <= 0 || Character.GunsLeft() == 0)
        {
            bool won = true;
            if(Character.GunsLeft() != 0)
            {
                won = false;
            }
            Win.gameObject.SetActive(won);
            Loss.gameObject.SetActive(!won);
            GunsLeft.gameObject.SetActive(false);

            return;
        }

        if (Time.realtimeSinceStartup > DelayStartup)
        {
            TimerText.text = "Time left: " + minutes + ":" + seconds;
            if(timeLeft < 15)
            {
                TimerText.text += "." + millis;
                float val = 0.9f + Mathf.Sin(Time.realtimeSinceStartup) * 0.1f ;
                TimerText.gameObject.transform.localScale = new Vector3(val, val);
                TimerText.color = new UnityEngine.Color(0.6f + 0.4f * (15 - timeLeft) / 15, 0.06657172f, 0.06657172f);
            }
            GunsLeft.text = System.Convert.ToString(Character.GunsLeft()) + " guns left";
        }
    }
}
