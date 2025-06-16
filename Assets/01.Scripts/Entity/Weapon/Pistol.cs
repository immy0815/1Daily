using _01.Scripts.Util;
using UnityEngine;

public class Pistol : Weapon, IShootable
{
    [Header("Components")] 
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider boxCollider;
    
    [Header("Pistol Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletPoolObj;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float recoilTime = 1f;
    [SerializeField] private float throwForce = 10;
    
    [field: Header("Pistol Condition")]
    [field: SerializeField] public float TimeSinceLastShoot { get; private set; }
    [field: SerializeField] public bool IsReady { get; private set; } = true;
    
    protected override void Awake()
    {
        base.Awake();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    private void Update()
    {
        if (IsReady) return;
        if (TimeSinceLastShoot < recoilTime) TimeSinceLastShoot += Time.deltaTime;
        else { IsReady = true; TimeSinceLastShoot = 0; }
    }

    protected override void Reset()
    {
        base.Reset();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    public bool OnShoot()
    {
        if (!IsReady || bulletCount < 1) return false;
        AttackCoroutine = StartCoroutine(ChangeTimeScaleForSeconds(0.5f));
        var bulletPool = bulletPoolObj?.GetComponent<BulletPool>();
        if (!bulletPool) return false;
        bullet = bulletPool.GetBullet();
        
        if (!bullet) return false;
        bulletCount--;
        IsReady = false;
        
        var direction = transform.forward;
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction, bulletPool);
        return true;
    }

    public override void OnThrow(Vector3 direction, bool isThrownByPlayer)
    {
        if (AttackCoroutine != null) { StopCoroutine(AttackCoroutine); AttackCoroutine = null; } 
        transform.SetParent(null);
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        boxCollider.isTrigger = false;
        bulletCount = 6;
        IsReady = true;
        TimeSinceLastShoot = 0;
        IsThrownByPlayer = isThrownByPlayer;
        IsThrownByEnemy = !isThrownByPlayer;
        
        rigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        thrownObject.enabled = true;
    }

    public override void OnInteract(Transform pivot)
    {
        if (IsThrownByPlayer) return;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;
        StartCoroutine(MoveToPivot(pivot));
    }
}