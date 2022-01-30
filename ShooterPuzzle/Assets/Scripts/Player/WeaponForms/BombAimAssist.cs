using UnityEngine;
using System.Collections;

public class BombAimAssist : MonoBehaviour
{
    public LayerMask layersToHit;
    public LineRenderer lr;
    int line_res;
    int lr_maxPoints;

    Vector3 startPos;

    float velocity;
    float angle;
    float g;
    float dt;

    float Vy_old;
    float Vy;
    float Vx;

    void Start()
    {
        dt = 0.01f;
        velocity = 10;
        angle = 45;
        startPos = new Vector3(1,-1,0);
        lr_maxPoints = 200;
        lr.positionCount = lr_maxPoints;

    }

    public void Init(float gravityMult)
    {
        g = -Physics2D.gravity.y * gravityMult;
    }

    public void RefreshParameters(Vector3 startPos, float angle,float velocity)
    {
        this.startPos = startPos;
        this.velocity = velocity;
        this.angle = angle;
    }
    void Update() // before each update the class must refresh the parameters!
    {
        Vy_old = velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        Vx = velocity * Mathf.Sin(angle * Mathf.Deg2Rad); //checked

        //Debug.Log(Vx + " , " + Vy_old);

        lr.positionCount = lr_maxPoints;

        for (int i = 0; i < lr_maxPoints; i++)
        {
            Vy = Vy_old - g * dt;
            Vy_old = Vy;
            lr.SetPosition(i, new Vector2(startPos.x + Vx * i * dt, startPos.y + Vy * i * dt));
            if (i>0)
            {
                Physics2D.queriesStartInColliders = true;
                RaycastHit2D hit = Physics2D.Raycast(lr.GetPosition(i), lr.GetPosition(i) - lr.GetPosition(i - 1), dt, layersToHit);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag.Equals("BaseGround") ||
                        hit.collider.gameObject.tag.Equals("Wood") ||
                        hit.collider.gameObject.tag.Equals("Steel")
                        )
                    {
                        lr.positionCount = i + 1;
                        break;
                    }
                }
            }
        }
    }
}
