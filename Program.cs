

namespace TechShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TechshopUi ui = new TechshopUi();
                ui.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" error occurred: {ex.Message}");
            }
        }
    }
}



