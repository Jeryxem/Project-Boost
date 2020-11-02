using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State { Preparing, Racing, Result, Selection }

    [SerializeField] State currentState;
    [SerializeField] public string player, enemy1, enemy2, enemy3 = null;
    [SerializeField] GameObject itemSpeedUp, itemShield, itemTeleport, itemLights, itemRespawn;
    public int playerPoints = 0;

    public float finalRoundTimer = 60f;

    public static GameManager instance;

    void Awake()
    {
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

    void Start()
    {
        currentState = State.Preparing;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "The Finish Line" && GameState == State.Racing)
        {
            finalRoundTimer -= Time.deltaTime;
        }
        else
        {
            finalRoundTimer = 60f;
        }
    }

    public State GameState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial 1");
    }

    public void LoadTutorial2()
    {
        SceneManager.LoadScene("Tutorial 2");
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene("Win Game");
    }


    public void SelectItem(string itemName)
    {
        if (FindObjectOfType<Goal>().GetPlacement == 1)
        {
            player = itemName;
        }

        if (SceneManager.GetActiveScene().name == "Tutorial 2")
        {
            player = null;
            playerPoints = 0;
            SceneManager.LoadScene("Main Menu");
        }

        DialogueManager.instance.ChangeGameState();
    }

    public void ItemAvailability()
    {
        itemSpeedUp.SetActive(true);
        itemShield.SetActive(true);
        itemTeleport.SetActive(true);
        itemLights.SetActive(true);
        itemRespawn.SetActive(true);

        DisableUnavailableItems();
    }

    private void DisableUnavailableItems()
    {
        string temp = null;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    temp = player;
                    break;
                case 1:
                    temp = enemy1;
                    break;
                case 2:
                    temp = enemy2;
                    break;
                case 3:
                    temp = enemy3;
                    break;
                default:
                    break;
            }

            switch (temp)
            {
                case "SpeedUp":
                    itemSpeedUp.SetActive(false);
                    break;
                case "Shield":
                    itemShield.SetActive(false);
                    break;
                case "Click Teleport":
                    itemTeleport.SetActive(false);
                    break;
                case "Lights":
                    itemLights.SetActive(false);
                    break;
                case "Fast Respawn":
                    itemRespawn.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    public string SelectItemEnemy(string player, string enemy1, string enemy2, string enemy3 )
    {
        if (FindObjectOfType<Goal>().GetPlacement != 1)
            return null;

        List<string> items = 
            new List<string> { "SpeedUp", "Shield", "Click Teleport", "Lights", "Fast Respawn" };

        if(player != null)
            items.Remove(player);

        if (enemy1 != null)
            items.Remove(enemy1);

        if (enemy2 != null)
            items.Remove(enemy2);

        if (enemy3 != null)
            items.Remove(enemy3);

        int random = Random.Range(0, items.Count());

        string chosenItem = items[random];

        return chosenItem;
    }
}
