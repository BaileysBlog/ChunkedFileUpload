using DataHandler.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataHandler
{
    class Program
    {
        const long MaxBufferSize = (1024 * 1024 * 1024)/(128*2);

        static void Main(string[] args)
        {
            //File Selection
            Console.WriteLine("Please enter path to file:");
            var path = @"C:\Users\Bailey Miller\Desktop\Fortnite Editted Videos\TeleportingKid.mp4";//Console.ReadLine();

            var FileUpload = new FileUpload(path, MaxBufferSize);
            FileUpload.UploadChunksAsync().Wait();
            Console.WriteLine($"File upload completed in {FileUpload.ExecutionTime.TotalSeconds} seconds");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        
    }
}
