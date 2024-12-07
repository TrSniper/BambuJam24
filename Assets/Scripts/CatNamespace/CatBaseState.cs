using ScriptableObjects;

namespace CatNamespace
{
    public abstract class CatBaseState
    {
        protected Cat cat;
        protected GameConstants gameConstants;
        protected CatStateMachine stateMachine;

        protected CatBaseState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine)
        {
            this.cat = cat;
            this.gameConstants = gameConstants;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual bool Exit() => true;
    }
}