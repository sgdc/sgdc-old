using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SGDE.SceneManagement;
using SGDE.Content;

namespace SGDE
{
    partial class Entity
    {
        /// <summary>
        /// For handling xbox controller input.
        /// </summary>
        protected class GamePadComponent
        {
            private Entity _objReference;
            private PlayerIndex _playerIndex = 0;
            private GamePadState gamePadState;
            private List<GamePadEvent> gamePadEvents;

            public GamePadComponent(Entity objReference, PlayerIndex playerIndex)
            {
                _objReference = objReference;
                _playerIndex = playerIndex;
                if (ContentUtil.LoadingBuilders)
                {
                    return;
                }
                gamePadState = new GamePadState();
                gamePadEvents = new List<GamePadEvent>();
            }

            public void RegisterEvent(GamePadEvent gamePadEvent)
            {
                gamePadEvents.Add(gamePadEvent);
            }

            public void Update()
            {
                gamePadState = GamePad.GetState(_playerIndex);

                for (int i = 0; i < gamePadEvents.Count; i++)
                {
                    gamePadEvents[i].HandleEvents(gamePadState);
                }
            }
        }
    }
}