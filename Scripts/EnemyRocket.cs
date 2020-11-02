using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemyRocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] ParticleSystem qualifyExplotion;

    public string name;
    public GameObject parent = null;

    [SerializeField] GameObject lightItem;
    [SerializeField] GameObject shieldItem;
    float randTimer;
    float shieldTimer = 0;
    bool clickTeleportActive = false;
    [SerializeField] float turnSpeed = 200f;
    [SerializeField] float moveSpeed = 250f;
    public float respawnTime = 3f;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        RepositionOnStart();
        CheckItem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindWayPoint();

        ControlSpeedNearGoal();

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

    public void CheckItem()
    {
        string temp = null;

        switch (tag)
        {
            case "Enemy1":
                temp = GameManager.instance.enemy1;
                break;
            case "Enemy2":
                temp = GameManager.instance.enemy2;
                break;
            case "Enemy3":
                temp = GameManager.instance.enemy3;
                break;
            default:
                temp = null;
                break;
        }

        switch (temp)
        {
            case "SpeedUp":
                moveSpeed = 300f;
                break;
            case "Shield":
                shieldItem.SetActive(true);
                break;
            case "Click Teleport":
                clickTeleportActive = true;
                randTimer = Random.Range(1f, 5f);
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

    private void ClickTeleport()
    {
        randTimer -= Time.deltaTime;

        if (randTimer <= 0 && clickTeleportActive && GameManager.instance.GameState == GameManager.State.Racing)
        {
            Vector3 target = transform.forward * 50f;

            //TODO: Add teleport vfx

            transform.position =
                new Vector3(target.x + transform.position.x, 
                            target.y + transform.position.y, 
                            target.z + transform.position.z);

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

    private void ReachGoal()
    {
        AudioManager.instance.Play("Goal");

        // This control player placement
        FindObjectOfType<Goal>().GetPlacement += 1;

        // This stop won enemy from spawning
        if (parent != null)
            parent.GetComponent<LaunchPad>().CanSpawn = false;

        // Explosion effect when qualify
        Instantiate(qualifyExplotion, transform.position, Quaternion.identity);
        Destroy(gameObject);

        // TODO: Choose item for enemy
        SelectItem();
    }

    private void SelectItem()
    {
        switch (tag)
        {
            case "Enemy1":
                GameManager.instance.enemy1 =
                    GameManager.instance.SelectItemEnemy(
                        GameManager.instance.player,
                        GameManager.instance.enemy1,
                        GameManager.instance.enemy2,
                        GameManager.instance.enemy3);
                break;
            case "Enemy2":
                GameManager.instance.enemy2 =
                    GameManager.instance.SelectItemEnemy(
                        GameManager.instance.player,
                        GameManager.instance.enemy1,
                        GameManager.instance.enemy2,
                        GameManager.instance.enemy3);
                break;
            case "Enemy3":
                GameManager.instance.enemy3 =
                    GameManager.instance.SelectItemEnemy(
                        GameManager.instance.player,
                        GameManager.instance.enemy1,
                        GameManager.instance.enemy2,
                        GameManager.instance.enemy3);
                break;
            default:
                break;
        }
    }

    private void FindWayPoint()
    {
        if (GameManager.instance.GameState != GameManager.State.Racing)
            return;

        // Control speed for certain map
        if(SceneManager.GetActiveScene().name == "The Zig Zag" || SceneManager.GetActiveScene().name == "The Dark" && !shieldItem.activeInHierarchy)
        {
            moveSpeed = 130f;
        }

        // Move forward
        rigidBody.AddRelativeForce(Vector3.forward * moveSpeed * Time.deltaTime);

        // Rotate towards targeted waypoint
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

        // Check targeted waypoint
        Debug.DrawLine(transform.position, targetPosition, Color.red);
    }

    private void ControlSpeedNearGoal()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Finish").transform.position) <= 200f)
        {
            moveSpeed = 150f;
        }
    }

    private void Destroy()
    {
        AudioManager.instance.Play("Death");
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void RepositionOnStart()
    {
        transform.eulerAngles = new Vector3(
                         -90f,
                         transform.eulerAngles.y,
                         transform.eulerAngles.z
                     );
    }
}