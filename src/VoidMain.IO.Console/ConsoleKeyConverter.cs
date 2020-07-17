using System;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.Console
{
    public class ConsoleKeyConverter
    {
        private readonly Key[] _keys;
        private readonly KeyModifiers[] _modifiers;

        public ConsoleKeyConverter()
        {
            _keys = new Key[256];
            Array.Fill(_keys, Key.Unknown);

            _keys[(int)ConsoleKey.D0] = Key.D0;
            _keys[(int)ConsoleKey.D1] = Key.D1;
            _keys[(int)ConsoleKey.D2] = Key.D2;
            _keys[(int)ConsoleKey.D3] = Key.D3;
            _keys[(int)ConsoleKey.D4] = Key.D4;
            _keys[(int)ConsoleKey.D5] = Key.D5;
            _keys[(int)ConsoleKey.D6] = Key.D6;
            _keys[(int)ConsoleKey.D7] = Key.D7;
            _keys[(int)ConsoleKey.D8] = Key.D8;
            _keys[(int)ConsoleKey.D9] = Key.D9;
            _keys[(int)ConsoleKey.A] = Key.A;
            _keys[(int)ConsoleKey.B] = Key.B;
            _keys[(int)ConsoleKey.C] = Key.C;
            _keys[(int)ConsoleKey.D] = Key.D;
            _keys[(int)ConsoleKey.E] = Key.E;
            _keys[(int)ConsoleKey.F] = Key.F;
            _keys[(int)ConsoleKey.G] = Key.G;
            _keys[(int)ConsoleKey.H] = Key.H;
            _keys[(int)ConsoleKey.I] = Key.I;
            _keys[(int)ConsoleKey.J] = Key.J;
            _keys[(int)ConsoleKey.K] = Key.K;
            _keys[(int)ConsoleKey.L] = Key.L;
            _keys[(int)ConsoleKey.M] = Key.M;
            _keys[(int)ConsoleKey.N] = Key.N;
            _keys[(int)ConsoleKey.O] = Key.O;
            _keys[(int)ConsoleKey.P] = Key.P;
            _keys[(int)ConsoleKey.Q] = Key.Q;
            _keys[(int)ConsoleKey.R] = Key.R;
            _keys[(int)ConsoleKey.S] = Key.S;
            _keys[(int)ConsoleKey.T] = Key.T;
            _keys[(int)ConsoleKey.U] = Key.U;
            _keys[(int)ConsoleKey.V] = Key.V;
            _keys[(int)ConsoleKey.W] = Key.W;
            _keys[(int)ConsoleKey.X] = Key.X;
            _keys[(int)ConsoleKey.Y] = Key.Y;
            _keys[(int)ConsoleKey.Z] = Key.Z;
            _keys[(int)ConsoleKey.NumPad0] = Key.NumPad0;
            _keys[(int)ConsoleKey.NumPad1] = Key.NumPad1;
            _keys[(int)ConsoleKey.NumPad2] = Key.NumPad2;
            _keys[(int)ConsoleKey.NumPad3] = Key.NumPad3;
            _keys[(int)ConsoleKey.NumPad4] = Key.NumPad4;
            _keys[(int)ConsoleKey.NumPad5] = Key.NumPad5;
            _keys[(int)ConsoleKey.NumPad6] = Key.NumPad6;
            _keys[(int)ConsoleKey.NumPad7] = Key.NumPad7;
            _keys[(int)ConsoleKey.NumPad8] = Key.NumPad8;
            _keys[(int)ConsoleKey.NumPad9] = Key.NumPad9;
            _keys[(int)ConsoleKey.Multiply] = Key.Multiply;
            _keys[(int)ConsoleKey.Add] = Key.Add;
            _keys[(int)ConsoleKey.Separator] = Key.Separator;
            _keys[(int)ConsoleKey.Subtract] = Key.Subtract;
            _keys[(int)ConsoleKey.Decimal] = Key.Decimal;
            _keys[(int)ConsoleKey.Divide] = Key.Divide;
            _keys[(int)ConsoleKey.F1] = Key.F1;
            _keys[(int)ConsoleKey.F2] = Key.F2;
            _keys[(int)ConsoleKey.F3] = Key.F3;
            _keys[(int)ConsoleKey.F4] = Key.F4;
            _keys[(int)ConsoleKey.F5] = Key.F5;
            _keys[(int)ConsoleKey.F6] = Key.F6;
            _keys[(int)ConsoleKey.F7] = Key.F7;
            _keys[(int)ConsoleKey.F8] = Key.F8;
            _keys[(int)ConsoleKey.F9] = Key.F9;
            _keys[(int)ConsoleKey.F10] = Key.F10;
            _keys[(int)ConsoleKey.F11] = Key.F11;
            _keys[(int)ConsoleKey.F12] = Key.F12;
            _keys[(int)ConsoleKey.F13] = Key.F13;
            _keys[(int)ConsoleKey.F14] = Key.F14;
            _keys[(int)ConsoleKey.F15] = Key.F15;
            _keys[(int)ConsoleKey.F16] = Key.F16;
            _keys[(int)ConsoleKey.F17] = Key.F17;
            _keys[(int)ConsoleKey.F18] = Key.F18;
            _keys[(int)ConsoleKey.F19] = Key.F19;
            _keys[(int)ConsoleKey.F20] = Key.F20;
            _keys[(int)ConsoleKey.F21] = Key.F21;
            _keys[(int)ConsoleKey.F22] = Key.F22;
            _keys[(int)ConsoleKey.F23] = Key.F23;
            _keys[(int)ConsoleKey.F24] = Key.F24;
            _keys[(int)ConsoleKey.LeftArrow] = Key.LeftArrow;
            _keys[(int)ConsoleKey.UpArrow] = Key.UpArrow;
            _keys[(int)ConsoleKey.RightArrow] = Key.RightArrow;
            _keys[(int)ConsoleKey.DownArrow] = Key.DownArrow;
            _keys[(int)ConsoleKey.Insert] = Key.Insert;
            _keys[(int)ConsoleKey.Delete] = Key.Delete;
            _keys[(int)ConsoleKey.End] = Key.End;
            _keys[(int)ConsoleKey.Home] = Key.Home;
            _keys[(int)ConsoleKey.PageUp] = Key.PageUp;
            _keys[(int)ConsoleKey.PageDown] = Key.PageDown;
            _keys[(int)ConsoleKey.Backspace] = Key.Backspace;
            _keys[(int)ConsoleKey.Tab] = Key.Tab;
            _keys[(int)ConsoleKey.Spacebar] = Key.Spacebar;
            _keys[(int)ConsoleKey.Enter] = Key.Enter;
            _keys[(int)ConsoleKey.Escape] = Key.Escape;
            _keys[(int)ConsoleKey.Sleep] = Key.Sleep;
            _keys[(int)ConsoleKey.PrintScreen] = Key.PrintScreen;
            _keys[(int)ConsoleKey.LeftWindows] = Key.LeftWindows;
            _keys[(int)ConsoleKey.RightWindows] = Key.RightWindows;
            _keys[(int)ConsoleKey.Applications] = Key.Applications;
            _keys[(int)ConsoleKey.MediaNext] = Key.MediaNext;
            _keys[(int)ConsoleKey.MediaPrevious] = Key.MediaPrevious;
            _keys[(int)ConsoleKey.MediaStop] = Key.MediaStop;
            _keys[(int)ConsoleKey.MediaPlay] = Key.MediaPlay;
            _keys[(int)ConsoleKey.VolumeMute] = Key.VolumeMute;
            _keys[(int)ConsoleKey.VolumeDown] = Key.VolumeDown;
            _keys[(int)ConsoleKey.VolumeUp] = Key.VolumeUp;
            _keys[(int)ConsoleKey.LaunchApp1] = Key.LaunchApp1;
            _keys[(int)ConsoleKey.LaunchApp2] = Key.LaunchApp2;
            _keys[(int)ConsoleKey.LaunchMail] = Key.LaunchMail;
            _keys[(int)ConsoleKey.BrowserBack] = Key.BrowserBack;
            _keys[(int)ConsoleKey.BrowserForward] = Key.BrowserForward;
            _keys[(int)ConsoleKey.BrowserRefresh] = Key.BrowserRefresh;
            _keys[(int)ConsoleKey.BrowserStop] = Key.BrowserStop;
            _keys[(int)ConsoleKey.BrowserSearch] = Key.BrowserSearch;
            _keys[(int)ConsoleKey.BrowserFavorites] = Key.BrowserFavorites;
            _keys[(int)ConsoleKey.BrowserHome] = Key.BrowserHome;
            _keys[(int)ConsoleKey.Oem1] = Key.OemSemicolon;
            _keys[(int)ConsoleKey.Oem2] = Key.OemQuestion;
            _keys[(int)ConsoleKey.Oem3] = Key.OemTilde;
            _keys[(int)ConsoleKey.Oem4] = Key.OemOpenBrackets;
            _keys[(int)ConsoleKey.Oem5] = Key.OemPipe;
            _keys[(int)ConsoleKey.Oem6] = Key.OemCloseBrackets;
            _keys[(int)ConsoleKey.Oem7] = Key.OemQuotes;
            _keys[(int)ConsoleKey.Oem102] = Key.OemBackslash;
            _keys[(int)ConsoleKey.OemPlus] = Key.OemPlus;
            _keys[(int)ConsoleKey.OemComma] = Key.OemComma;
            _keys[(int)ConsoleKey.OemMinus] = Key.OemMinus;
            _keys[(int)ConsoleKey.OemPeriod] = Key.OemPeriod;

            var CAlt = ConsoleModifiers.Alt;
            var CShift = ConsoleModifiers.Shift;
            var CControl = ConsoleModifiers.Control;

            var None = KeyModifiers.None;
            var Alt = KeyModifiers.Alt;
            var Shift = KeyModifiers.Shift;
            var Control = KeyModifiers.Control;

            _modifiers = new KeyModifiers[8];

            _modifiers[0] = None;
            _modifiers[(int)CAlt] = Alt;
            _modifiers[(int)CShift] = Shift;
            _modifiers[(int)CControl] = Control;
            _modifiers[(int)(CAlt | CShift)] = Alt | Shift;
            _modifiers[(int)(CAlt | CControl)] = Alt | Control;
            _modifiers[(int)(CShift | CControl)] = Shift | Control;
            _modifiers[(int)(CAlt | CShift | CControl)] = Alt | Shift | Control;
        }

        public Key ConvertKey(ConsoleKey key) => _keys[(int)key];

        public KeyModifiers ConvertModifiers(ConsoleModifiers modifiers) => _modifiers[(int)modifiers];
    }
}
