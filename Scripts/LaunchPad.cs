using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] GameObject shipPrefab;
    [SerializeField] GameObject newShip;
    [SerializeField] bool canSpawn = true;
    public float respawnTime = 3f;

    //Waypoints For Enemy Rocket
    [SerializeField] GameObject[] wayPoints;
    int currentWayPoints = 0;
    Vector3 target;

    void Start()
    {
        Respawn();
    }

    void Update()
    {
        //TODO - Respawn ship at launch pad
        if (newShip == null)
            CheckSpawning();

        CheckWayPoints();
    }

    public bool CanSpawn
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    public void Respawn()
    {
        newShip = Instantiate(shipPrefab, new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z), transform.rotation);

        if (newShip.GetComponent<Rocket>() != null)
        {
            newShip.GetComponent<Rocket>().parent = gameObject;
            newShip.GetComponent<Rocket>().name = gameObject.name;
        }

        if (newShip.GetComponent<EnemyRocket>() != null)
        {
            newShip.GetComponent<EnemyRocket>().parent = gameObject;
            newShip.GetComponent<EnemyRocket>().name = gameObject.name;
        }
        
        canSpawn = true;
    }

    public void CheckSpawning()
    {
        if (canSpawn && FindObjectOfType<GameManager>().GameState == GameManager.State.Racing)
        {
            canSpawn = false;
            switch (shipPrefab.name)
            {
                case "Rocket":
                    Invoke("Respawn", respawnTime);
                    break;
                default:
                    Invoke("Respawn", respawnTime);
                    break;
            }
        }
    }

    public void CheckWayPoints()
    {
        if (wayPoints.Length <= 0 || newShip == null)
        {
            currentWayPoints = 0;
            return;
        }

        if(currentWayPoints != wayPoints.Length)
        {
            target = wayPoints[currentWayPoints].transform.position;
            newShip.GetComponent<EnemyRocket>().targetPosition = target;

            if (Vector3.Distance(newShip.transform.position, target) <= 70f)
            {
                currentWayPoints++;
            }
        }
        
    }
}
