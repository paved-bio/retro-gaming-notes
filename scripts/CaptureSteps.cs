using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

/// <summary>
/// Step-by-step XInput capture for diagnosing cheap DInput pads via XOutput/ViGEm.
/// Hold the asked input, press Enter; writes capture_steps.txt next to the exe.
/// </summary>
class Program
{
    [StructLayout(LayoutKind.Sequential)]
    struct G
    {
        public ushort btn;
        public byte LT, RT;
        public short LX, LY, RX, RY;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct S
    {
        public uint n;
        public G g;
    }

    [DllImport("xinput1_4.dll")]
    static extern uint XInputGetState(uint i, ref S s);

    static G ReadPad(out bool ok)
    {
        S s = new S();
        ok = XInputGetState(0, ref s) == 0;
        return s.g;
    }

    static string Fmt(G g)
    {
        return "LX=" + g.LX + " LY=" + g.LY + " RX=" + g.RX + " RY=" + g.RY +
               " LT=" + g.LT + " RT=" + g.RT + " btn=" + g.btn;
    }

    static int Score(G g)
    {
        return Math.Abs((int)g.LX) + Math.Abs((int)g.LY) + Math.Abs((int)g.RX) + Math.Abs((int)g.RY) +
               g.LT + g.RT + g.btn;
    }

    static void Main()
    {
        try
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }
        catch { }

        string dir = AppDomain.CurrentDomain.BaseDirectory;
        string outPath = Path.Combine(dir, "capture_steps.txt");
        StreamWriter log = new StreamWriter(outPath, false, new UTF8Encoding(true));
        log.AutoFlush = true;

        string[] steps = new string[] {
            "Idle — sticks centered",
            "Left stick LEFT — hold",
            "Left stick RIGHT — hold",
            "Left stick UP — hold",
            "Left stick DOWN — hold",
            "Right stick LEFT — hold",
            "Right stick RIGHT — hold",
            "Right stick UP — hold",
            "Right stick DOWN — hold",
            "D-pad UP — hold",
            "D-pad DOWN — hold",
            "D-pad LEFT — hold",
            "D-pad RIGHT — hold",
            "Face button North — hold",
            "Face button East — hold",
            "Face button South — hold",
            "Face button West — hold",
            "Left bumper (L1/LB) — hold",
            "Right bumper (R1/RB) — hold",
            "Left trigger (L2/LT) — hold",
            "Right trigger (R2/RT) — hold",
            "Select / Back — hold",
            "Start — hold",
            "Press Left stick (L3) — hold",
            "Press Right stick (R3) — hold"
        };

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("  CaptureSteps (XInput slot 0)");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("  1) Pad Mode = ANALOG (Oklick: RED LED)");
        Console.WriteLine("  2) XOutput -> Start");
        Console.WriteLine("  Each step: do it, HOLD, press Enter");
        Console.WriteLine();

        bool ok;
        G g0 = ReadPad(out ok);
        if (!ok)
        {
            Console.WriteLine("No Xbox 360 pad on slot 0. Start XOutput, then retry.");
            Console.WriteLine("Press Enter...");
            Console.ReadLine();
            log.Close();
            return;
        }
        Console.WriteLine("Controller found.");
        Console.WriteLine();
        log.WriteLine("=== START " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ===");

        for (int i = 0; i < steps.Length; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("STEP " + (i + 1) + " / " + steps.Length);
            Console.WriteLine(steps[i]);
            Console.WriteLine();
            Console.WriteLine("Hold and press Enter...");
            Console.ReadLine();

            G best = ReadPad(out ok);
            for (int k = 0; k < 10; k++)
            {
                Thread.Sleep(40);
                G cur = ReadPad(out ok);
                if (!ok) continue;
                if (Score(cur) > Score(best)) best = cur;
            }

            string line = "STEP " + (i + 1) + ": " + steps[i] + "  =>  " + Fmt(best);
            Console.WriteLine("Logged: " + Fmt(best));
            Console.WriteLine();
            log.WriteLine(line);
        }

        log.WriteLine("=== DONE ===");
        log.Close();
        Console.WriteLine("========================================");
        Console.WriteLine(" Done. See capture_steps.txt");
        Console.WriteLine(" Tip: |axis| ~16000 at full tilt => need ~200% scale in PCSX2/LRPS2");
        Console.WriteLine("========================================");
        Console.WriteLine("Press Enter...");
        Console.ReadLine();
    }
}
