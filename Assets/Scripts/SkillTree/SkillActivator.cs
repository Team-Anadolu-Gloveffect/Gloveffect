using Player;
using UnityEngine;
using UnityEngine.UI;

public class SkillActivator : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    
    private Button sprintButton;
    private Button doubleJumpButton;
    private Button slideButton;
    private Button wallRideButton;
    private Button slowMotionButton;

    private PlayerInputHandler playerInputHandler;
    private SkillPointManager skillPointManager;
    private TimeManager timeManager;
    private PlayerCharacterController characterController;
    private WallRun wallRun;

    private void Start()
    {
        skillPointManager = GameObject.Find("GameManager").GetComponent<SkillPointManager>();
        
        sprintButton = GameObject.Find("SprintSkill").GetComponent<Button>();
        sprintButton.onClick.AddListener(ActivateSprint);
        
        doubleJumpButton = GameObject.Find("DoubleJumpSkill").GetComponent<Button>();
        doubleJumpButton.onClick.AddListener(ActivateDoubleJump);
        
        slideButton = GameObject.Find("SlideSkill").GetComponent<Button>();
        slideButton.onClick.AddListener(ActivateSlide);
        
        wallRideButton = GameObject.Find("WallRideSkill").GetComponent<Button>();
        wallRideButton.onClick.AddListener(ActivateWallRide);
        
        slowMotionButton = GameObject.Find("SlowMotionSkill").GetComponent<Button>();
        slowMotionButton.onClick.AddListener(ActivateSlowMotion);
    }

    private void ActivateSprint()
    {
        if (skillPointManager.skillPoints >= 2)
        {
            skillPointManager.skillPoints -= 2;
            playerInputHandler = player.GetComponent<PlayerInputHandler>();
            playerInputHandler.activateSprint = true;
            sprintButton.interactable = false;
        }
    }
    
    private void ActivateDoubleJump()
    {
        if (skillPointManager.skillPoints >= 4)
        {
            skillPointManager.skillPoints -= 4;
            characterController = player.GetComponent<PlayerCharacterController>();
            characterController.activateDoubleJump = true;   
            doubleJumpButton.interactable = false;
        }
    }
    
    private void ActivateSlide()
    {
        if (skillPointManager.skillPoints >= 4)
        {
            skillPointManager.skillPoints -= 4;
            playerInputHandler = player.GetComponent<PlayerInputHandler>();
            playerInputHandler.activateSlide = true;
            slideButton.interactable = false;
        }
    }
    
    private void ActivateWallRide()
    {
        if (skillPointManager.skillPoints >= 6)
        {
            skillPointManager.skillPoints -= 6;
            wallRun = player.GetComponent<WallRun>();
            wallRun.activateWallrun = true;
            wallRideButton.interactable = false;
        }
    }
    
    private void ActivateSlowMotion()
    {
        if (skillPointManager.skillPoints >= 6)
        {
            skillPointManager.skillPoints -= 6;
            timeManager = gameManager.GetComponent<TimeManager>();
            timeManager.activateSlowMotion = true;
            slowMotionButton.interactable = false;
        }
    }
}