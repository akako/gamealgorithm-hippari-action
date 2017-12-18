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

        void Start()
        {
            player.IsMovable = true;

            speedKeepSkillButton.onClick.AddListener(player.InvokeSpeedKeepSkill);
            laserSkillButton.onClick.AddListener(player.InvokeLaserSkill);
        }
    }
}
