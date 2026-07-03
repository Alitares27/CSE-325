using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "mslearn-dotnet-files", "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");

Directory.CreateDirectory(salesTotalDir);
var salesFiles = FindFiles(storesDirectory);
GenerateSalesSummary(salesFiles, salesTotalDir);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

Console.WriteLine($"Process completed. Total sales: {salesTotal}");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    foreach (var file in salesFiles)
    {      
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        salesTotal += data?.Total ?? 0;
    }
    
    return salesTotal;
}

void GenerateSalesSummary(IEnumerable<string> salesFiles, string outputDir)
{
    double grandTotal = 0;
    StringBuilder reportBuilder = new StringBuilder();

    reportBuilder.AppendLine("Sales Summary");
    reportBuilder.AppendLine("-----------------------------------------");

    StringBuilder detailsBuilder = new StringBuilder();
    detailsBuilder.AppendLine("Details:");

    foreach (var file in salesFiles)
    {      
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        double fileTotal = data?.Total ?? 0;
        
        grandTotal += fileTotal;

        string friendlyPath = Path.Combine(Path.GetFileName(Path.GetDirectoryName(file) ?? ""), Path.GetFileName(file));
        detailsBuilder.AppendLine($"  {friendlyPath}: {fileTotal:C}");
    }

    reportBuilder.AppendLine($"  Total Sales: {grandTotal:C}");
    reportBuilder.AppendLine();
    reportBuilder.Append(detailsBuilder.ToString());

    string outputPath = Path.Combine(outputDir, "sales_summary.txt");
    File.WriteAllText(outputPath, reportBuilder.ToString());
}

record SalesData (double Total);
