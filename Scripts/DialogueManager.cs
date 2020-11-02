using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;
    [SerializeField] GameObject selectionPanel;
    [SerializeField] Text nextMapNameText;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject startPanel;
    [SerializeField] Text mapNameText;
    [SerializeField] Text playerPoints;
    [SerializeField] GameObject winGamePanel;
    [SerializeField] GameObject timerBorder;
    [SerializeField] Text finalRoundTimer;
    [SerializeField] public GameObject soCloseToWinningPanel;


    private Queue<string> sentences;

    public static DialogueManager instance;

    void Awake()
    {
        sentences = new Queue<string>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        MainMenuPanelToogle();
        WinGamePanelToogle();
        StartGamePanelToogle();
        DisplayFinalRoundTimer();
    }

    private void MainMenuPanelToogle()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            mainMenuPanel.SetActive(false);
        }
    }

    private void WinGamePanelToogle()
    {
        if (SceneManager.GetActiveScene().name == "Win Game")
        {
            winGamePanel.SetActive(true);
        }
        else
        {
            winGamePanel.SetActive(false);
        }
    }

    private void StartGamePanelToogle()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "The U-Turn":
            case "The Asteroid Party":
            case "The Zig Zag":
            case "The Dark":
            case "The Finish Line":
                if (GameManager.instance.GameState == GameManager.State.Preparing && !dialoguePanel.activeInHierarchy)
                {
                    startPanel.SetActive(true);
                    mapNameText.text = SceneManager.GetActiveScene().name;
                }
                else
                {
                    startPanel.SetActive(false);
                }
                break;
            default:
                startPanel.SetActive(false);
                break;
        }
    }

    public GameObject GetDialoguePanel()
    {
        return dialoguePanel;
    }

    public void SetDialoguePanel(GameObject setActive)
    {
        dialoguePanel = setActive;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        ChangeGameState();
    }

    public void ChangeGameState()
    {
        switch (FindObjectOfType<GameManager>().GameState)
        {
            case GameManager.State.Preparing:
                //Set items of all rockets before race
                FindObjectOfType<Rocket>().CheckItem();
                EnemyRocket[] array = FindObjectsOfType<EnemyRocket>();
                foreach (EnemyRocket enemyRocket in array)
                {
                    enemyRocket.CheckItem();
                }
                GameManager.instance.GameState = GameManager.State.Racing;
                break;
            case GameManager.State.Racing:
                GameManager.instance.GameState = GameManager.State.Result;
                break;
            case GameManager.State.Result:
                GameManager.instance.GameState = GameManager.State.Selection;
                resultPanel.SetActive(false);
                selectionPanel.SetActive(true);
                GameManager.instance.ItemAvailability();

                if(RandomMapGenerator.instance.currentMapIndex == RandomMapGenerator.instance.availableMapNames.Length
                    && GameManager.instance.playerPoints > 7)
                {
                    nextMapNameText.text
                        = "POINTS OVERLOADED, GAME OVER.";
                    playerPoints.text = "SCORE: " + GameManager.instance.playerPoints;
                }
                else
                {
                    nextMapNameText.text = "NEXT MAP: " + RandomMapGenerator.instance.GetMapName();
                    playerPoints.text = "SCORE: " + GameManager.instance.playerPoints;
                }
                break;
            case GameManager.State.Selection:
                selectionPanel.SetActive(false);
                GameManager.instance.GameState = GameManager.State.Preparing;
                if (SceneManager.GetActiveScene().name != "Tutorial 2")
                    RandomMapGenerator.instance.LoadNextMap();
                break;
            default:
                break;
        }
    }

    public void DisplayFinalRoundTimer()
    {
        if (SceneManager.GetActiveScene().name == "The Finish Line")
        {
            timerBorder.SetActive(true);

            finalRoundTimer.text = "TIMER: " + Mathf.RoundToInt(GameManager.instance.finalRoundTimer).ToString();

            if (GameManager.instance.finalRoundTimer <= 0)
            {
                finalRoundTimer.text = "TIMER: 0";
            }
        }
        else
        {
            timerBorder.SetActive(false);
        }
    }

    public void DisplayResult()
    {
        resultPanel.SetActive(true);
        switch (FindObjectOfType<Goal>().GetPlacement)
        {
            case 1:
                resultText.text = "1ST PLACE!";
                break;
            case 2:
                resultText.text = "2ND PLACE!";
                break;
            case 3:
                resultText.text = "3RD PLACE!";
                break;
            case 4:
                resultText.text = "4TH PLACE!";
                break;
            default:
                break;
        }

        if (SceneManager.GetActiveScene().name == "Tutorial 2")
        {
            GameObject.Find("TutorialDialogueTrigger2").GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}
