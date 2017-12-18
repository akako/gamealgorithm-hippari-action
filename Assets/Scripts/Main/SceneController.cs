using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HippariAction.Main
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] Button speedKeepSkillButton;
        [SerializeField] Button laserSkillButton;
        [SerializeField] Player player;
        [SerializeField] Transform enemyContainer;
        [SerializeField] Text message;

        void Start()
        {
            foreach (var enemy in enemyContainer.GetComponentsInChildren<Enemy>())
            {
                enemy.AttackWaitCount = Random.Range(1, 6);
            }

            player.onMoveFinishedAction = OnPlayerMoveFinished;
            player.onDestroyed = GameOver;
            player.IsMovable = true;

            speedKeepSkillButton.onClick.AddListener(player.InvokeSpeedKeepSkill);
            laserSkillButton.onClick.AddListener(player.InvokeLaserSkill);
        }

        void OnPlayerMoveFinished()
        {
            StartCoroutine(EnemyActionCoroutine());
        }

        void GameOver()
        {
            message.text = "Game Over";
            message.gameObject.SetActive(true);
        }

        void GameClear()
        {
            message.text = "Game Clear!!";
            message.gameObject.SetActive(true);
        }

        IEnumerator EnemyActionCoroutine()
        {
            player.IsMovable = false;
            yield return new WaitForSeconds(0.5f);
            var enemies = enemyContainer.GetComponentsInChildren<Enemy>();
            if (enemies.Length <= 0)
            {
                GameClear();
                yield break;
            }
            foreach (var enemy in enemies)
            {
                enemy.AttackWaitCount--;
                yield return enemy.AttackIfPossibleCoroutine();
            }
            player.IsMovable = true;
        }
    }
}
