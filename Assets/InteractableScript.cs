using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public int interactionID; // 0 - audio
    public InteractionHandler.InteractionType primaryInteraction;
    public InteractionHandler.InteractionType secondaryInteraction;
}
