using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] ParticleSystem qualifyExplotion;

    public string name;
    public GameObject parent = null;

    [SerializeField] GameObject lightItem;
    [SerializeField] GameObject shieldItem;
    float shieldTimer = 0;
    [SerializeField] bool clickTeleportActive = false;
    [SerializeField] float turnSpeed = 200f;
    [SerializeField] float moveSpeed = 250f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        CheckItem();
    }

    void Update()
    {
        ProcessInput();
        ShieldItemTimer();
        ClickTeleport();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                break;
            case "Finish":
                ReachGoal();
                break;
            default:
                if (shieldItem.activeInHierarchy)
                    return;
                Destroy();
                break;
        }
    }

    private void ReachGoal()
    {
        AudioManager.instance.Play("Goal");

        if (SceneManager.GetActiveScene().name == "Tutorial 1")
        {
            GameManager.instance.GameState = GameManager.State.Preparing;
            GameManager.instance.Invoke("LoadTutorial2", 1f);
        }
        else if (SceneManager.GetActiveScene().name == "The Finish Line")
        {
            if(GameManager.instance.finalRoundTimer > 0)
            {
                GameManager.instance.Invoke("LoadWinScreen", 1f);
            }
            else
            {
                DialogueManager.instance.soCloseToWinningPanel.SetActive(true);
                RandomMapGenerator.instance.Invoke("LoadMainMenu", 2f);
            }
        }
        else
        {
            FindObjectOfType<Goal>().GetPlacement += 1;
            GameManager.instance.playerPoints += FindObjectOfType<Goal>().GetPlacement;
            DialogueManager.instance.DisplayResult();
            GameManager.instance.GameState = GameManager.State.Result;

            if (SceneManager.GetActiveScene().name != "Tutorial 2")
                DialogueManager.instance.Invoke("ChangeGameState", 2f);
        }

        Instantiate(qualifyExplotion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void ClickTeleport()
    {
        if (Input.GetMouseButtonDown(1) && clickTeleportActive && GameManager.instance.GameState == GameManager.State.Racing)
        {
            Vector3 target = transform.up * 50f;

            transform.position = new Vector3(target.x + transform.position.x, target.y + transform.position.y, target.z + transform.position.z);
            clickTeleportActive = false;
        }
    }

    private void ShieldItemTimer()
    {
        if (shieldItem.activeInHierarchy && shieldTimer <= 15f && GameManager.instance.GameState == GameManager.State.Racing)
        {
            shieldTimer += Time.deltaTime;
        }
        else
        {
            shieldItem.SetActive(false);
        }
    }

    private void Destroy()
    {
        AudioManager.instance.Play("Death");
        AudioManager.instance.StopPlaying("Boost");
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void ProcessInput()
    {
        if (GameManager.instance.GameState != GameManager.State.Racing)
            return;
        BoostRocket();
        RotateRocket();
    }

    private void RotateRocket()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationThisFrame = turnSpeed * Time.deltaTime;

        //TODO: Rotate left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.left * rotationThisFrame);
        }
        //TODO: Rotate right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.right * rotationThisFrame);
        }
    }

    private void BoostRocket()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * moveSpeed * Time.deltaTime);
            AudioManager.instance.IsPlaying("Boost");
        }
        else
        {
            AudioManager.instance.StopPlaying("Boost");
        }
    }

    public void CheckItem()
    {
        switch (GameManager.instance.player)
        {
            case "SpeedUp":
                moveSpeed = 300f;
                break;
            case "Shield":
                shieldItem.SetActive(true);
                break;
            case "Click Teleport":
                clickTeleportActive = true;
                break;
            case "Lights":
                lightItem.SetActive(true);
                break;
            case "Fast Respawn":
                parent.GetComponent<LaunchPad>().respawnTime = 1f;
                break;
            default:
                break;
        }
    }
}