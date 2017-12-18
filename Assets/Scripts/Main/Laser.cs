using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Laser : MonoBehaviour
{
    int power;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO なぜEnemyと衝突しない・・・？
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.GetComponent<Enemy>();
            enemy.Damage(power);
        }
    }

    public void StartAnimate(int power)
    {
        this.power = power;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f)
                 .OnComplete(() =>
                 {
                     transform.localScale = Vector3.one;
                     //                     Destroy(gameObject);
                 });
    }
}
