using System;
using System.Collections.Generic;
using System.Text;

namespace ClientUploader.Models
{
    public class FileProgress
    {

        public FileUpload Parent { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }
        public bool Status { get; private set; }
        public long BytesUploaded { get; private set; }

        public FileProgress(FileUpload parent, TimeSpan executionTime, long byteCount ,bool status)
        {
            Parent = parent;
            ExecutionTime = executionTime;
            BytesUploaded = byteCount;
            Status = status;
        }
    }
}
