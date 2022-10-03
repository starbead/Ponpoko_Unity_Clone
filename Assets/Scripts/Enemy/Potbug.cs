using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potbug : MonoBehaviour
{
    public GameObject Pot;
    public float speed;
    public int floor;

    bool turnArrow;
    Vector2 movement;
    Rigidbody2D m_rb2d;
    SpriteRenderer m_spriteRenderer;
    Vector2 RightWall;
    Vector2 LeftWall;

    bool showEffect;

    float timer;
    float blinktime;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        showEffect = true;
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        blinktime = 0.1f;
        AudioManager.instance.SetAudio((int)AudioManager.audio.trappot);
        AudioManager.instance.PlayAudio();
        StartCoroutine(blinkEffect());
        Pot.SetActive(false);
    }

    private void Start()
    {
        turnArrow = true;
        m_rb2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new Vector2(1, 0);

        RightWall = new Vector2(13f, 0);
        LeftWall = new Vector2(-13f, 0);
    }
    private void Update()
    {
        if (showEffect)
        {
            if(timer < blinktime)
            {
                m_spriteRenderer.color = new Color(1, 1, 1, 1f - timer * 10);
            }
            else
            {
                timer = 0;
                m_spriteRenderer.color = new Color(1, 1, 1, (timer - (0.2f + blinktime)) * 10);
            }
            timer += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (turnArrow)
        {
            moveRight();
        }
        else
        {
            moveLeft();
        }
    }

    private void moveRight()
    {
        m_rb2d.position += movement * speed * Time.deltaTime;

        if (m_rb2d.position.x > RightWall.x)
        {
            turnArrow = false;
            m_spriteRenderer.flipX = false;
        }
    }

    private void moveLeft()
    {
        m_rb2d.position += -1f * movement * speed * Time.deltaTime;
        if (m_rb2d.position.x < LeftWall.x)
        {
            turnArrow = true;
            m_spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !showEffect)
        {
            GameManager.instance.setTimer(floor);
            GameManager.instance.diedPonpoko();
            AudioManager.instance.SetAudio(floor);
            AudioManager.instance.PlayAudio();
            GameManager.instance.restartGame(floor);
        }
    }

    IEnumerator blinkEffect()
    {
        yield return new WaitForSeconds(3f);
        showEffect = false;
        m_spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
}
