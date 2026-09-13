namespace Game.Features.Tetris
{
    public class TetrisUIModule : TetrisModule
    {
        UITetrisHud _hud;

        protected override void OnRegister()
        {
            _hud = Context.Hud;
        }

        protected override void OnBegin()
        {
            if (_hud != null)
            {
                _hud.Bind(Context);
            }
        }

        public void Tick()
        {
            if (_hud == null)
            {
                return;
            }
            _hud.Refresh(Context);
        }

        protected override void OnEnd()
        {
            if (_hud != null)
            {
                _hud.Unbind();
            }
        }
    }
}
