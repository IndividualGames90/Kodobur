using IndividualGames.CaseLib.Signalization;

namespace IndividualGames.Enemy
{
    /// <summary>
    /// Base AI node for FSM.
    /// </summary>
    public abstract class AIState
    {
        /// <summary> Emits the type of next AIState to change. </summary>
        public readonly BasicSignal<AI.AIState> StateChanged = new();

        protected AIParams _aiParams { get; set; }

        public AIState(AIParams aiParams)
        {
            _aiParams = aiParams;
        }

        /// <summary> Logic runner of the node. </summary>
        public virtual void Tick()
        {
            CheckDie();
        }

        public abstract void Exit();

        public void CheckDie()
        {
            if (_aiParams.EnemyController.IsDead())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Die);
            }
        }
    }

    /// <summary>
    /// Idle State of AI.
    /// </summary>
    public class Idle : AIState
    {
        public Idle(AIParams aiParams) : base(aiParams)
        {
        }

        public override void Exit()
        {
            _aiParams.Animator.SetBool("Idle", false);
        }

        public override void Tick()
        {
            _aiParams.Animator.SetBool("Idle", true);

            if (_aiParams.EnemyController.CanSpotPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Run);
            }

            Exit();
            StateChanged.Emit(AI.AIState.Walk);

            base.Tick();
        }
    }

    /// <summary>
    /// Walk State of AI.
    /// </summary>
    public class Walk : AIState
    {
        public Walk(AIParams aiParams) : base(aiParams)
        {
        }

        public override void Exit()
        {
            _aiParams.EnemyController.StopAgent();
            _aiParams.Animator.SetBool("Walk", false);
        }

        public override void Tick()
        {
            _aiParams.Animator.SetBool("Walk", true);

            if (_aiParams.EnemyController.CanSpotPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Run);
            }

            _aiParams.EnemyController.MoveTowardsNavNode();

            base.Tick();
        }
    }

    /// <summary>
    /// Run State of AI.
    /// </summary>
    public class Run : AIState
    {
        public Run(AIParams aiParams) : base(aiParams)
        {
        }

        public override void Exit()
        {
            _aiParams.Animator.SetBool("Run", false);
        }

        public override void Tick()
        {
            _aiParams.Animator.SetBool("Run", true);

            if (_aiParams.EnemyController.CanAttackPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Attack);
            }
            else if (!_aiParams.EnemyController.CanSpotPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Idle);
            }

            _aiParams.EnemyController.MoveTowardsPlayer();
            _aiParams.EnemyController.RotateTowardsPlayer();

            base.Tick();
        }
    }

    /// <summary>
    /// Attack State of AI.
    /// </summary>
    public class Attack : AIState
    {
        public Attack(AIParams aiParams) : base(aiParams)
        {
        }

        public override void Exit()
        {
            _aiParams.EnemyController.ToggleFireVFX(false);
            _aiParams.Animator.SetBool("Attack", false);
        }

        public override void Tick()
        {
            _aiParams.EnemyController.ToggleFireVFX(true);
            _aiParams.Animator.SetBool("Attack", true);

            if (!_aiParams.EnemyController.CanAttackPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Run);
            }
            else if (!_aiParams.EnemyController.CanSpotPlayer())
            {
                Exit();
                StateChanged.Emit(AI.AIState.Idle);
            }

            _aiParams.EnemyController.RotateTowardsPlayer();

            base.Tick();
        }
    }

    /// <summary>
    /// Die State of AI.
    /// </summary>
    public class Die : AIState
    {
        private bool _isDead = false;

        public Die(AIParams aiParams) : base(aiParams)
        { }

        public override void Exit()
        { }

        public override void Tick()
        {
            if (_isDead)
            {
                return;
            }

            _aiParams.EnemyController.StopAgent();
            _isDead = true;
            _aiParams.Animator.SetTrigger("Die");
        }
    }

}