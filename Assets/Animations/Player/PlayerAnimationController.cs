using Player;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimationState currentAnimation;
    [Header("References")]
    [SerializeField] private WallRun wallRun;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private SpellAttackController spellAttackController;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private PlayerCombat playerCombat;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentAnimation = PlayerAnimationState.Idle;
    }

    private void Update()
    {
        CheckActionInput();
    }

    private void CheckActionInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayAnimation(PlayerAnimationState.Jump);
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            PlayAnimation(PlayerAnimationState.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            PlayAnimation(PlayerAnimationState.Crouch);
        else if (Input.GetKeyDown(KeyCode.C) && playerInputHandler.activateSlide)
            PlayAnimation(PlayerAnimationState.Slide);
        
        else if (Input.GetKeyDown(KeyCode.H) && playerCombat.activateUpperCut)
            PlayAnimation(PlayerAnimationState.Uppercut);
        else if (Input.GetKeyDown(KeyCode.F) && playerCombat.activateCrossPunch)
            PlayAnimation(PlayerAnimationState.Crosspunch);
        
        else if (Input.GetKeyDown(KeyCode.Mouse0) && progressBar.current > 0)
            PlayAnimation(PlayerAnimationState.FireLeft);
        else if (Input.GetKeyDown(KeyCode.Mouse1) && progressBar.current > 0 && spellAttackController.activateSecondGlove)
            PlayAnimation(PlayerAnimationState.FireRight);
        else if (wallRun.isWallRunning && wallRun.GetSide() < 0)
            PlayAnimation(PlayerAnimationState.WallLeft);
        else if (wallRun.isWallRunning && wallRun.GetSide() > 0)
            PlayAnimation(PlayerAnimationState.WallRight);
    }

    private void PlayAnimation(PlayerAnimationState animation)
    {
        animator.ResetTrigger(currentAnimation.ToString());
        animator.SetTrigger(animation.ToString());
        currentAnimation = animation;
    }
}