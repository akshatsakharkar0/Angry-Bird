using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : MonoBehaviour
{
    [SerializeField] private AudioClip hitClip;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private bool hasBeenLaunched;
    private bool shouldFaceVelocityDirection;

    private void Awake() {
        rb= GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
        circleCollider.enabled =false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hasBeenLaunched && shouldFaceVelocityDirection){

            transform.right = rb.velocity;
        }
    }
    public void LaunchBird(Vector2 direction, float force){
            rb.isKinematic = false;
            circleCollider.enabled=true;

            //apply the force
            rb.AddForce(direction*force,ForceMode2D.Impulse);
            hasBeenLaunched=true;
            shouldFaceVelocityDirection=true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        shouldFaceVelocityDirection=false;
        SoundManager.instance.PlayClip(hitClip,audioSource);
        Destroy(this);
    }
}
