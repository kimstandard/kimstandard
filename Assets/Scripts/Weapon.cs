using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    public float range;
    public float damage;
    public int maxShot;
}

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Camera FPCamera;
    [SerializeField] protected GameObject hitEffect;    
    [SerializeField] protected AudioClip shootingSound;
    [SerializeField] protected int maxShotsBeforeReload = 10;

    protected AudioSource audioSource;
    protected bool isReloading = false;
    public Data data;

    protected virtual void Start() // virtual Ű���� �߰�
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (!GameManager.instance.IsGamePaused())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    protected virtual void Shoot()
    {
        PlayShootingSound();        
        ProcessRaycast();
    }

    protected void PlayShootingSound()
    {
        audioSource.PlayOneShot(shootingSound);
    }

    protected void ProcessRaycast()
    {
        RaycastHit hit;
        // ī�޶��� ��ġ�� ������ ����� �α׷� ���
        Debug.DrawRay(FPCamera.transform.position, FPCamera.transform.forward * data.range, Color.red);

        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, data.range))
        {
            Debug.Log("Raycast hit: " + hit.transform.name); // ���̰� � ��ü�� �¾Ҵ��� �α׷� ���

            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            target.TakeDamage(data.damage);
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }


    protected void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }
}
