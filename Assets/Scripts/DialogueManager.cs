using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [System.Serializable]
    public class Dialogue
    {
        public string dialogueName;

        [TextArea(2, 5)]
        public List<string> lines = new List<string>();
    }

    [Header("Dialogue Database")]
    [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();

    [Header("UI")]
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typewriter")]
    [SerializeField] private float charactersPerSecond = 40f;

    private Dialogue currentDialogue;
    private int currentLineIndex;

    private Coroutine typewriterCoroutine;

    private bool isTyping;
    private bool dialogueFinished;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Hide dialogue when the game starts
        dialogueContainer.SetActive(false);
    }

    private void Update()
    {
        //TEST DIALOGUE
        if (Input.GetKeyDown(KeyCode.P))
        {
            DialogueManager.Instance.Play("Hola");
        }
        //TEST DIALOGUE

        if (!dialogueContainer.activeSelf)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    // =========================================================
    // PUBLIC METHODS
    // =========================================================

    public void Play(string dialogueName)
    {
        Dialogue dialogue = dialogues.Find(
            dialogue => dialogue.dialogueName == dialogueName
        );

        if (dialogue == null)
        {
            Debug.LogWarning(
                $"Dialogue '{dialogueName}' was not found."
            );

            return;
        }

        Play(dialogue);
    }

    public void Play(int dialogueIndex)
    {
        if (dialogueIndex < 0 || dialogueIndex >= dialogues.Count)
        {
            Debug.LogWarning(
                $"Dialogue index {dialogueIndex} is out of range."
            );

            return;
        }

        Play(dialogues[dialogueIndex]);
    }

    public void Stop()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        currentDialogue = null;

        isTyping = false;
        dialogueFinished = false;

        dialogueContainer.SetActive(false);
    }

    public bool IsPlaying()
    {
        return currentDialogue != null;
    }

    // =========================================================
    // DIALOGUE
    // =========================================================

    private void Play(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.lines.Count == 0)
            return;

        // Stop any dialogue currently playing
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;

        dialogueFinished = false;

        dialogueContainer.SetActive(true);

        typewriterCoroutine = StartCoroutine(ShowLine());
    }

    private void NextLine()
    {
        currentLineIndex++;

        // Dialogue finished
        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            Stop();
            return;
        }

        typewriterCoroutine = StartCoroutine(ShowLine());
    }

    // =========================================================
    // TYPEWRITER
    // =========================================================

    private IEnumerator ShowLine()
    {
        isTyping = true;
        dialogueFinished = false;

        dialogueText.text = "";

        string line = currentDialogue.lines[currentLineIndex];

        foreach (char character in line)
        {
            dialogueText.text += character;

            float delay = 1f / charactersPerSecond;

            yield return new WaitForSecondsRealtime(delay);
        }

        isTyping = false;
        dialogueFinished = true;

        typewriterCoroutine = null;
    }

    private void SkipTypewriter()
    {
        if (currentDialogue == null)
            return;

        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        dialogueText.text =
            currentDialogue.lines[currentLineIndex];

        isTyping = false;
        dialogueFinished = true;
    }

    // =========================================================
    // INPUT
    // =========================================================

    private void HandleInput()
    {
        // First click while typing:
        // instantly show the complete line.
        if (isTyping)
        {
            SkipTypewriter();
            return;
        }

        // Second click:
        // go to the next line.
        if (dialogueFinished)
        {
            NextLine();
        }
    }
}