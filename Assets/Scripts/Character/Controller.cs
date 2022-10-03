using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Movement
{
    public float Speed = 5;
    public float JumpSpeed = 7;

    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    private float m_direction = -1;
    private bool m_highjump;

    public AudioClip[] audioclips;
    bool isMoving;
    float timer;
    Vector3 startP;
    Vector3 destP;
    bool isDied;
    float runtime = 0f;
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }
    private void Start()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        runtime = 0f;
    }
    protected override void ComputeVelocity()
    {
        
        base.ComputeVelocity();
        if (isDied)
        {
            if (runtime < timer)
            {
                runtime += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startP, destP, runtime / timer);
                return;
            }
            m_animator.SetTrigger("deadEnd");
            isDied = false;
            this.gameObject.GetComponent<Collider2D>().enabled = true;
            runtime = 0f;
            return;
        }
        Vector2 move = Vector2.zero;
        if (GameManager.instance.isBlockInput())
        {
            isDied = true;
            timer = GameManager.instance.getTimer();
            m_rb2d.velocity = Vector2.zero;
            m_audioSrc.Stop();
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            destP = this.transform.localPosition;
            startP = this.transform.localPosition;
            destP.y = -8.599f;
            m_animator.SetTrigger("dead");
            return;
        }
            
        if (is_jump)
        {
            if (m_highjump)
                move.x = 0.8f * m_direction;
            else
                move.x = 0.5f * m_direction;

            targetVelocity = move * Speed;
            return;
        }

        move.x = Input.GetAxisRaw("Horizontal");
        if(move.x == 0)
        {
            isMoving = false;
            m_animator.SetFloat("MoveX", -1f);
        }
        else
        {
            isMoving = true;
            m_animator.SetFloat("MoveX", 1f);
        }

        //점프
        if (Input.GetButtonDown("Jump") && is_ground && move.x == 0)
        {
            is_jump = true;
            m_animator.SetTrigger("isJump");
            velocity.y = JumpSpeed;
            m_highjump = false;
        }
        else if(Input.GetButtonDown("Jump") && is_ground && move.x != 0)
        {
            is_jump = true;
            m_animator.SetTrigger("isJump");
            velocity.y = JumpSpeed;
            m_highjump = true;
        }
        if (is_ground)
        {
            m_animator.SetBool("isGround", true);
        }
        else
        {
            m_animator.SetBool("isGround", false);
        }

        //사다리
        if(is_ladder && move.x == 0)
        {
            move.y = Input.GetAxisRaw("Vertical");
            ladderVelocity = move * Speed * 0.9f;

            if(move.y == 0)
            {
                isMoving = false;
                m_animator.SetFloat("MoveY", -1f);
            }
            else
            {
                isMoving = true;
                m_animator.SetFloat("MoveY", 1f);
            }
        }

        //스프라이트 방향조절
        if(move.x > 0.01f && !on_ladder)
        {
            if (gameObject.transform.localScale.x > 0)
            {
                Vector3 temp = gameObject.transform.localScale;
                temp.x *= -1;
                gameObject.transform.localScale = temp;
                m_direction = 1;
            }
        }
        else if(move.x < -0.01f && !on_ladder)
        {
            if(gameObject.transform.localScale.x < 0)
            {
                Vector3 temp = gameObject.transform.localScale;
                temp.x *= -1;
                gameObject.transform.localScale = temp;
                m_direction = -1;
            }
        }

        targetVelocity = move * Speed;
        PlayWalk();
        PlayJump();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ladders" && !is_jump)
           is_ladder = true;

        if (collision.gameObject.tag == "onladder" && !is_jump)
        {
            on_ladder = true;
            this.gameObject.GetComponent<Collider2D>().isTrigger = true;
            m_animator.SetBool("isClimb", true);
        }
    }
    private void PlayWalk()
    {
        if (isMoving && !is_jump)
        {
            m_audioSrc.loop = true;
            if (!m_audioSrc.isPlaying)
            {
                m_audioSrc.PlayOneShot(audioclips[0]);
            }
        }
        else
        {
            m_audioSrc.loop = false;
            m_audioSrc.Stop();
        }
    }

    private void PlayJump()
    {
        if (is_jump)
        {
            m_audioSrc.loop = false;
            if(!m_audioSrc.isPlaying)
                m_audioSrc.PlayOneShot(audioclips[1]);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladders")
           is_ladder = false;
        
        if(collision.gameObject.tag == "onladder")
        {
            on_ladder = false;
            this.gameObject.GetComponent<Collider2D>().isTrigger = false;
            m_animator.SetBool("isClimb", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "blanks")
        {
            int floor = 0;
            //애니메이션 재생
            if (this.transform.localPosition.y > -5f && this.transform.localPosition.y < -0.5f)
            {
                floor = 1;
            }
            else if(this.transform.localPosition.y > -0.5f && this.transform.localPosition.y < 3.4f)
            {
                floor = 2;
            }
            else if(this.transform.localPosition.y > 3.4f && this.transform.localPosition.y < 7.4f)
            {
                floor = 3;
            }
            else
            {
                floor = 4;
            }
            GameManager.instance.setTimer(floor);
            GameManager.instance.diedPonpoko();
            AudioManager.instance.SetAudio(floor);
            AudioManager.instance.PlayAudio();
            GameManager.instance.restartGame(floor);
        }
    }
}
