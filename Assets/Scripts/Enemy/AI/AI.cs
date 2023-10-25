using IndividualGames.CaseLib.DI;
using System;
using UnityEngine;

namespace IndividualGames.Enemy
{
    /// <summary>
    /// Params class to be passed in states.
    /// </summary>
    public sealed class AIParams
    {
        public EnemyController EnemyController;
        public Animator Animator;

        public AIParams(EnemyController enemyController, Animator _animator)
        {
            EnemyController = enemyController;
            Animator = _animator;
        }
    }

    /// <summary>
    /// Basic FSM for AI.
    /// </summary>
    public class AI : MonoBehaviour, IInitializable
    {
        public enum AIState { Idle, Walk, Run, Attack, Die };

        private Enemy.AIState[] _nodeCache;
        private Enemy.AIState _currentNode;
        private AIParams _aiParams;
        private bool _running = false;

        public bool Initialized
        {
            get { return _initialized; }
            set { }
        }
        private bool _initialized = false;

        public void Init(Container containers)
        {
            _aiParams = (AIParams)containers.Value;

            var stateNames = Enum.GetNames(typeof(AIState));
            _nodeCache = new Enemy.AIState[stateNames.Length];

            for (int i = 0; i < stateNames.Length; i++)
            {
                var typeToCreate = Type.GetType($"{typeof(AI).Namespace}.{stateNames[i]}");
                var newNode = (Enemy.AIState)Activator.CreateInstance(typeToCreate, new object[] { _aiParams });
                newNode.StateChanged.Connect(NewState);
                _nodeCache[i] = newNode;
            }

            NewState(AIState.Idle);

            _running = true;
        }

        private void Update()
        {
            if (_running)
            {
                _currentNode.Tick();
            }
        }

        /// <summary> Change to a new state within the FSM. </summary>
        private void NewState(AIState newState)
        {
            if (_nodeCache.Length == 0)
            {
                throw new NullReferenceException($"{nameof(AI)}: Node Cache is empty.");
            }

            _currentNode = newState switch
            {
                AIState.Idle => _nodeCache[0],
                AIState.Walk => _nodeCache[1],
                AIState.Run => _nodeCache[2],
                AIState.Attack => _nodeCache[3],
                AIState.Die => _nodeCache[4],
                _ => throw new ArgumentException($"{nameof(AI)}: Invalid argument for {newState}")
            };
        }
    }
}