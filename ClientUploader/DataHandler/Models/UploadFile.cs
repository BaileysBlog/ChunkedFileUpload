using System;
using System.Collections.Generic;
using System.Text;

namespace DataHandler.Models
{
    public class UploadFile
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public float TotalSize { get; set; }
        public bool IsComplete { get; set; }
        public string Path { get; set; }
    }
}
