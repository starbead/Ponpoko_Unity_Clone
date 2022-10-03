using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected Vector2 ladderVelocity;
    protected bool is_ground;
    protected bool is_jump;
    protected bool is_ladder;
    protected bool on_ladder;
    protected Vector2 groundNormal;
    protected Rigidbody2D m_rb2d;
    protected AudioSource m_audioSrc;
    protected Collider2D m_collider;
    protected Vector2 velocity;
    protected Vector2 velocity_ladder;
    protected ContactFilter2D m_contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_audioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_contactFilter.useTriggers = false;
        m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        m_contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        targetVelocity = Vector2.zero;
        ladderVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {
    }

    private void FixedUpdate()
    {
        if (is_ladder)
        {
            velocity_ladder.y = ladderVelocity.y;
            ComputeLadderMovement(velocity_ladder);

            if (on_ladder)
            {
                m_rb2d.gravityScale = 0;
                m_rb2d.velocity = Vector2.zero;
                return;
            }
            else
            {
                m_rb2d.gravityScale = 1;
            }
        }

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        is_ground = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        ComputeMovement(move, false);

        move = Vector2.up * deltaPosition.y;

        ComputeMovement(move, true);
    }

    private void ComputeMovement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if(distance > minMoveDistance)
        {
            int count = m_rb2d.Cast(move, m_contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for(int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                if(currentNormal.y > minGroundNormalY)
                {
                    is_ground = true;
                    is_jump = false;

                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);

                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        m_rb2d.position = m_rb2d.position + move.normalized * distance;
    }

    private void ComputeLadderMovement(Vector2 move)
    {
        Vector2 deltaPosition = velocity_ladder * Time.deltaTime;
        m_rb2d.position = m_rb2d.position + deltaPosition;
    }

}
