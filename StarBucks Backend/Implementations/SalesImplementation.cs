using StarBucks_Backend.Services;
using StarBucks_Backend.Models;

namespace StarBucks_Backend.Implementations
{
    public class SalesImplementation:ISalesService
    {
        private readonly string _salesFilePath = "D:\\project 1\\.vs\\StarBucks Project\\salesReport.csv";

        public IEnumerable<Sales> GetSalesRecord()
        {
            var sales = new List<Sales>();
            using (var reader = new StreamReader(_salesFilePath))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length != 5)
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                        continue;
                    }

                    try
                    {
                        string productName = values[0];
                        int quantity = int.Parse(values[1]);
                        decimal rate = decimal.Parse(values[2]);
                        decimal amount = decimal.Parse(values[3]);
                        DateTime date = DateTime.Parse(values[4]).Date;

                        sales.Add(new Sales
                        {
                            ProductName = productName,
                            Quantity = quantity,
                            Rate = rate,
                            Amount = amount,
                            Date=date
                        });
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Format error in line: {line}. Error: {ex.Message}");
                    }
                }
            }
            return sales;
        }
    }
}