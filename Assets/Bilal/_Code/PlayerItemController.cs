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

    public System.Action onCharacterChanged;

    ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();

    public Sprite[] avatars;


    string playerAvatarKey = "playerAvatar";

    public Player player;

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

        string playerName = _player.IsMasterClient ? $"{_player.NickName}(Host)" : $"{_player.NickName}";

        SetNameText(playerName);
        UpdatePlayerItem(player);
        PhotonNetwork.SetPlayerCustomProperties(playerProps);
    }

    public void ApplyLocalChanges()
    {
        leftArrowButton.gameObject.SetActive(true);
        rightArrowButton.gameObject.SetActive(true);
        backgroundImage.color = highlightColor;
    }

    public void OnClickLeftArrow()
    {

        Debug.Log(" OnClickLeftArrow()");

        if((int)playerProps[playerAvatarKey] == 0)
        {
            playerProps[playerAvatarKey] = avatars.Length - 1;
        }
        else
        {
            playerProps[playerAvatarKey] = (int)playerProps[playerAvatarKey] - 1;
        }

       PhotonNetwork.SetPlayerCustomProperties(playerProps);

    }

    public void OnClickRightArrow()
    {

        Debug.Log("OnClickRightArrow() ");

        if ((int)playerProps[playerAvatarKey] == avatars.Length-1)
        {
            playerProps[playerAvatarKey] = 0;
        }
        else
        {
            playerProps[playerAvatarKey] = (int)playerProps[playerAvatarKey] + 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProps);

    }

    void UpdatePlayerItem(Player player)
    {
        

        if(player.CustomProperties.ContainsKey(playerAvatarKey))
        {
            int index = (int)player.CustomProperties[playerAvatarKey];
            img_PlayerAvatar.sprite = avatars[index];
            
            playerProps[playerAvatarKey] = index;

            if(player == this.player)
                onCharacterChanged?.Invoke();
        }
        else
        {
            playerProps[playerAvatarKey] = 0;

            if (player == this.player)
                onCharacterChanged?.Invoke();

        }
        //PhotonNetwork.SetPlayerCustomProperties(playerProps);
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(changedProps.ContainsKey("playerAvatar"))
        {
            if (player == targetPlayer)
            {
                UpdatePlayerItem(player);
                // playerProps[playerAvatarKey] = (int)player.CustomProperties[playerAvatarKey];
            }

        }

        



    }


}
