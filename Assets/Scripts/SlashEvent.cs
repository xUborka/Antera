using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEvent : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public void Slash(){
        playerMovement.Slash();
    }
}
