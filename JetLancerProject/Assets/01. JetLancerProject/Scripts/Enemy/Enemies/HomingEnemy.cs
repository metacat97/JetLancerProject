using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HomingEnemy : EnemyBase, IDamageable
{
  
    private float fireMTime = default;
    private float maxfireTime = default;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        base.SetTarget();
    }

    private void Update()
    {
        base.CheckTarget();
        Move();
        Fire();
    }

    // TODO : 추후 폴리싱 기간에 가능하다면 CSV 파일로 정보를 읽어와서 넣어주기
    // 해당 초기화 값은 임시값 
    protected override void Init()
    {
        base.Init();

        Type = TYPE.HOMING;
        hp = 3;
        damage = 1;
        speed = 5f;
        maxSpeed = 10f;

        fireMTime = 0f;
        maxfireTime = 15f;

    }
    protected override void Move()
    {
        // 여기서 타입별로 moveFunc을 다르게 준다.
        this.moveFunc = () =>
        {
            // TODO : 공용 움직임 구현 한다면, 상위 기체에 추가적인 움직임 추가 
        };

        // {위 TODO 부분은 null도 아니고 default도 아니기 때문에 default 초기화 해줌
        this.moveFunc = default;
        // }위 TODO 부분은 null도 아니고 default도 아니기 때문에 default 초기화 해줌

        base.Move();
    }       // Move()

    protected override void Fire()
    {
      
        if (distToTarget < 30f)
        {
            fireMTime += Time.deltaTime;

            if (fireMTime > maxfireTime)
            {
                fireMTime = 0f;

                FireMissile();
                audioSource.PlayOneShot(fireClip);
            }
        }
    }

    public void FireMissile()
    {
        GameManager.Instance.poolManager.
            SpawnFromPool(RDefine.ENEMY_ROCKET, this.transform.position, Quaternion.identity);
       
    }


    protected override void Die()
    {
        GameManager.Instance.poolManager.SpawnFromPool(RDefine.ENEMY_EXPLOSION, this.transform.position, Quaternion.identity);
        // TODO : 파괴 기타 사항 추가 
    }



}
