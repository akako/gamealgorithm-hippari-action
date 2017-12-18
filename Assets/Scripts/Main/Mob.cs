using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace HippariAction.Main
{
    public class Mob : MonoBehaviour
    {
        public Action onMoveFinishedAction;

        [SerializeField] Text hitPointText;
        [SerializeField] protected int hitPoint = 100;
        [SerializeField] protected int power = 10;
        [SerializeField] protected float speed = 10f;
        [SerializeField] [Range(0f, 1f)] protected float slipRate = 1f;

        protected int speedKeepCount;
        protected Rigidbody2D rigidBodyCache;
        protected bool isMovable;
        protected bool isMoving;

        public virtual bool IsMovable
        {
            set
            {
                isMovable = value;
                rigidBodyCache.constraints = value ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
            }
        }

        protected virtual void Awake()
        {
            rigidBodyCache = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            hitPointText.text = "" + hitPoint;
        }

        protected virtual void Update()
        {
            if (isMoving)
            {
                if (speedKeepCount <= 0)
                {
                    // 摩擦減速処理
                    rigidBodyCache.velocity *= 0.99f - (0.09f * (1f - slipRate));
                }
                if (rigidBodyCache.velocity.magnitude <= 10f)
                {
                    // ある程度遅くなったら動きを止める
                    rigidBodyCache.velocity = Vector2.zero;
                    isMoving = false;
                    if (null != onMoveFinishedAction)
                    {
                        OnMoveFinished();
                        onMoveFinishedAction();
                    }
                }
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (isMoving && speedKeepCount <= 0)
            {
                // 衝突減速処理
                rigidBodyCache.velocity *= 0.9f;
            }

            if (isMoving)
            {
                var targetTag = collision.gameObject.tag;
                if (targetTag == "Enemy" || targetTag == "Wall")
                {
                    speedKeepCount = Mathf.Max(0, speedKeepCount - 1);
                }

                if (targetTag == "Enemy")
                {
                    collision.gameObject.GetComponent<Enemy>().Damage(power);
                }
            }
        }

        protected virtual void OnMoveFinished() { }

        public void Move(Vector2 velocity)
        {
            rigidBodyCache.velocity = velocity;
            isMoving = true;
        }

        public void Damage(int value)
        {
            hitPointText.transform.DOShakePosition(0.5f, 10)
                        .OnComplete(() =>
                        {
                            hitPointText.transform.localPosition = Vector3.zero;
                        });
            hitPoint = Mathf.Max(0, hitPoint - value);
            hitPointText.text = "" + hitPoint;
            if (hitPoint <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
