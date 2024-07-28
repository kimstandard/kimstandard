using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGun : Weapon
{
    protected override void Start() // override 키워드 추가
    {
        base.Start();
        // Data 구조체 초기화
        data.range = 30f;
        data.damage = 30f;
        data.maxShot = 10;
    }

    protected override void Shoot()
    {
        base.Shoot();
        // DefaultGun에 특화된 추가 로직이 있다면 여기에 작성합니다.
    }
}


