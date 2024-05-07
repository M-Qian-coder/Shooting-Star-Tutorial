using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

///<summary>
///
///</summary>
public class Player : Character
{
    [SerializeField] AudioData projectileLaunchSFX;

    [SerializeField]StarsBas_HUD starsBas_HUD;
    
    [SerializeField]bool regenerateHealth = true;
    [SerializeField]float healthRegenerateTime;
    [SerializeField,Range(0f,1f)] float healthRegeneratePercent;
    
    [Header("Input Setting")]
    [SerializeField]PlayerInput input;
    [Header("Move Setting")]
    [SerializeField]float moveSpeed = 10f;
    [SerializeField] float paddingx = 0.2f;
    [SerializeField] float paddingy = 0.1f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float decelerationTime = -3f;
    [SerializeField] float moveRotationAngle = 50f;
    [Header("Projectile Setting")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField,Range(0,2)] int weaponPower = 0;
    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;
    [SerializeField] float fireInterval=0.2f;
    [Header("---DODGE----")]
    [SerializeField] AudioData dodgeSFX;
    [SerializeField,Range(0,100)] int dodgeEnergyCost=25;
    [SerializeField] float maxRoll = 720f;
    [SerializeField] float rollSpeed = 360f;
    [SerializeField]bool IsDodging=false;
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    float currentRoll;
    float dodgeDuring;
    [Header("--OVERDRIVE--")]
    [SerializeField]int overdriveDodgeFactor = 2;
    [SerializeField]float overdriveSpeedFactor = 1.2f;
    [SerializeField]float overdriveFireFactor = 1.2f;
   
    bool isOverdrive = false;

    readonly float slowMotionDuratin=0.5f;

    Vector2 previousVelocity;
    Quaternion previousRotation;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitHealthRegenerateTime;
    WaitForFixedUpdate waitForFixedUpdate=new WaitForFixedUpdate();
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waitDecelerationTime;
    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;
    new Collider2D collider;
    //脚本开始
    new Rigidbody2D rigidbody=null;

    MissileSystem missile;
   
    private void Awake()
    {
        rigidbody =GetComponent<Rigidbody2D>();
        collider=GetComponent<Collider2D>();
        dodgeDuring = maxRoll / rollSpeed;
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overdriveFireFactor);
        waitDecelerationTime=new WaitForSeconds(decelerationTime);
        missile = GetComponent<MissileSystem>();
    }
    private void Start()
    {
        rigidbody.gravityScale = 0f;
        starsBas_HUD.Initialize(health, maxHealth);
        
        input.EnableGamePlayInput();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        starsBas_HUD.UpdateStats(health, maxHealth);
        TimeController.Instance.BulletTime(slowMotionDuratin);
        if(gameObject.activeSelf)
        {
            if(regenerateHealth)
            {
                if(healthRegenerateCoroutine!=null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine=StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent)); ;
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        
        base.RestoreHealth(value);
        starsBas_HUD.UpdateStats(health, maxHealth);
    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        starsBas_HUD.UpdateStats(0, maxHealth);
        base.Die();
    }
    #region 订阅和取消订阅事件
    protected override void OnEnable()
    {
        base.OnEnable();
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onDodge += Dodge;
        input.onOverDrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;

    }
   
    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onDodge -= Dodge;
        input.onOverDrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    #endregion
    #region 处理移动
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
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MoveRangeLimatationCoroutine));
    }
    void StopMove()
    {
        //rigidbody.velocity = Vector2.zero;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine=StartCoroutine(MoveCoroutine(decelerationTime,Vector2.zero,Quaternion.identity));
        StartCoroutine(nameof(DecelerationCoroutine));
    }
     
    //限制移动协程
    IEnumerator MoveRangeLimatationCoroutine()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(this.transform.position,paddingx,paddingy);
            yield return null;
        }
        
    }
    IEnumerator DecelerationCoroutine()
    {
        yield return waitDecelerationTime;
        StopCoroutine(nameof(MoveRangeLimatationCoroutine));
    }
    //加速协程
    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity,Quaternion moveRotation)
    {
        float t = 0f;
        previousVelocity=rigidbody.velocity;
        previousRotation = transform.rotation;
        while (t < 1f)
        {
            t += Time.fixedDeltaTime/time;
            rigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t / time);
            transform.rotation=Quaternion.Lerp(previousRotation, moveRotation, t / time);
            yield return waitForFixedUpdate;
        }
    }
    #endregion
    #region 处理开火
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
        
    }
    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }
    //开火协程
    IEnumerator FireCoroutine()
    {
        
        while (true)
        {
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleBottom.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile1, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile2, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectile3, muzzleBottom.position, Quaternion.identity);
                    break;
            }
            
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            yield return isOverdrive ? waitForOverdriveFireInterval : waitForFireInterval;
           
            //yield return waitForFireInterval;
        }
    }
    #endregion
    #region Dodge
    void Dodge()
    {
        if (IsDodging||!PlayerEnergy.Instance.IsEnough(dodgeEnergyCost)) return;
        StartCoroutine(nameof(DodgeCoroutine));
        
      

    }
    IEnumerator DodgeCoroutine()
    {
        IsDodging = true;
        AudioManager.Instance.PlayRandomSFX(dodgeSFX);
        //消耗能量
        PlayerEnergy.Instance.Use(dodgeEnergyCost);
        //闪避过程中无敌
        collider.isTrigger = true;
        //视觉效果
        currentRoll = 0f;
        var scale = transform.localScale;
        TimeController.Instance.BulletTime(slowMotionDuratin, slowMotionDuratin);
        while(currentRoll<maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);
            transform.localScale = BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeScale, currentRoll / maxRoll);

            //currentRoll+=rollSpeed*Time.deltaTime;
            //transform.rotation =Quaternion.AngleAxis(currentRoll,Vector3.right);
            //if (currentRoll < maxRoll / 2f)
            //{
            //    scale.x = Mathf.Clamp(scale.x - Time.deltaTime / dodgeDuring, dodgeScale.x, 1f);
            //    scale.y = Mathf.Clamp(scale.y - Time.deltaTime / dodgeDuring, dodgeScale.y, 1f);
            //    scale.z = Mathf.Clamp(scale.z - Time.deltaTime / dodgeDuring, dodgeScale.z, 1f);
            //    //scale -= (Time.deltaTime / dodgeDuring) * Vector3.one;
            //}
            //else
            //{
            //    scale.x = Mathf.Clamp(scale.x + Time.deltaTime / dodgeDuring, dodgeScale.x, 1f);
            //    scale.y = Mathf.Clamp(scale.y + Time.deltaTime / dodgeDuring, dodgeScale.y, 1f);
            //    scale.z = Mathf.Clamp(scale.z + Time.deltaTime / dodgeDuring, dodgeScale.z, 1f);
            //}
            //transform.localScale = scale;
            yield return null;
        }
       

        collider.isTrigger = false;
        IsDodging=false;
    }

    #endregion
    #region OVERDRIVE
    void Overdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;
        PlayerOverdrive.on.Invoke();

    }
    void OverdriveOn()
    {
        isOverdrive = true;
        dodgeEnergyCost += overdriveDodgeFactor;
        moveSpeed += overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuratin,slowMotionDuratin);

    }
    void OverdriveOff()
    {
        isOverdrive = false;
        dodgeEnergyCost -= overdriveDodgeFactor;
        moveSpeed -= overdriveSpeedFactor;
    }
    #endregion
    #region Missile
    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
    #endregion
}
