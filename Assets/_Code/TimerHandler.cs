using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;


public class TimerHandler : MonoBehaviour
{
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 20;
    ExitGames.Client.Photon.Hashtable CustomeValue;

    public Action onTimerFinishedEvent;

    void Start()
    {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    CustomeValue = new ExitGames.Client.Photon.Hashtable();
        //    startTime = PhotonNetwork.Time;
        //    startTimer = true;
        //    CustomeValue.Add("StartTime", startTime);
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        //}
        //else
        //{
        //    startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        //    startTimer = true;
        //}
    }

    public void InitializeTimer(double photonTime)
    {
        startTime = photonTime;
        startTimer = true;
    }

    public void StopTimer()
    {
        startTimer = false;
        GameManager_Bilal.instance.uiManagerInstance.ResetTimerTxt();
    }

    void Update()
    {

        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.Time - startTime;

        var t = timer - timerIncrementValue;

        Debug.Log($"Time-> {t} = {timer} - {timerIncrementValue}");

        GameManager_Bilal.instance.uiManagerInstance.SetTimerText((int)t);

        if (timerIncrementValue >= timer)
        {
            Debug.Log("Timer Finished");

            onTimerFinishedEvent?.Invoke();

            //Timer Completed
            //Do What Ever You What to Do Here
        }
    }

   
}
