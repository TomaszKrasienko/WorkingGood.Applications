// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using ElementsTest;

Console.WriteLine("Hello, World!");

RecordInitExample recordInitExample = new()
{
    Field = "Tmp"
};
RecordSetExample recordSetExample = new()
{
    Field = "Tmp"
};
recordInitExample.Write();
recordSetExample.Field = "TMP2";
Console.WriteLine(recordSetExample.ToString());
