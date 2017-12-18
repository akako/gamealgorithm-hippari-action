using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HippariAction.Main
{
    public class Enemy : Mob
    {
        public int AttackWaitCount
        {
            set
            {
                attackWaitCount = value;
                attackWaitCountText.text = string.Format("攻撃まで<size=30><color=white>{0}</color></size>", value);
            }
            get { return attackWaitCount; }
        }

        [SerializeField] int attackWaitCount;
        [SerializeField] EnemyAttack attackPrefab;
        [SerializeField] int attackWaitMax = 5;
        [SerializeField] Text attackWaitCountText;

        public IEnumerator AttackIfPossibleCoroutine()
        {
            if (attackWaitCount > 0)
            {
                yield break;
            }
            var laser = Instantiate(attackPrefab, transform.position, Quaternion.identity);
            laser.transform.SetParent(transform.parent);
            yield return laser.StartAnimate(power);
            AttackWaitCount = attackWaitMax;
        }
    }
}
