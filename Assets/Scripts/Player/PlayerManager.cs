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
        [SerializeField] private GameObject richVisual, poorVisual;
        [SerializeField] private ParticleSystem greenParticle, redParticle;

        [Header("Values")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float swerveSpeed;
        [SerializeField] private float swerveSmoother = .2f;
        [SerializeField] private float maxXOffset = 2.8f;
        [SerializeField] private float maxSwerveRotation;
        [SerializeField] private Vector3 swerveRotationVector;
        [SerializeField] private float swerveRotationSmoother = .1f;
        
        private PlayerState playerState;
        private bool isRich = false;

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
            SwitchVisual(false);
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
                case PlayerState.WalkingRich:
                    HandleSwerve();
                    break;
                case PlayerState.Jumping:
                    break;
                case PlayerState.Dancing:
                    break;
                default:
                    break;
            }
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
            }
        }

        #endregion

        #region Private Methods

        private void SwitchState(PlayerState newState)
        {
            if (newState == playerState)
            {
                return;
            }

            playerState = newState;
            switch (playerState)
            {
                case PlayerState.Dancing:
                    break;
                case PlayerState.Idle:
                    break;
                case PlayerState.WalkingPoor:
                    playerAnimationHandler.SetAnimation(PlayerAnimation.WalkingP);
                    break;
                case PlayerState.WalkingRich:
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
                SwitchState(PlayerState.WalkingPoor);
            }
        }

        private void HandleOnFinancialStatusChanged(bool gotRich)
        {
            isRich = gotRich;
            if (isRich)
            {
                SwitchVisual(true);
                SwitchState(PlayerState.WalkingRich);
            }
            else
            {
                SwitchVisual(false);
                SwitchState(PlayerState.WalkingPoor);
            }
        }

        private void SwitchVisual(bool rich)
        {
            poorVisual.SetActive(!rich);
            richVisual.SetActive(rich);
        }

        #endregion
        
    }
}

