using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HippariAction.Main
{
    public class Player : Mob
    {
        [SerializeField] Laser laserPrefab;

        Vector2 startPosition;
        bool isLaserSkillActive;

        protected override void Start()
        {
            base.Start();
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
            Move(-diff * speed);
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
        }

        public void InvokeLaserSkill()
        {
            speedKeepCount = 0;
            isLaserSkillActive = true;
        }

        void FireLaser()
        {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.transform.SetParent(transform.parent);
            laser.StartAnimate(power);
        }
    }
}
