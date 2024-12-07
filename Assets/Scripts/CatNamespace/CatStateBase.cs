namespace CatNamespace
{
    public abstract class CatStateBase
    {
        protected Cat cat;
        protected CatStateMachine stateMachine;

        protected CatStateBase(Cat cat, CatStateMachine stateMachine)
        {
            this.cat = cat;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}