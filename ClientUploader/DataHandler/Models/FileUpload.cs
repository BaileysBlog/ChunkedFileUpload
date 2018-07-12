using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DataHandler.Models
{
    public class FileUpload
    {
        public FileInfo FileInfo { get; private set; }
        public long MaxChunkSize { get; private set; }

        private long ChunkTotal = 0;
        private string FileId;

        public List<FileProgress> ProgressEvents = new List<FileProgress>();
        public long BytesUploaded
        {
            get
            {
                return ProgressEvents.Where(x => x.Status == true).Sum(x => x.BytesUploaded);
            }
        }

        public TimeSpan ExecutionTime
        {
            get
            {
                var tSpan = new TimeSpan();
                var times = ProgressEvents.Select(x => x.ExecutionTime);
                foreach (var time in times)
                {
                    tSpan = tSpan.Add(time);
                }

                return tSpan;
            }
        }

        public FileUpload(string path, long maxChunkSize = 1048576)
        {
            FileInfo = new FileInfo(path);
            MaxChunkSize = maxChunkSize;

            CalculateChunkValues();
        }


        private void CalculateChunkValues()
        {
            if (File.Exists(FileInfo.FullName))
            {
                //Determine number of chunks
                if (FileInfo.Length > MaxChunkSize)
                {
                    if (FileInfo.Length % MaxChunkSize == 0)
                    {
                        ChunkTotal = FileInfo.Length / MaxChunkSize;
                    }
                    else
                    {
                        ChunkTotal = (FileInfo.Length / MaxChunkSize) + 1;
                    }
                }
                else
                {
                    //Can send in one chunk
                    ChunkTotal = 1;
                }







            }
        }

        public async Task UploadChunksAsync()
        {
            await await Task.Factory.StartNew(async () =>
            {
                var startTime = DateTime.Now;
                FileChunk chunk = null;
                using (FileStream fs = FileInfo.OpenRead())
                {
                    for (long index = 0; index < ChunkTotal; index++)
                    {
                        chunk = new FileChunk(index, ChunkTotal == 1 || ChunkTotal - 1 == index);
                        if (chunk.IsFinalChunk) 
                        {
                            await chunk.BufferBytesAsync(fs, ChunkTotal == 1 ? FileInfo.Length : FileInfo.Length - BytesUploaded);
                        }
                        else
                        {
                            await chunk.BufferBytesAsync(fs, MaxChunkSize);
                        }

                        var result = await chunk.UploadChunk(FileId);

                        var executionTime = DateTime.Now.Subtract(startTime);

                        ProgressEvents.Add(new FileProgress(this, executionTime, chunk.GetChunkSize(), result));
                    }
                }
            });
        }

        public async Task<bool> GetFileId()
        {
            return null;
        }
    }
}
