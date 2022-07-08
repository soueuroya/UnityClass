using UnityEngine;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour
{
    public Text primaryInteractionUI;
    public Text secondaryInteractionUI;
    public string[] interactionPrompts;
    public string[] secondaryInteractionPrompts;
    
    public InteractionHandler.InteractionType primaryInteraction; // button press
    public InteractionHandler.InteractionType secondaryInteraction; // button hold

    public Transform grabbingLocation;
    public AudioSource targetAudio;
    public Animator targetAnimator;
    public Rigidbody targetRigidbody;
    public GameObject targetObject;
    public Transform tr;
    public LayerMask layerMask;
    public float pitchCount = 0.02f;
    public float pitchTime = 0.02f;
    public bool turningOn = false;
    public bool shouldUpdate = false;
    public bool shouldUpdateToInverted = false;
    public bool isHolding;
    public KeyCode primary;
    public KeyCode secondary;

    private void Awake()
    {
        tr = transform;
        interactionPrompts = new string[2];
        interactionPrompts[0] = "["+primary.ToString()+"] On/Off"; // radio
        interactionPrompts[1] = "["+primary.ToString()+"] Open/Close"; // door

        secondaryInteractionPrompts[0] = "["+ secondary.ToString() + "] Grab"; // radio
        secondaryInteractionPrompts[1] = ""; // door
    }

    // Update is called once per frame
    void Update()
    {
        if (targetAudio != null)
        {
            if (shouldUpdate)
            {
                if (pitchCount < pitchTime)
                {
                    pitchCount += Time.deltaTime;
                }
                else
                {
                    pitchCount = 0;
                    if (turningOn)
                    {
                        targetAudio.pitch += 0.07f;
                        if (targetAudio.pitch >= 1)
                        {
                            shouldUpdate = false;
                            targetAudio.pitch = 1;
                        }
                    }
                    else
                    {
                        targetAudio.pitch -= 0.07f;
                        if (targetAudio.pitch <= 0)
                        {
                            shouldUpdate = false;
                            targetAudio.pitch = 0;
                        }
                    }
                }
            }
            if (shouldUpdateToInverted)
            {
                if (pitchCount < pitchTime)
                {
                    pitchCount += Time.deltaTime;
                }
                else
                {
                    pitchCount = 0;
                    if (turningOn)
                    {
                        targetAudio.pitch += 0.07f;
                        if (targetAudio.pitch >= 1)
                        {
                            shouldUpdateToInverted = false;
                            targetAudio.pitch = 1;
                        }
                    }
                    else
                    {
                        targetAudio.pitch -= 0.07f;
                        if (targetAudio.pitch <= -1)
                        {
                            shouldUpdateToInverted = false;
                            targetAudio.pitch = -1;
                        }
                    }
                }
            }
        }
        
        if (isHolding)
        { 
            if (Input.GetKeyDown(secondary))
            {
                targetObject.transform.parent = null;
                targetObject.GetComponent<Rigidbody>().isKinematic = false;
                targetObject.GetComponent<BoxCollider>().enabled = true;
                isHolding = false;   
                targetObject = null;
            }
        }

        RaycastHit HitInfo;
        if (Physics.Raycast(tr.position, tr.forward, out HitInfo, 2.5f, layerMask))
        {
            Debug.Log(interactionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID] + " -- " + HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID);
            targetObject = HitInfo.collider.gameObject;
            primaryInteraction = HitInfo.collider.gameObject.GetComponent<InteractableScript>().primaryInteraction;
            secondaryInteraction = HitInfo.collider.gameObject.GetComponent<InteractableScript>().secondaryInteraction;

            if (!primaryInteraction.Equals(InteractionHandler.InteractionType.NONE))
            {
                primaryInteractionUI.gameObject.SetActive(true);
                primaryInteractionUI.text = interactionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID];
            }
            else
            {
                primaryInteractionUI.gameObject.SetActive(false);
            }

            if (!secondaryInteraction.Equals(InteractionHandler.InteractionType.NONE))
            {
                secondaryInteractionUI.gameObject.SetActive(true);
                secondaryInteractionUI.text = secondaryInteractionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID];
            }
            else
            {
                secondaryInteractionUI.gameObject.SetActive(false);
            }
            
            if (Input.GetKeyDown(primary))
            {
                switch (primaryInteraction)
                {
                    case InteractionHandler.InteractionType.AUDIO:
                        AudioInteraction(HitInfo);
                        break;
                    case InteractionHandler.InteractionType.OPEN:
                        OpenInteraction(HitInfo);
                        break;
                    case InteractionHandler.InteractionType.GRAB:
                        GrabInteraction(HitInfo);
                        break;
                    default:
                        break;
                }   
            }

            if (Input.GetKeyDown(secondary))
            {
                switch (secondaryInteraction)
                {
                    case InteractionHandler.InteractionType.AUDIO:
                        AudioInteraction(HitInfo);
                        break;
                    case InteractionHandler.InteractionType.OPEN:
                        OpenInteraction(HitInfo);
                        break;
                    case InteractionHandler.InteractionType.GRAB:
                        GrabInteraction(HitInfo);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            // NOT LOOKING
            //Debug.DrawRay(tr.position, tr.forward * 2.5f, Color.yellow, 0.1f);
            primaryInteractionUI.gameObject.SetActive(false);
            secondaryInteractionUI.gameObject.SetActive(false);
        }
    }
    public void OpenInteraction(RaycastHit HitInfo)
    {
        targetAnimator = HitInfo.collider.gameObject.GetComponent<Animator>();
        targetAnimator.SetTrigger("Interacted");
    }

    public void GrabInteraction(RaycastHit HitInfo)
    {
        if (!isHolding)
        {
            HitInfo.collider.gameObject.transform.position = grabbingLocation.position;
            HitInfo.collider.gameObject.transform.parent = transform;
            HitInfo.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            HitInfo.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
            isHolding = true;
        }
    }

    public void AudioInteraction(RaycastHit HitInfo)
    {
        targetAudio = HitInfo.collider.gameObject.GetComponent<AudioSource>();
        if (targetAudio.pitch > 0.5f)
        {
            turningOn = false;
            if (Random.Range(0, 51) == 0)
            {
                shouldUpdateToInverted = true;
            }
            else
            {
                shouldUpdate = true;
            }
        }
        else
        {
            shouldUpdate = true;
            turningOn = true;
        }
    }

    void ShowUI()
    {
        primaryInteractionUI.gameObject.SetActive(true);
    }

    void HideUI()
    {
        primaryInteractionUI.gameObject.SetActive(false);
    }
}
