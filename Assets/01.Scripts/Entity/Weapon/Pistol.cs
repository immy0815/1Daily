using _01.Scripts.Entity.Player.Scripts;
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
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float recoilTime = 1f;
    [SerializeField] private float throwForce = 10;
    
    [field: Header("Pistol Condition")]
    [field: SerializeField] public float TimeSinceLastShoot { get; private set; }
    [field: SerializeField] public bool IsReady { get; private set; } = true;
    
	private int originalBulletCount;

    protected override void Awake()
    {
        base.Awake();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
        
        originalBulletCount = bulletCount;
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

    public bool OnShoot(Player player)
    {
        if (!IsReady || bulletCount < 1) return false;
        var bulletPool = bulletPoolObj?.GetComponent<BulletPool>();
        if (!bulletPool) return false;
        bullet = bulletPool.GetBullet();
        
        if (!bullet) return false;
        bulletCount--;
        IsReady = false;
        
        var direction = Physics.Raycast(
            player.MainCameraTransform.position,
            player.MainCameraTransform.forward, out var hitInfo, float.MaxValue, shootableLayer)
            ? (hitInfo.point - player.PlayerInventory.WeaponPivot.position).normalized
            : player.MainCameraTransform.forward;
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction, bulletPool, IsOwnedByPlayer);
        return true;
    }

    public bool OnShoot(Enemy enemy)
    {
        if (!IsReady || bulletCount < 1) return false;
        var bulletPool = bulletPoolObj?.GetComponent<BulletPool>();
        if (!bulletPool) return false;
        bullet = bulletPool.GetBullet();
        
        if (!bullet) return false;

        bulletCount--;
        IsReady = false;
        
        Vector3 targetPosRandomElement = new Vector3(
            Random.Range(-0.15f, 0.15f),
            Random.Range(-0.15f, 0.15f),
            Random.Range(-0.15f, 0.15f)
        );

        Vector3 targetPos = enemy.Target.transform.position + targetPosRandomElement + Vector3.up * 1.5f;
        
        // direction 결정
        var direction = targetPos - firePoint.transform.position; // 1.5f는 대략 눈높이 키
        
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction, bulletPool, IsOwnedByPlayer);
        return true;
    }

    public bool CanShoot()
    {
        return bulletCount != 0;
    }

    public override void OnThrow(Vector3 direction, bool isThrownByPlayer)
    {
        transform.SetParent(null);
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        boxCollider.isTrigger = false;
        bulletCount = 6;
        IsReady = true;
        TimeSinceLastShoot = 0;
        IsThrownByPlayer = isThrownByPlayer;
        
        rigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        thrownObject.enabled = true;
    }

    /// <summary>
    /// Enemy가 총을 쏠 때 재장전이 필요할 때 부르는 함수
    /// </summary>
	public void FillAmmo()
    {
        bulletCount = originalBulletCount;
    }

    public override void OnInteract(Transform pivot, bool isOwnedByPlayer)
    {
        if (IsThrownByPlayer) return;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;
        IsOwnedByPlayer = isOwnedByPlayer;
        FillAmmo();
        StartCoroutine(MoveToPivot(pivot));
    }
    
}