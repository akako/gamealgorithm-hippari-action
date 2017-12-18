using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HippariAction.Main
{
    public class Player : Mob
    {
        public Action onDestroyed;

        [SerializeField] Laser laserPrefab;

        Vector2 startPosition;
        bool isLaserSkillActive;
        Image image;

        public override bool IsMovable
        {
            set
            {
                base.IsMovable = value;
                if (null != image)
                {
                    image.color = value ? Color.white : Color.black;
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            image = GetComponent<Image>();

            var eventTrigger = GetComponent<EventTrigger>();
            var beginDragEntry = new EventTrigger.Entry();
            beginDragEntry.eventID = EventTriggerType.PointerDown;
            beginDragEntry.callback.AddListener(data =>
            {
                OnBeginDrag((PointerEventData)data);
            });
            eventTrigger.triggers.Add(beginDragEntry);

            var dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener(data =>
            {
                OnDrag((PointerEventData)data);
            });
            eventTrigger.triggers.Add(dragEntry);

            var endDragEntry = new EventTrigger.Entry();
            endDragEntry.eventID = EventTriggerType.EndDrag;
            endDragEntry.callback.AddListener(data =>
            {
                OnEndDrag((PointerEventData)data);
            });
            eventTrigger.triggers.Add(endDragEntry);
        }

        protected override void Update()
        {
            base.Update();
            Debug.Log(speedKeepCount);
        }

        void OnDestroy()
        {
            if (null != onDestroyed)
            {
                onDestroyed();
            }
        }

        void OnBeginDrag(PointerEventData data)
        {
            if (!isMovable || isMoving)
            {
                return;
            }

            Debug.Log("begin drag");
            Debug.Log(data.position);
            startPosition = data.position;
        }

        void OnDrag(PointerEventData data)
        {
            if (!isMovable || isMoving)
            {
                return;
            }

            var diff = data.position - startPosition;
            Debug.Log(diff);
        }

        void OnEndDrag(PointerEventData data)
        {
            if (!isMovable || isMoving)
            {
                return;
            }

            var diff = data.position - startPosition;
            Debug.Log("go!!");
            var magnitudeLimit = 50f;
            Debug.Log(diff.magnitude);
            var magnitudeLimitRatio = magnitudeLimit / Mathf.Max(diff.magnitude, magnitudeLimit);
            var speedBonus = speedKeepCount > 0 ? 2f : 1f;
            Move(-diff * magnitudeLimitRatio * speed * speedBonus);
        }

        protected override void OnMoveFinished()
        {
            base.OnMoveFinished();
            speedKeepCount = 0;
            isLaserSkillActive = false;
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (isLaserSkillActive && collision.gameObject.tag == "Enemy")
            {
                FireLaser();
            }
        }

        public void InvokeSpeedKeepSkill()
        {
            speedKeepCount = 5;
            isLaserSkillActive = false;
            Debug.Log("iteru");
        }

        public void InvokeLaserSkill()
        {
            speedKeepCount = 0;
            isLaserSkillActive = true;
            Debug.Log("hogehoge");
        }

        void FireLaser()
        {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.SetParent(transform.parent);
            laser.StartAnimate(power);
        }
    }
}
