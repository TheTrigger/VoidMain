using System;
using System.Collections.Generic;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.Console
{
    public class ConsoleKeyConverter
    {
        private readonly Dictionary<ConsoleKey, Key> _keys;

        public ConsoleKeyConverter()
        {
            _keys = new Dictionary<ConsoleKey, Key>
            {
                [ConsoleKey.D0] = Key.D0,
                [ConsoleKey.D1] = Key.D1,
                [ConsoleKey.D2] = Key.D2,
                [ConsoleKey.D3] = Key.D3,
                [ConsoleKey.D4] = Key.D4,
                [ConsoleKey.D5] = Key.D5,
                [ConsoleKey.D6] = Key.D6,
                [ConsoleKey.D7] = Key.D7,
                [ConsoleKey.D8] = Key.D8,
                [ConsoleKey.D9] = Key.D9,
                [ConsoleKey.A] = Key.A,
                [ConsoleKey.B] = Key.B,
                [ConsoleKey.C] = Key.C,
                [ConsoleKey.D] = Key.D,
                [ConsoleKey.E] = Key.E,
                [ConsoleKey.F] = Key.F,
                [ConsoleKey.G] = Key.G,
                [ConsoleKey.H] = Key.H,
                [ConsoleKey.I] = Key.I,
                [ConsoleKey.J] = Key.J,
                [ConsoleKey.K] = Key.K,
                [ConsoleKey.L] = Key.L,
                [ConsoleKey.M] = Key.M,
                [ConsoleKey.N] = Key.N,
                [ConsoleKey.O] = Key.O,
                [ConsoleKey.P] = Key.P,
                [ConsoleKey.Q] = Key.Q,
                [ConsoleKey.R] = Key.R,
                [ConsoleKey.S] = Key.S,
                [ConsoleKey.T] = Key.T,
                [ConsoleKey.U] = Key.U,
                [ConsoleKey.V] = Key.V,
                [ConsoleKey.W] = Key.W,
                [ConsoleKey.X] = Key.X,
                [ConsoleKey.Y] = Key.Y,
                [ConsoleKey.Z] = Key.Z,
                [ConsoleKey.NumPad0] = Key.NumPad0,
                [ConsoleKey.NumPad1] = Key.NumPad1,
                [ConsoleKey.NumPad2] = Key.NumPad2,
                [ConsoleKey.NumPad3] = Key.NumPad3,
                [ConsoleKey.NumPad4] = Key.NumPad4,
                [ConsoleKey.NumPad5] = Key.NumPad5,
                [ConsoleKey.NumPad6] = Key.NumPad6,
                [ConsoleKey.NumPad7] = Key.NumPad7,
                [ConsoleKey.NumPad8] = Key.NumPad8,
                [ConsoleKey.NumPad9] = Key.NumPad9,
                [ConsoleKey.Multiply] = Key.Multiply,
                [ConsoleKey.Add] = Key.Add,
                [ConsoleKey.Separator] = Key.Separator,
                [ConsoleKey.Subtract] = Key.Subtract,
                [ConsoleKey.Decimal] = Key.Decimal,
                [ConsoleKey.Divide] = Key.Divide,
                [ConsoleKey.F1] = Key.F1,
                [ConsoleKey.F2] = Key.F2,
                [ConsoleKey.F3] = Key.F3,
                [ConsoleKey.F4] = Key.F4,
                [ConsoleKey.F5] = Key.F5,
                [ConsoleKey.F6] = Key.F6,
                [ConsoleKey.F7] = Key.F7,
                [ConsoleKey.F8] = Key.F8,
                [ConsoleKey.F9] = Key.F9,
                [ConsoleKey.F10] = Key.F10,
                [ConsoleKey.F11] = Key.F11,
                [ConsoleKey.F12] = Key.F12,
                [ConsoleKey.F13] = Key.F13,
                [ConsoleKey.F14] = Key.F14,
                [ConsoleKey.F15] = Key.F15,
                [ConsoleKey.F16] = Key.F16,
                [ConsoleKey.F17] = Key.F17,
                [ConsoleKey.F18] = Key.F18,
                [ConsoleKey.F19] = Key.F19,
                [ConsoleKey.F20] = Key.F20,
                [ConsoleKey.F21] = Key.F21,
                [ConsoleKey.F22] = Key.F22,
                [ConsoleKey.F23] = Key.F23,
                [ConsoleKey.F24] = Key.F24,
                [ConsoleKey.LeftArrow] = Key.LeftArrow,
                [ConsoleKey.UpArrow] = Key.UpArrow,
                [ConsoleKey.RightArrow] = Key.RightArrow,
                [ConsoleKey.DownArrow] = Key.DownArrow,
                [ConsoleKey.Insert] = Key.Insert,
                [ConsoleKey.Delete] = Key.Delete,
                [ConsoleKey.End] = Key.End,
                [ConsoleKey.Home] = Key.Home,
                [ConsoleKey.PageUp] = Key.PageUp,
                [ConsoleKey.PageDown] = Key.PageDown,
                [ConsoleKey.Backspace] = Key.Backspace,
                [ConsoleKey.Tab] = Key.Tab,
                [ConsoleKey.Spacebar] = Key.Spacebar,
                [ConsoleKey.Enter] = Key.Enter,
                [ConsoleKey.Escape] = Key.Escape,
                [ConsoleKey.Sleep] = Key.Sleep,
                [ConsoleKey.PrintScreen] = Key.PrintScreen,
                [ConsoleKey.LeftWindows] = Key.LeftWindows,
                [ConsoleKey.RightWindows] = Key.RightWindows,
                [ConsoleKey.Applications] = Key.Applications,
                [ConsoleKey.MediaNext] = Key.MediaNext,
                [ConsoleKey.MediaPrevious] = Key.MediaPrevious,
                [ConsoleKey.MediaStop] = Key.MediaStop,
                [ConsoleKey.MediaPlay] = Key.MediaPlay,
                [ConsoleKey.VolumeMute] = Key.VolumeMute,
                [ConsoleKey.VolumeDown] = Key.VolumeDown,
                [ConsoleKey.VolumeUp] = Key.VolumeUp,
                [ConsoleKey.LaunchApp1] = Key.LaunchApp1,
                [ConsoleKey.LaunchApp2] = Key.LaunchApp2,
                [ConsoleKey.LaunchMail] = Key.LaunchMail,
                [ConsoleKey.BrowserBack] = Key.BrowserBack,
                [ConsoleKey.BrowserForward] = Key.BrowserForward,
                [ConsoleKey.BrowserRefresh] = Key.BrowserRefresh,
                [ConsoleKey.BrowserStop] = Key.BrowserStop,
                [ConsoleKey.BrowserSearch] = Key.BrowserSearch,
                [ConsoleKey.BrowserFavorites] = Key.BrowserFavorites,
                [ConsoleKey.BrowserHome] = Key.BrowserHome,
                [ConsoleKey.Oem1] = Key.OemSemicolon,
                [ConsoleKey.Oem2] = Key.OemQuestion,
                [ConsoleKey.Oem3] = Key.OemTilde,
                [ConsoleKey.Oem4] = Key.OemOpenBrackets,
                [ConsoleKey.Oem5] = Key.OemPipe,
                [ConsoleKey.Oem6] = Key.OemCloseBrackets,
                [ConsoleKey.Oem7] = Key.OemQuotes,
                [ConsoleKey.Oem102] = Key.OemBackslash,
                [ConsoleKey.OemPlus] = Key.OemPlus,
                [ConsoleKey.OemComma] = Key.OemComma,
                [ConsoleKey.OemMinus] = Key.OemMinus,
                [ConsoleKey.OemPeriod] = Key.OemPeriod
            };
        }

        public Key ConvertKey(ConsoleKey key)
        {
            if (!_keys.TryGetValue(key, out var result))
            {
                result = Key.Unknown;
            }
            return result;
        }

        public KeyModifiers ConvertModifiers(ConsoleModifiers modifiers)
        {
            var result = KeyModifiers.None;

            if (modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                result |= KeyModifiers.Alt;
            }
            if (modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                result |= KeyModifiers.Shift;
            }
            if (modifiers.HasFlag(ConsoleModifiers.Control))
            {
                result |= KeyModifiers.Control;
            }

            return result;
        }
    }
}
