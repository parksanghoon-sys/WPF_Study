// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Test test = Test.Test3;

double d = Convert.ToDouble(test);
Console.WriteLine($"Hello, World! {d.ToString()}");

Console.ReadLine();
enum Test
{
    Test1,
    Test2,
    Test3
}