using UnityEngine;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour
{
    public Text interactionUI;
    public Text secondaryInteractionUI;
    public string[] interactionPrompts;
    public string[] secondaryInteractionPrompts =   { "Hold [E] Grab", "" };
    public AudioSource target;
    public Transform tr;
    public LayerMask layerMask;
    public float pitchCount = 0.02f;
    public float pitchTime = 0.02f;
    public bool turningOn = false;
    public bool shouldUpdate = false;
    public bool shouldUpdateToInverted = false;

    private void Awake()
    {
        tr = transform;
        interactionPrompts = new string[2];
        interactionPrompts[0] = "[E] On/Off";
        interactionPrompts[1] = "[E] Open";

        secondaryInteractionPrompts[0] = "Hold [E] Grab";
        secondaryInteractionPrompts[1] = "";
    }

    // Update is called once per frame
    void Update()
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
                    target.pitch += 0.07f;
                    if (target.pitch >= 1)
                    {
                        shouldUpdate = false;
                        target.pitch = 1;
                    }
                }
                else
                {
                    target.pitch -= 0.07f;
                    if (target.pitch <= 0)
                    {
                        shouldUpdate = false;
                        target.pitch = 0;
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
                    target.pitch += 0.07f;
                    if (target.pitch >= 1)
                    {
                        shouldUpdateToInverted = false;
                        target.pitch = 1;
                    }
                }
                else
                {
                    target.pitch -= 0.07f;
                    if (target.pitch <= -1)
                    {
                        shouldUpdateToInverted = false;
                        target.pitch = -1;
                    }
                }
            }
        }

        RaycastHit HitInfo;
        if (Physics.Raycast(tr.position, tr.forward, out HitInfo, 2.5f, layerMask))
        {
            // LOOKING AT INTERACTABLE
            //Debug.DrawRay(tr.position, tr.forward * 2.5f, Color.green, 0.1f);
            interactionUI.gameObject.SetActive(true);
            secondaryInteractionUI.gameObject.SetActive(true);
            Debug.Log(interactionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID] + " -- " + HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID);
            interactionUI.text = interactionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID];
            secondaryInteractionUI.text = secondaryInteractionPrompts[HitInfo.collider.gameObject.GetComponent<InteractableScript>().interactionID];
            if (Input.GetKeyDown(KeyCode.E))
            {
                target = HitInfo.collider.gameObject.GetComponent<AudioSource>();
                if (target.pitch > 0.5f)
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
                    //target.Pause();
                }
                else
                {
                    shouldUpdate = true;
                    turningOn = true;
                    //target.Play();
                }
            }
        }
        else
        {
            // NOT LOOKING
            //Debug.DrawRay(tr.position, tr.forward * 2.5f, Color.yellow, 0.1f);
            interactionUI.gameObject.SetActive(false);
            secondaryInteractionUI.gameObject.SetActive(false);
        }
    }

    void ShowUI()
    {
        interactionUI.gameObject.SetActive(true);
    }

    void HideUI()
    {
        interactionUI.gameObject.SetActive(false);
    }
}
