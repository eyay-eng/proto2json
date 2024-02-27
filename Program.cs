namespace ProtoBuf2Json;
using TransitRealtime;
using ProtoBuf;
using Google.Protobuf;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {

        //Console.WriteLine(getFile());
        string getMtaResponse = getLocalFile();
        byte [] bytes = streamFile(getMtaResponse);

        FeedMessage newMessage = ProtoDeserialize<FeedMessage>(bytes);

        writeJsonFile<FeedMessage>(newMessage);
    }

    static private byte [] streamFile(string filename)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(filename);
        return(bytes);
    }

    private static string getUserInput()
    {
        string ?usrInput = Console.ReadLine();
        if(string.IsNullOrEmpty(usrInput))
            throw new Exception ("Input Invalid");    
        return usrInput;
    }

    private static string getLocalFile()
    {
        Console.WriteLine("Enter Folder:");
        string folderLoc = getUserInput();
        Console.WriteLine("Enter File:");
        string fileLoc = getUserInput();
        string filePath = folderLoc+@"\"+fileLoc;

        return filePath;
    }

    public static T ProtoDeserialize<T>(byte[] data) where T : class
    {
        if (null == data) return null;
        try
        {
            using(MemoryStream stream = new MemoryStream(data))
            {
                //return Serializer.Deserialize<T>(stream);
                return Serializer.Deserialize<T>(stream);
            }
        }
        catch
        {
            //Log error
            throw;
        }
    }
 
    public static void writeJsonFile<T>(FeedMessage feed)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(feed.Entities, options);
        // var json = JsonSerializer.Serialize(feed.Entities);
        Console.WriteLine(json);
        File.WriteAllText(@"C:\Repo\ProtoBuf2Json\tripUpdate.json", json);
    }

}
