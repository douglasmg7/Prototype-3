using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityModifier;
    public float jumpForce;
    public bool isOnGround = true;
    public bool gameOver;
    public ParticleSystem exploxionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    Rigidbody playerRb;
    Animator playerAnim;
    AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        playerAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Jump.
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            audioSource.PlayOneShot(jumpSound, 1f);
            dirtParticle.Stop();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            playerAnim.SetTrigger("Jump_trig");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Allow jump again.
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        // Crash.
        if (other.gameObject.CompareTag("Obstacle") && !gameOver)
        {
            audioSource.PlayOneShot(crashSound, 1f);
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            dirtParticle.Stop();
            exploxionParticle.Play();
            Debug.Log("Game Over!");
        }
    }
}
