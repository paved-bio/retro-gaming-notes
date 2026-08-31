using System;
using System.Runtime.InteropServices;
using System.Threading;

/// <summary>
/// Live XInput stick dump (10 seconds). Use after XOutput Start.
/// If RX/RY only change when moving the RIGHT stick, camera mapping is OK.
/// </summary>
class PadTest
{
    [StructLayout(LayoutKind.Sequential)]
    struct Gamepad
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct State
    {
        public uint dwPacketNumber;
        public Gamepad Gamepad;
    }

    [DllImport("xinput1_4.dll")]
    static extern uint XInputGetState(uint dwUserIndex, ref State pState);

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("============================================");
        Console.WriteLine(" PadTest — 10 seconds");
        Console.WriteLine(" 1) Mode LED = RED (analog)");
        Console.WriteLine(" 2) XOutput -> Start");
        Console.WriteLine(" 3) Move LEFT stick, then RIGHT stick");
        Console.WriteLine("============================================");

        State s = new State();
        if (XInputGetState(0, ref s) != 0)
        {
            Console.WriteLine("No Xbox 360 on slot 0. Start XOutput first.");
            return;
        }

        short lx = 0, ly = 0, rx = 0, ry = 0;
        int t0 = Environment.TickCount;
        while (Environment.TickCount - t0 < 10000)
        {
            s = new State();
            if (XInputGetState(0, ref s) != 0)
            {
                Console.WriteLine("Pad disconnected.");
                return;
            }

            Gamepad g = s.Gamepad;
            if (Math.Abs(g.sThumbLX - lx) > 2500 || Math.Abs(g.sThumbLY - ly) > 2500 ||
                Math.Abs(g.sThumbRX - rx) > 2500 || Math.Abs(g.sThumbRY - ry) > 2500)
            {
                Console.WriteLine(
                    "LX=" + g.sThumbLX + " LY=" + g.sThumbLY +
                    " RX=" + g.sThumbRX + " RY=" + g.sThumbRY +
                    " btn=" + g.wButtons);
                lx = g.sThumbLX;
                ly = g.sThumbLY;
                rx = g.sThumbRX;
                ry = g.sThumbRY;
            }
            Thread.Sleep(40);
        }

        Console.WriteLine("DONE. Right stick should drive RX/RY only (analog mode).");
    }
}
