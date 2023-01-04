using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

if (args.Length == 0)
{
    Console.WriteLine("Must drag a file onto the program");
    Console.ReadLine();
    return;
}
FileStream stream = File.OpenRead(args[0]);
long len = stream.Length;
if (len < 10)
{
    Console.WriteLine("File cannot be valid safetensors: too short");
    Console.ReadLine();
    return;
}
byte[] headerBlock = new byte[8];
stream.Read(headerBlock, 0, 8);
long headerSize = BitConverter.ToInt64(headerBlock);
if (len < 8 + headerSize || headerSize <= 0 || headerSize > 100_000_000)
{
    Console.WriteLine($"File cannot be valid safetensors: header len wrong {headerSize}");
    Console.ReadLine();
    return;
}
byte[] headerBytes = new byte[headerSize];
stream.Read(headerBytes, 0, (int) headerSize);
string header = Encoding.UTF8.GetString(headerBytes);
string formatted = JToken.Parse(header).ToString(Formatting.Indented);
string data = $"Header length: {headerSize}, as string: {header.Length}, full file len {len}:\n\n----------\n{formatted}\n--------";
Console.WriteLine(data);
File.WriteAllText("out_raw.json", data);
Console.ReadLine();
