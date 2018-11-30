using System.Windows.Forms;

namespace GeneticTSP
{
    internal class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Visual());
        }
    }
}
