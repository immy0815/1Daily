using System;
using System.Collections;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Util;
using Retronia.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pistol : Weapon, IShootable
{
    [Header("Components")] 
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private AudioClip gunshotClip;
    [SerializeField] private ParticleSystem muzzleFlash;
    

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
    
    public static event Action OnReloadStart;
    
	private int originalBulletCount;
    private float recoilAngle = -70f;  
    private float upTime  = 0.1f;         
    private float downTime  = 0.15f;    
    private Quaternion originalRot;
    private Coroutine recoilRoutine;
    
    protected override void Awake()
    {
        base.Awake();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
        if (!bulletPoolObj) bulletPoolObj = GameObject.Find("BulletPool");

        originalRot = Quaternion.Euler(0, 0, 0);
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
        if (!bulletPoolObj) bulletPoolObj = GameObject.Find("BulletPool");
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
        
        OnReloadStart?.Invoke();
        PlayRecoil();  // 반동
        PlayMuzzleFlash();
        SoundManager.Play(gunshotClip,gameObject);
        
        // Direction 결정
        var direction = Physics.Raycast(
            player.MainCameraTransform.position,
            player.MainCameraTransform.forward, out var hitInfo, float.MaxValue, shootableLayer)
            ? (hitInfo.point - player.PlayerInventory.WeaponPivot.position).normalized
            : player.MainCameraTransform.forward;
        
        if (!bullet.TryGetComponent<Bullet>(out var obj)) return false;
        obj.Init(firePoint.transform.position, direction, bulletPool, IsOwnedByPlayer);
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
        
        var targetPosRandomElement = new Vector3(
            Random.Range(-0.15f, 0.15f),
            Random.Range(-0.15f, 0.15f),
            Random.Range(-0.15f, 0.15f)
        );

        var targetPos = enemy.Target.transform.position + targetPosRandomElement + Vector3.up * 1.5f; // 1.5f는 대략 플레이어 모델의 상체~머리
        
        // Direction 결정
        var direction = targetPos - firePoint.transform.position;

        if (!bullet.TryGetComponent<Bullet>(out var obj)) return false;
        obj.Init(firePoint.transform.position, direction, bulletPool, IsOwnedByPlayer);
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
        IsCurrentlyOwned = false;
        
        // TODO: 던지는 데미지를 적용해야 하는데 현재는 무기의 기본 데미지가 적용되어있음.(수정 필요!!!)
        if (IsThrownByPlayer)
        {
            thrownObject.Init(WeaponData.damage);
            rigidBody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            rigidBody.AddForce(direction, ForceMode.Impulse);
        }
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
        if (IsThrownByPlayer || IsCurrentlyOwned) return;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;
        IsOwnedByPlayer = isOwnedByPlayer;
        IsCurrentlyOwned = true;
        FillAmmo();
        StartCoroutine(MoveToPivot(pivot));
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash == null) return;
        
        muzzleFlash.transform.position = firePoint.transform.position;
        muzzleFlash.transform.rotation = firePoint.transform.rotation;
        muzzleFlash.Play(); 
    }
    
    public void PlayRecoil()
    {
        if (recoilRoutine != null)
            StopCoroutine(recoilRoutine);

        recoilRoutine = StartCoroutine(DoRecoil());
    }

    private IEnumerator DoRecoil()
    {
        Quaternion recoilRot = Quaternion.Euler(recoilAngle, 0f, 0f);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / upTime;
            transform.localRotation = Quaternion.Slerp(originalRot, recoilRot, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / downTime;
            transform.localRotation = Quaternion.Slerp(recoilRot, originalRot, t);
            yield return null;
        }

        transform.localRotation = originalRot;
    }
}