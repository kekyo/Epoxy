using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace EpoxyHello.Uno.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new EpoxyHello.Uno.App(), args);
            host.Run();
        }
    }
}
