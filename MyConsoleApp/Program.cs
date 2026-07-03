var date = DateTime.Now;
var christmas = new DateTime(date.Year, 12, 25);


Console.WriteLine("Hello, World!");
Console.WriteLine("The current time is " + DateTime.Now);
Console.WriteLine($"There are {(christmas - date).Days} days until the next christmas");

