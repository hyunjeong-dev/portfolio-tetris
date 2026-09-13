namespace Game.Features.Tetris
{
    public abstract class TetrisModule
    {
        protected TetrisContext Context { get; private set; }

        public void Register(TetrisContext context)
        {
            Context = context;
            OnRegister();
        }

        public void Begin()
        {
            OnBegin();
        }

        public void End()
        {
            OnEnd();
            Context = null;
        }

        public virtual void Reset()
        {
        }

        protected virtual void OnRegister()
        {
        }

        protected virtual void OnBegin()
        {
        }

        protected virtual void OnEnd()
        {
        }
    }
}
