using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HippariAction.Main
{
    public class EnemyAttack : MonoBehaviour
    {
        int power;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                var player = collision.GetComponent<Player>();
                player.Damage(power);
            }
        }

        public IEnumerator StartAnimate(int power)
        {
            this.power = power;
            transform.localScale = Vector3.zero;
            yield return transform.DOScale(Vector3.one, 0.3f)
                     .OnComplete(() =>
                     {
                         transform.localScale = Vector3.one;
                         Destroy(gameObject);
                     }).WaitForCompletion();
        }
    }
}
