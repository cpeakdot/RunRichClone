using System;
using DG.Tweening;
using Dreamteck.Splines;
using RRC.Core;
using UnityEngine;

namespace RRC.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SwerveInput swerveInput;
        [SerializeField] private PlayerAnimationHandler playerAnimationHandler;
        [SerializeField] private FinanceHandler financeHandler;
        [SerializeField] private MoneyTextHandler moneyTextHandler;
        [SerializeField] private Transform visualContainer;
        [SerializeField] private GameObject richVisual,averageVisual, poorVisual;
        [SerializeField] private ParticleSystem greenParticle, redParticle, levelUpParticle, levelDownParticle;
        [SerializeField] private ParticleSystem moneyBlastParticle;

        [Header("Values")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float swerveSpeed;
        [SerializeField] private float swerveSmoother = .2f;
        [SerializeField] private float maxXOffset = 2.8f;
        [SerializeField] private float maxSwerveRotation;
        [SerializeField] private Vector3 swerveRotationVector;
        [SerializeField] private float swerveRotationSmoother = .1f;
        
        private PlayerState playerState;

        private const float SCREEN_WIDTH = 1920f;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            splineFollower.followSpeed = moveSpeed;
            financeHandler.OnFinancialStatusChanged += HandleOnFinancialStatusChanged;
        }

        private void Start()
        {
            GameManager.OnGameStateUpdated += HandleOnGameStateChanged;
            splineFollower.spline = SplineManager.Instance.splineComputer;
            splineFollower.spline.triggerGroups[0].triggers[0].AddListener(HandleLevelEnd);
            SwitchVisual(FinanceState.Poor);
        }

        private void Update()
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    HandleSwerve();
                    break;
                case PlayerState.WalkingPoor:
                    HandleSwerve();
                    break;
                case PlayerState.WalkingAverage:
                    HandleSwerve();
                    break;
                case PlayerState.WalkingRich:
                    HandleSwerve();
                    break;
                case PlayerState.Jumping:
                    break;
                case PlayerState.Dancing:
                    break;
                case PlayerState.Crying:
                    break;
                default:
                    break;
            }
        }

        private void OnDisable()
        {
            financeHandler.OnFinancialStatusChanged -= HandleOnFinancialStatusChanged;
            GameManager.OnGameStateUpdated -= HandleOnGameStateChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Collectable collectable))
            {
                return;
            }

            int collectedAmount = collectable.Collect();
            
            financeHandler.UpdateMoney(collectedAmount);
            moneyTextHandler.ShowMoneyText(collectedAmount);

            if (collectedAmount < 0)
            {
                redParticle.Play();
            }
            else if(collectedAmount > 0)
            {
                greenParticle.Play();
                moneyBlastParticle.Play();
            }
        }

        #endregion

        #region Private Methods

        private void SwitchState(PlayerState newState)
        {
            PlayerState lastState = playerState;
            if (newState == playerState)
            {
                return;
            }

            playerState = newState;
            float spinDuration = .5f;
            switch (playerState)
            {
                case PlayerState.Dancing:
                    splineFollower.follow = false;
                    visualContainer.localEulerAngles = Vector3.zero;
                    visualContainer.DOLocalRotate(Vector3.up * 180f, spinDuration).OnComplete(() =>
                    {
                        playerAnimationHandler.SetAnimation(PlayerAnimation.Dance);
                    });
                    break;
                case PlayerState.Crying:
                    splineFollower.follow = false;
                    visualContainer.localEulerAngles = Vector3.zero;
                    visualContainer.DOLocalRotate(Vector3.up * 180f, spinDuration).OnComplete(() =>
                    {
                        playerAnimationHandler.SetAnimation(PlayerAnimation.Cry);
                    });
                    break;
                case PlayerState.Idle:
                    break;
                case PlayerState.WalkingPoor:

                    levelDownParticle.Play();
                    
                    playerAnimationHandler.SetAnimation(PlayerAnimation.JumpSad);
                    playerAnimationHandler.SetAnimation(PlayerAnimation.WalkingP);
                    
                    break;
                case PlayerState.WalkingAverage:
                    
                    playerAnimationHandler.SetAnimation(lastState == PlayerState.WalkingPoor 
                        ? PlayerAnimation.Jump 
                        : PlayerAnimation.JumpSad);
                    
                    if (lastState == PlayerState.WalkingPoor)
                    {
                        levelUpParticle.Play();
                    }
                    else
                    {
                        levelDownParticle.Play();
                    }
                    
                    playerAnimationHandler.SetAnimation(PlayerAnimation.WalkingA);
                    
                    break;
                case PlayerState.WalkingRich:

                    levelUpParticle.Play();
                    
                    playerAnimationHandler.SetAnimation(PlayerAnimation.Jump);
                    playerAnimationHandler.SetAnimation(PlayerAnimation.WalkingR);
                    
                    break;
                case PlayerState.Jumping:
                    break;
                default:
                    break;
            }
        }

        private float CalculateSwerveSpeed()
        {
            float speedFactor = Screen.width / SCREEN_WIDTH;
            return swerveInput.changeOnX * speedFactor;
        }

        private void HandleSwerve()
        {
            float changeOnX = CalculateSwerveSpeed();
            Vector2 offset = splineFollower.motion.offset;
            float targetXOffset = Mathf.Clamp(offset.x + changeOnX * swerveSpeed * Time.deltaTime, -maxXOffset, maxXOffset);
            DOTween.To(() => offset.x, x => offset.x = x, targetXOffset, swerveSmoother).OnUpdate(() =>
            {
                splineFollower.motion.offset = offset;
            });
            HandleSwerveRotation(changeOnX);
        }

        private void HandleSwerveRotation(float changeOnX)
        {
            if (changeOnX > 0)
            {
                visualContainer.DOLocalRotate(swerveRotationVector * maxSwerveRotation, swerveRotationSmoother);
            }
            else if (changeOnX < 0)
            {
                visualContainer.DOLocalRotate(swerveRotationVector * -maxSwerveRotation, swerveRotationSmoother);
            }
            else
            {
                visualContainer.DOLocalRotate(Vector3.zero, swerveRotationSmoother);
            }
        }

        private void HandleOnGameStateChanged(GameState gameState)
        {
            splineFollower.follow = (gameState == GameState.Started);
            if (gameState == GameState.Started)
            {
                playerAnimationHandler.SetAnimation(PlayerAnimation.WalkingP);
                playerState = PlayerState.WalkingPoor;
            }
            else if (gameState == GameState.FinishedL)
            {
                splineFollower.follow = false;
                SwitchState(PlayerState.Crying);
            }
        }

        private void HandleOnFinancialStatusChanged(FinanceState financeState)
        {
            SwitchVisual(financeState);
            switch (financeState)
            {
                case FinanceState.Poor:
                    SwitchState(PlayerState.WalkingPoor);
                    break;
                case FinanceState.Average:
                    SwitchState(PlayerState.WalkingAverage);
                    break;
                case FinanceState.Rich:
                    SwitchState(PlayerState.WalkingRich);
                    break;
                default:
                    break;
            }
        }

        private void SwitchVisual(FinanceState financeState)
        {
            averageVisual.SetActive(false);
            poorVisual.SetActive(false);
            richVisual.SetActive(false);
            
            switch (financeState)
            {
                case FinanceState.Average:
                    averageVisual.SetActive(true);
                    break;
                case FinanceState.Poor:
                    poorVisual.SetActive(true);
                    break;
                case FinanceState.Rich:
                    richVisual.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void HandleLevelEnd()
        {
            if (financeHandler.GetFinanceState == FinanceState.Poor)
            {
                SwitchState(PlayerState.Crying);
            }
            else
            {
                SwitchState(PlayerState.Dancing);
            }
            GameManager.Instance.HandleGameOver(true);
        }

        #endregion
        
    }
}

