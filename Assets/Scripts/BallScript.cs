using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private const string _Player = "Player";
    private const string _Block = "Block";
    private const string _LostBallDetector = "LostBallDetector";
    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;
    private Rigidbody2D rb;
    private Rigidbody2D playerRB;
    public AudioClip[] ballSounds;
    private List<Vector2> bouncePosList = new List<Vector2>{ Vector2.zero, Vector2.one };

    private GameManagerScript GMS;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRB = transform.parent.GetComponent<Rigidbody2D>();
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_Player))
        {
            SoundManager.instance.RandomizeSfx(1, ballSounds[0]);
            if (Mathf.Abs(rb.velocity.x) < .0001f)                      //Prevent 90 degrees repeat bouncing
            {
                float random = Random.value * 30 + 30;
                rb.velocity += new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * 2;
            }
            rb.velocity += playerRB.velocity / 2;
            SetVelocityLimits();
        }
        else if (collision.gameObject.CompareTag(_Block))
        {
            BlockScript BS = collision.gameObject.GetComponent<BlockScript>();
            BS.health--;
            GMS.UpdateProgress(BS);
            if (BS.health == 0)
            {
                SoundManager.instance.RandomizeSfx(.3f, ballSounds[2]);
                StartCoroutine(BS.DestroyBlockCoroutine());
            }
            else
            {
                SoundManager.instance.RandomizeSfx(.3f, ballSounds[1]);
                BS.ChangeSprite();
            }
        }
        else if (collision.gameObject.CompareTag(_LostBallDetector)) GMS.GMH.GameOver();
        else
        {
            SoundManager.instance.RandomizeSfx(1, ballSounds[0]);
            CheckBallStucks(transform.position);
        }
    }

    private void LateUpdate()
    {
        SetVelocityLimits();
    }

    private void SetVelocityLimits()
    {
        double sm = rb.velocity.sqrMagnitude;
        if (sm > (double)maxVelocity * maxVelocity) rb.velocity = rb.velocity.normalized * maxVelocity;
        else if (sm < (double)minVelocity * minVelocity) rb.velocity = rb.velocity.normalized * minVelocity;
    }

    public void LaunchBall()
    {
        transform.SetParent(null);
        rb.simulated = true;
        rb.AddForce(new Vector2(60, 200));
    }

    private void CheckBallStucks(Vector2 lastBouncePos)
    {
        lastBouncePos = new Vector2(Mathf.Round(lastBouncePos.x * 10.0f) * 0.1f, Mathf.Round(lastBouncePos.y * 10.0f) * 0.1f);
        if (lastBouncePos == bouncePosList[0])                       //Add random velocity vector if ball stucks between 2 walls
        {
            float random = Random.value * 30 + 30;
            rb.velocity += new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * 2;
            print("Stuck!");
        }
        bouncePosList.RemoveAt(0);
        bouncePosList.Add(lastBouncePos);
    }
}
