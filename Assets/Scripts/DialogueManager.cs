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

        [Header("Time Between Lines")]
        public float timeBetweenLines = 2f;
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

    private Coroutine dialogueCoroutine;

    private bool isTyping;
    private bool dialogueFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        dialogueContainer.SetActive(false);
    }

    private void Update()
    {
        // TEST DIALOGUE
        if (Input.GetKeyDown(KeyCode.P))
        {
            DialogueManager.Instance.Play("Hola");
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
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
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

        // Stop current dialogue if there is one
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;

        dialogueFinished = false;

        dialogueContainer.SetActive(true);

        dialogueCoroutine = StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        while (currentLineIndex < currentDialogue.lines.Count)
        {
            // Show current line
            yield return StartCoroutine(ShowLine());

            // Wait after the line has finished typing
            yield return new WaitForSecondsRealtime(
                currentDialogue.timeBetweenLines
            );

            currentLineIndex++;
        }

        Stop();
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
    }
}