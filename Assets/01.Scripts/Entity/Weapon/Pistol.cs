using System.Collections;
using _01.Scripts.Util;
using Retronia.Core;
using UnityEngine;

public class Pistol : Weapon, IShootable
{
    [Header("Components")] 
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private AudioClip gunshotClip;
     
    
    [Header("Pistol Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletPoolObj;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float recoilTime = 1f;
    [SerializeField] private float throwForce = 10;
    [SerializeField] private ParticleSystem muzzleFlash;
    public float recoilAngle = -100f;  
    public float upTime  = 0.1f;         
    public float downTime  = 0.15f;    
    private Quaternion originalRot;
    private Coroutine recoilRoutine;
     
    
    [field: Header("Pistol Condition")]
    [field: SerializeField] public float TimeSinceLastShoot { get; private set; }
    [field: SerializeField] public bool IsReady { get; private set; } = true;
    
	private int originalBulletCount;

    protected override void Awake()
    {
        originalRot = transform.localRotation;
        base.Awake();
        if (!rigidBody) rigidBody = gameObject.GetComponent_Helper<Rigidbody>();
        if (!boxCollider) boxCollider = gameObject.GetComponent_Helper<BoxCollider>();
    }

    protected override void Start()
    {
        base.Start();
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

    public bool OnShoot()
    {
        if (!IsReady || bulletCount < 1) return false;
        var bulletPool = bulletPoolObj?.GetComponent<BulletPool>();
        if (!bulletPool) return false;
        PlayMuzzleFlash();
        SoundManager.Play(gunshotClip, AudioType.UI);
        
        bullet = bulletPool.GetBullet();
        if (!bullet) return false;
        bulletCount--;
        IsReady = false;
        PlayRecoil();  // 반동
        var direction = transform.forward;
        bullet.GetComponent<Bullet>().Init(firePoint.transform.position, direction, bulletPool, IsOwnedByPlayer);
        return true;
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