using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler.Models
{
    public class FileChunk
    {
        public long ChunkId { get; private set; }
        public byte[] Chunk { get; private set; }

        public bool IsFinalChunk { get; private set; }

        public FileChunk(long id, bool lastChunk,byte[] chunk)
        {
            ChunkId = id;
            IsFinalChunk = lastChunk;
            Chunk = chunk;
        }

        public FileChunk(long id, bool lastChunk)
        {
            ChunkId = id;
            IsFinalChunk = lastChunk;
        }


        public void BufferBytes(Stream fs, long numberOfBytes)
        {
            Chunk = new byte[numberOfBytes];
            fs.Read(Chunk, 0, Chunk.Length);
        }

        public Task BufferBytesAsync(Stream fs, long numberOfBytes)
        {
            Chunk = new byte[numberOfBytes];
            return fs.ReadAsync(Chunk, 0, Chunk.Length);
        }

        public long GetChunkSize()
        {
            return Chunk.LongLength;
        }

        public async Task<bool> UploadChunk()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(new Random().Next(1, 251)));
            return true;
        }
    }
}
