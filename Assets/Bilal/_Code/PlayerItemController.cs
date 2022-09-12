using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItemController : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] Image img_PlayerAvatar;

    [SerializeField] Image backgroundImage;
    public Color highlightColor;

    public Button leftArrowButton;
    public Button rightArrowButton;


    ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();

    public Sprite[] avatars;


    Player player;

    private void Start()
    {
        leftArrowButton.onClick.AddListener(OnClickLeftArrow);
        rightArrowButton.onClick.AddListener(OnClickRightArrow);
    }

    public void SetNameText(string txt)
    {
        playerNameTxt.text = txt;
    }

    public void SetPlayerImage(Sprite img)
    {
        img_PlayerAvatar.sprite = img;
    }

    public void SetPlayerInfo(Player _player)
    {
        player = _player;
        SetNameText(_player.NickName);
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        leftArrowButton.gameObject.SetActive(true);
        rightArrowButton.gameObject.SetActive(true);
        backgroundImage.color = highlightColor;
    }

    public void OnClickLeftArrow()
    {
        if((int)playerProps["playerAvatar"] == 0)
        {
            playerProps["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProps["playerAvatar"] = (int)playerProps["playerAvatar"] - 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProps);

    }

    public void OnClickRightArrow()
    {
        if ((int)playerProps["playerAvatar"] == avatars.Length-1)
        {
            playerProps["playerAvatar"] = 0;
        }
        else
        {
            playerProps["playerAvatar"] = (int)playerProps["playerAvatar"] + 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProps);

    }

    void UpdatePlayerItem(Player player)
    {
        if(player.CustomProperties.ContainsKey("playerAvatar"))
        {
            int index = (int)player.CustomProperties["playerAvatar"];

            img_PlayerAvatar.sprite = avatars[index];
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
       if(player == targetPlayer)
        {
            UpdatePlayerItem(player);
            playerProps["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
       else
        {
            playerProps["playerAVatar"] = 0;
        }
    }


}
