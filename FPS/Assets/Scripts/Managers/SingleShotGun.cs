using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun 
{
    public AudioSource awp;
    PhotonView PV;
    public ParticleSystem muzzleFlash;

    public AudioClip gunSound;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        awp = GetComponent<AudioSource>();
    }
    void Awake(){
        PV = GetComponent<PhotonView>();
    }
    [SerializeField] Camera cam;
    public override void Use(){
        Shoot();
    }
    void Shoot(){
        muzzleFlash.Play();
        // awp.Play();
        PV.RPC("Gunshot", RpcTarget.All);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit)){
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point,hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal){
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0){
            GameObject BulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up)*bulletImpactPrefab.transform.rotation);
            Destroy(BulletImpactObj, 10f);
            BulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    [PunRPC]
    public void Gunshot(){
        AudioSource audioRPC = gameObject.AddComponent<AudioSource> ();
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 25;
        audioRPC.maxDistance = 100;
        audioRPC.PlayOneShot(gunSound);
    }

}
