using System.Collections.Generic;

namespace Game.Features.Tetris
{
    public partial class TetrisContext
    {
        public TetrisDataModule Data { get; private set; }
        public TetrisBoardModule Board { get; private set; }
        public TetrisBlockModule Block { get; private set; }
        public TetrisObjectModule Object { get; private set; }
        public TetrisUIModule UI { get; private set; }
        public TetrisInputModule Input { get; private set; }

        readonly List<TetrisModule> _modules = new(6);

        void InitializeModules()
        {
            Data = RegisterModule(new TetrisDataModule());
            Board = RegisterModule(new TetrisBoardModule());
            Block = RegisterModule(new TetrisBlockModule(RandomSeed >= 0 ? RandomSeed : (int?)null));
            Object = RegisterModule(new TetrisObjectModule());
            UI = RegisterModule(new TetrisUIModule());
            Input = RegisterModule(new TetrisInputModule());

            BeginModules();
        }

        void DisposeModules()
        {
            for (var i = _modules.Count - 1; i >= 0; i--)
            {
                _modules[i].End();
            }

            _modules.Clear();
        }

        T RegisterModule<T>(T module) where T : TetrisModule
        {
            module.Register(this);
            _modules.Add(module);
            return module;
        }

        void BeginModules()
        {
            foreach (var module in _modules)
            {
                module.Begin();
            }
        }

        void ResetModules()
        {
            foreach (var module in _modules)
            {
                module.Reset();
            }
        }
    }
}
