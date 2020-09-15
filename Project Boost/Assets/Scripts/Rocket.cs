using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] ParticleSystem deathExplosion;

    [SerializeField] float turnSpeed = 200f;
    [SerializeField] float moveSpeed = 250f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Respawn":
                //TODO - Respawn ship at launch pad
                break;
            case "Goal":
                //TODO - Reach Goal
                break;
            default:
                //TODO - Add particle effect and destroy ship

                Destroy(gameObject);
                break;
        }
    }

    private void ProcessInput()
    {
        StartRace();
        RotateRocket();
    }

    private void RotateRocket()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationThisFrame = turnSpeed * Time.deltaTime;

        //TODO: Rotate left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.left * rotationThisFrame);
        }
        //TODO: Rotate right
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right * rotationThisFrame);
        }
    }

    private void StartRace()
    {
        //TODO: Start Race
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * moveSpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
