namespace CatNamespace
{
    public abstract class CatBaseState
    {
        protected Cat cat;
        protected CatStateMachine stateMachine;

        protected CatBaseState(Cat cat, CatStateMachine stateMachine)
        {
            this.cat = cat;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual bool Exit() => true;
    }
}