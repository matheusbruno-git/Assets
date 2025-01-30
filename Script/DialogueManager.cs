using System.Collections;
using UnityEngine;
using TMPro;
using StarterAssets;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;
    public bool isDialoguing = false;
    public float detectionRadius = 5f;
    private string[] lines;
    private int index;
    private NPC_Behaviour detectedNPC;

    private StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        textComponent.text = string.Empty;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        DetectNPC();

        if (starterAssetsInputs.attack1 && !isDialoguing && detectedNPC != null)
        {
            StartDialogue();
        }

        if (isDialoguing && starterAssetsInputs.attack1)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    private void DetectNPC()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        detectedNPC = null;

        foreach (var hitCollider in hitColliders)
        {
            NPC_Behaviour NPC = hitCollider.GetComponent<NPC_Behaviour>();

            if (NPC != null)
            {
                detectedNPC = NPC;
                lines = NPC.lines;
                Debug.Log("Detected NPC with NPC_Behaviour script.");
                textComponent.gameObject.SetActive(true);
                textComponent.text = "Press X or Mouse 1 to start conversation";
                break;
            }
        }
    }

    public void StartDialogue()
    {
        isDialoguing = true;
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialoguing = false;
            textComponent.gameObject.SetActive(false);
        }
    }
}