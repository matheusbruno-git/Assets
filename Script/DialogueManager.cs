using System.Collections;
using UnityEngine;
using TMPro;
using StarterAssets;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    private float textSpeed = 0.05f;
    [SerializeField]
    private float detectionRadius = 5f;

    private bool isDialoguing;
    private int currentIndex;
    private string[] lines;
    private NPC_Behaviour detectedNPC;

    private StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        textComponent.text = string.Empty;
        textComponent.gameObject.SetActive(false);
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
            if (textComponent.text == lines[currentIndex])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[currentIndex];
            }
        }
    }

    private void DetectNPC()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        detectedNPC = null;

        foreach (var hitCollider in hitColliders)
        {
            NPC_Behaviour npc = hitCollider.GetComponent<NPC_Behaviour>();

            if (npc != null)
            {
                detectedNPC = npc;
                lines = npc.lines;
                Debug.Log("Detected NPC with NPC_Behaviour script.");
                textComponent.gameObject.SetActive(true);
                textComponent.text = "Press X or Mouse 1 to start conversation";
                break;
            }
        }
    }

    public void StartDialogue()
    {
        textComponent.gameObject.SetActive(true);
        isDialoguing = true;
        currentIndex = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[currentIndex].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (currentIndex < lines.Length - 1)
        {
            currentIndex++;
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
