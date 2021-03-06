﻿using Myre.UI.InputDevices;

namespace Myre.UI.Gestures
{
    public class MousePressedGesture
        : Gesture<MouseDevice>
    {
        public MouseButtons Button { get; private set; }

        public MousePressedGesture(MouseButtons button)
            : base(false)
        {
            Button = button;
            BlockedInputs.Add((int)Button);
        }

        protected override bool Test(MouseDevice device)
        {
            return device.IsButtonNewlyDown(Button);
        }
    }
}
