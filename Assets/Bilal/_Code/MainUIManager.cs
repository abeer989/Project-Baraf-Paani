using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public TitleUIManager titleUIController;
    public LobbyUIManager lobbyUIController;


    public void OnStartEvent()
    {


        titleUIController.FadeOutPanel();
        lobbyUIController.FadeInPanel();
    }

    public void OnLeadboardEvent()
    {
        //TODO
    }


    
    

}
