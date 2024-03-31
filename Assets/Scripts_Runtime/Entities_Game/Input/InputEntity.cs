using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Air {

    public class InputEntity {

        public Vector2 moveAxis;
        InputKeybindingComponent keybindingCom;

        public void Ctor() {
            keybindingCom.Ctor();
        }

        public void ProcessInput(Camera camera, float dt) {

            if (keybindingCom.IsKeyPressing(InputKeyEnum.TurnLeft)) {
                moveAxis.x = -1;
            } else if (keybindingCom.IsKeyPressing(InputKeyEnum.TurnRight)) {
                moveAxis.x = 1;
            }

        }

        public void Keybinding_Set(InputKeyEnum key, KeyCode[] keyCodes) {
            keybindingCom.Bind(key, keyCodes);
        }

        public void Reset() {
            moveAxis = Vector2.zero;
        }

    }

}