using OpenSilver.Simulator;
using System;

namespace EpoxyHello.OpenSilver.Simulator;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) =>
        SimulatorLauncher.Start(typeof(App));
}
