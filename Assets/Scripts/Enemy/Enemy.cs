using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int floor;
    bool turnArrow;
    Vector2 movement;
    Rigidbody2D m_rb2d;
    SpriteRenderer m_spriteRenderer;
    Vector2 RightWall;
    Vector2 LeftWall;

    private void Start()
    {
        turnArrow = true;
        m_rb2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new Vector2(1, 0);

        RightWall = new Vector2(13f, 0);
        LeftWall = new Vector2(-13f, 0);
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
            Vector3 temp = gameObject.transform.localScale;
            temp.x *= -1;
            gameObject.transform.localScale = temp;
        }
    }

    private void moveLeft()
    {
        m_rb2d.position += -1f * movement * speed * Time.deltaTime;
        if(m_rb2d.position.x < LeftWall.x)
        {
            turnArrow = true;
            Vector3 temp = gameObject.transform.localScale;
            temp.x *= -1;
            gameObject.transform.localScale = temp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.setTimer(floor);
            GameManager.instance.diedPonpoko();
            AudioManager.instance.SetAudio(floor);
            AudioManager.instance.PlayAudio();
            GameManager.instance.restartGame(floor);
        }
    }
}
