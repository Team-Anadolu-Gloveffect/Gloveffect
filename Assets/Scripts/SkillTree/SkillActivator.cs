using Player;
using UnityEngine;
using UnityEngine.UI;

public class SkillActivator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameManager;

    [SerializeField] private Button sprintButton;
    [SerializeField] private Button doubleJumpButton;
    [SerializeField] private Button slideButton;
    [SerializeField] private Button wallRideButton;
    [SerializeField] private Button slowMotionButton;
    [SerializeField] private Button secondGloveButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private Button crossPunchButton;
    [SerializeField] private Button upperCutButton;

    private PlayerInputHandler playerInputHandler;
    private SkillPointManager skillPointManager;
    private TimeManager timeManager;
    private PlayerCharacterController characterController;
    private WallRun wallRun;
    private SpellAttackController spellAttackController;
    private PlayerCombat playerCombat;

    private void Start()
    {
        skillPointManager = gameManager.GetComponent<SkillPointManager>();

        sprintButton.onClick.AddListener(ActivateSprint);
        doubleJumpButton.onClick.AddListener(ActivateDoubleJump);
        slideButton.onClick.AddListener(ActivateSlide);
        wallRideButton.onClick.AddListener(ActivateWallRide);
        slowMotionButton.onClick.AddListener(ActivateSlowMotion);
        secondGloveButton.onClick.AddListener(ActivateSecondGlove);
        shieldButton.onClick.AddListener(ActivateShield);
        crossPunchButton.onClick.AddListener(ActivateCrossPunch);
        upperCutButton.onClick.AddListener(ActivateUpperCut);
    }

    private void ActivateSprint()
    {
        if (skillPointManager.SkillPoints >= 2)
        {
            skillPointManager.SkillPoints -= 2;
            skillPointManager.UpdateSkillPointText();
            playerInputHandler = player.GetComponent<PlayerInputHandler>();
            playerInputHandler.activateSprint = true;
            sprintButton.interactable = false;
        }
    }
    
    private void ActivateDoubleJump()
    {
        if (skillPointManager.SkillPoints >= 4)
        {
            skillPointManager.SkillPoints -= 4;
            skillPointManager.UpdateSkillPointText();
            characterController = player.GetComponent<PlayerCharacterController>();
            characterController.activateDoubleJump = true;   
            doubleJumpButton.interactable = false;
        }
    }
    
    private void ActivateSlide()
    {
        if (skillPointManager.SkillPoints >= 4)
        {
            skillPointManager.SkillPoints -= 4;
            skillPointManager.UpdateSkillPointText();
            playerInputHandler = player.GetComponent<PlayerInputHandler>();
            playerInputHandler.activateSlide = true;
            slideButton.interactable = false;
        }
    }
    
    private void ActivateWallRide()
    {
        if (skillPointManager.SkillPoints >= 6)
        {
            skillPointManager.SkillPoints -= 6;
            skillPointManager.UpdateSkillPointText();
            wallRun = player.GetComponent<WallRun>();
            wallRun.activateWallrun = true;
            wallRideButton.interactable = false;
        }
    }
    
    private void ActivateSlowMotion()
    {
        if (skillPointManager.SkillPoints >= 6)
        {
            skillPointManager.SkillPoints -= 6;
            skillPointManager.UpdateSkillPointText();
            timeManager = gameManager.GetComponent<TimeManager>();
            timeManager.activateSlowMotion = true;
            slowMotionButton.interactable = false;
        }
    }

    private void ActivateSecondGlove()
    {
        if (skillPointManager.SkillPoints >= 2)
        {
            skillPointManager.SkillPoints -= 2;
            skillPointManager.UpdateSkillPointText();
            spellAttackController = player.GetComponent<SpellAttackController>();
            spellAttackController.activateSecondGlove = true;
            secondGloveButton.interactable = false;
        }
    }
    
    private void ActivateShield()
    {
        if (skillPointManager.SkillPoints >= 4)
        {
            skillPointManager.SkillPoints -= 4;
            skillPointManager.UpdateSkillPointText();
            playerCombat = player.GetComponent<PlayerCombat>();
            playerCombat.activateShield = true;
            shieldButton.interactable = false;
        }
    }

    private void ActivateCrossPunch()
    {
        if (skillPointManager.SkillPoints >= 6)
        {
            skillPointManager.SkillPoints -= 6;
            skillPointManager.UpdateSkillPointText();
            playerCombat = player.GetComponent<PlayerCombat>();
            playerCombat.activateCrossPunch = true;
            crossPunchButton.interactable = false;
        }
    }
    
    private void ActivateUpperCut()
    {
        if (skillPointManager.SkillPoints >= 6)
        {
            skillPointManager.SkillPoints -= 6;
            skillPointManager.UpdateSkillPointText();
            playerCombat = player.GetComponent<PlayerCombat>();
            playerCombat.activateUpperCut = true;
            upperCutButton.interactable = false;
        }
    }
}