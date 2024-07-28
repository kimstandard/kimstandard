using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGun : Weapon
{
    protected override void Start() // override Ű���� �߰�
    {
        base.Start();
        // Data ����ü �ʱ�ȭ
        data.range = 30f;
        data.damage = 30f;
        data.maxShot = 10;
    }

    protected override void Shoot()
    {
        base.Shoot();
        // DefaultGun�� Ưȭ�� �߰� ������ �ִٸ� ���⿡ �ۼ��մϴ�.
    }
}


