using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class ConsoleKeyConverter : IConsoleKeyConverter
    {
        private readonly Dictionary<ConsoleKey, InputKey> _keys;

        public ConsoleKeyConverter()
        {
            _keys = new Dictionary<ConsoleKey, InputKey>
            {
                [ConsoleKey.D0] = InputKey.D0,
                [ConsoleKey.D1] = InputKey.D1,
                [ConsoleKey.D2] = InputKey.D2,
                [ConsoleKey.D3] = InputKey.D3,
                [ConsoleKey.D4] = InputKey.D4,
                [ConsoleKey.D5] = InputKey.D5,
                [ConsoleKey.D6] = InputKey.D6,
                [ConsoleKey.D7] = InputKey.D7,
                [ConsoleKey.D8] = InputKey.D8,
                [ConsoleKey.D9] = InputKey.D9,
                [ConsoleKey.A] = InputKey.A,
                [ConsoleKey.B] = InputKey.B,
                [ConsoleKey.C] = InputKey.C,
                [ConsoleKey.D] = InputKey.D,
                [ConsoleKey.E] = InputKey.E,
                [ConsoleKey.F] = InputKey.F,
                [ConsoleKey.G] = InputKey.G,
                [ConsoleKey.H] = InputKey.H,
                [ConsoleKey.I] = InputKey.I,
                [ConsoleKey.J] = InputKey.J,
                [ConsoleKey.K] = InputKey.K,
                [ConsoleKey.L] = InputKey.L,
                [ConsoleKey.M] = InputKey.M,
                [ConsoleKey.N] = InputKey.N,
                [ConsoleKey.O] = InputKey.O,
                [ConsoleKey.P] = InputKey.P,
                [ConsoleKey.Q] = InputKey.Q,
                [ConsoleKey.R] = InputKey.R,
                [ConsoleKey.S] = InputKey.S,
                [ConsoleKey.T] = InputKey.T,
                [ConsoleKey.U] = InputKey.U,
                [ConsoleKey.V] = InputKey.V,
                [ConsoleKey.W] = InputKey.W,
                [ConsoleKey.X] = InputKey.X,
                [ConsoleKey.Y] = InputKey.Y,
                [ConsoleKey.Z] = InputKey.Z,
                [ConsoleKey.NumPad0] = InputKey.NumPad0,
                [ConsoleKey.NumPad1] = InputKey.NumPad1,
                [ConsoleKey.NumPad2] = InputKey.NumPad2,
                [ConsoleKey.NumPad3] = InputKey.NumPad3,
                [ConsoleKey.NumPad4] = InputKey.NumPad4,
                [ConsoleKey.NumPad5] = InputKey.NumPad5,
                [ConsoleKey.NumPad6] = InputKey.NumPad6,
                [ConsoleKey.NumPad7] = InputKey.NumPad7,
                [ConsoleKey.NumPad8] = InputKey.NumPad8,
                [ConsoleKey.NumPad9] = InputKey.NumPad9,
                [ConsoleKey.Multiply] = InputKey.Multiply,
                [ConsoleKey.Add] = InputKey.Add,
                [ConsoleKey.Separator] = InputKey.Separator,
                [ConsoleKey.Subtract] = InputKey.Subtract,
                [ConsoleKey.Decimal] = InputKey.Decimal,
                [ConsoleKey.Divide] = InputKey.Divide,
                [ConsoleKey.F1] = InputKey.F1,
                [ConsoleKey.F2] = InputKey.F2,
                [ConsoleKey.F3] = InputKey.F3,
                [ConsoleKey.F4] = InputKey.F4,
                [ConsoleKey.F5] = InputKey.F5,
                [ConsoleKey.F6] = InputKey.F6,
                [ConsoleKey.F7] = InputKey.F7,
                [ConsoleKey.F8] = InputKey.F8,
                [ConsoleKey.F9] = InputKey.F9,
                [ConsoleKey.F10] = InputKey.F10,
                [ConsoleKey.F11] = InputKey.F11,
                [ConsoleKey.F12] = InputKey.F12,
                [ConsoleKey.F13] = InputKey.F13,
                [ConsoleKey.F14] = InputKey.F14,
                [ConsoleKey.F15] = InputKey.F15,
                [ConsoleKey.F16] = InputKey.F16,
                [ConsoleKey.F17] = InputKey.F17,
                [ConsoleKey.F18] = InputKey.F18,
                [ConsoleKey.F19] = InputKey.F19,
                [ConsoleKey.F20] = InputKey.F20,
                [ConsoleKey.F21] = InputKey.F21,
                [ConsoleKey.F22] = InputKey.F22,
                [ConsoleKey.F23] = InputKey.F23,
                [ConsoleKey.F24] = InputKey.F24,
                [ConsoleKey.LeftArrow] = InputKey.LeftArrow,
                [ConsoleKey.UpArrow] = InputKey.UpArrow,
                [ConsoleKey.RightArrow] = InputKey.RightArrow,
                [ConsoleKey.DownArrow] = InputKey.DownArrow,
                [ConsoleKey.Insert] = InputKey.Insert,
                [ConsoleKey.Delete] = InputKey.Delete,
                [ConsoleKey.End] = InputKey.End,
                [ConsoleKey.Home] = InputKey.Home,
                [ConsoleKey.PageUp] = InputKey.PageUp,
                [ConsoleKey.PageDown] = InputKey.PageDown,
                [ConsoleKey.Backspace] = InputKey.Backspace,
                [ConsoleKey.Tab] = InputKey.Tab,
                [ConsoleKey.Spacebar] = InputKey.Spacebar,
                [ConsoleKey.Enter] = InputKey.Enter,
                [ConsoleKey.Escape] = InputKey.Escape,
                [ConsoleKey.Sleep] = InputKey.Sleep,
                [ConsoleKey.PrintScreen] = InputKey.PrintScreen,
                //[ConsoleKey.LeftWindows] = InputKey.LeftWindows,
                //[ConsoleKey.RightWindows] = InputKey.RightWindows,
                //[ConsoleKey.Applications] = InputKey.Applications,
                //[ConsoleKey.MediaNext] = InputKey.MediaNext,
                //[ConsoleKey.MediaPrevious] = InputKey.MediaPrevious,
                //[ConsoleKey.MediaStop] = InputKey.MediaStop,
                //[ConsoleKey.MediaPlay] = InputKey.MediaPlay,
                //[ConsoleKey.VolumeMute] = InputKey.VolumeMute,
                //[ConsoleKey.VolumeDown] = InputKey.VolumeDown,
                //[ConsoleKey.VolumeUp] = InputKey.VolumeUp,
                //[ConsoleKey.LaunchApp1] = InputKey.LaunchApp1,
                //[ConsoleKey.LaunchApp2] = InputKey.LaunchApp2,
                //[ConsoleKey.LaunchMail] = InputKey.LaunchMail,
                //[ConsoleKey.BrowserBack] = InputKey.BrowserBack,
                //[ConsoleKey.BrowserForward] = InputKey.BrowserForward,
                //[ConsoleKey.BrowserRefresh] = InputKey.BrowserRefresh,
                //[ConsoleKey.BrowserStop] = InputKey.BrowserStop,
                //[ConsoleKey.BrowserSearch] = InputKey.BrowserSearch,
                //[ConsoleKey.BrowserFavorites] = InputKey.BrowserFavorites,
                //[ConsoleKey.BrowserHome] = InputKey.BrowserHome,
                [ConsoleKey.Oem1] = InputKey.OemSemicolon,
                [ConsoleKey.Oem2] = InputKey.OemQuestion,
                [ConsoleKey.Oem3] = InputKey.OemTilde,
                [ConsoleKey.Oem4] = InputKey.OemOpenBrackets,
                [ConsoleKey.Oem5] = InputKey.OemPipe,
                [ConsoleKey.Oem6] = InputKey.OemCloseBrackets,
                [ConsoleKey.Oem7] = InputKey.OemQuotes,
                //[ConsoleKey.Oem102] = InputKey.OemBackslash,
                [ConsoleKey.OemPlus] = InputKey.OemPlus,
                [ConsoleKey.OemComma] = InputKey.OemComma,
                [ConsoleKey.OemMinus] = InputKey.OemMinus,
                [ConsoleKey.OemPeriod] = InputKey.OemPeriod
            };
        }

        public InputKey ConvertKey(ConsoleKey key)
        {
            if (!_keys.TryGetValue(key, out var inputKey))
            {
                inputKey = InputKey.Unknown;
            }
            return inputKey;
        }

        public InputModifiers ConvertModifiers(ConsoleModifiers modifiers)
        {
            InputModifiers inputModifiers = InputModifiers.None;

            if ((modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
            {
                inputModifiers |= InputModifiers.Alt;
            }
            if ((modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
            {
                inputModifiers |= InputModifiers.Shift;
            }
            if ((modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
            {
                inputModifiers |= InputModifiers.Control;
            }

            return inputModifiers;
        }
    }
}
