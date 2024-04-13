using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class Player : MonoBehaviour
{
    Coroutine moveCoroutine;
   [SerializeField]PlayerInput input;
    [SerializeField]float moveSpeed = 10f;
    [SerializeField] float paddingx = 0.2f;
    [SerializeField] float paddingy = 0.1f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = -3f;
    [SerializeField] float moveRotationAngle = 50f;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireInterval=0.2f;
    WaitForSeconds waitForFireInterval;
    //�ű���ʼ
    new Rigidbody2D rigidbody=null;
    private void Awake()
    {
        rigidbody=GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        waitForFireInterval = new WaitForSeconds(fireInterval);
        rigidbody.gravityScale = 0f;
        input.EnableGamePlayInput();
    }
     void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
    }
   
    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
    }
    #region �����ƶ�
    void Move(Vector2 moveInput)
    {
        //Vector2 moveAmount =moveInput*moveSpeed;
        //rigidbody.velocity = moveInput * moveSpeed;
        if(moveCoroutine!=null)
        {
            StopCoroutine(moveCoroutine);
        }
        Quaternion rotation = Quaternion.AngleAxis(moveRotationAngle*moveInput.y,Vector3.right);
        moveCoroutine = StartCoroutine(MoveCoroutine(accelerationTime,moveInput.normalized * moveSpeed,rotation));
        StartCoroutine(MovePositionCoroutine());
    }
    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine=StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
       StopCoroutine(MovePositionCoroutine());
    }
     
    //�����ƶ�Э��
    IEnumerator MovePositionCoroutine()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(this.transform.position,paddingx,paddingy);
            yield return null;
        }
        
    }
    //����Э��
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity,Quaternion moveRotation)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.fixedDeltaTime / time;
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, moveVelocity, t / time);
            transform.rotation=Quaternion.Lerp(transform.rotation, moveRotation, t / time);
            yield return null;
        }
    }
    #endregion
    #region ������
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
        
    }
    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {
        
        while (true)
        {
            Instantiate(projectile, muzzle.position, Quaternion.identity);
            yield return waitForFireInterval;
        }

    }
    #endregion
}
