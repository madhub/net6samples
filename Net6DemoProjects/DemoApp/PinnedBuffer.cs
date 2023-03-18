using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    // https://stackoverflow.com/questions/23254759/how-can-i-pin-an-array-of-byte
    /// <summary>
    ///  class that can be used to pin a byte array until is disposed.
    /// </summary>
    public class PinnedBuffer : IDisposable
    {
        public GCHandle Handle { get; }
        public byte[] Data { get; private set; }

        public IntPtr Ptr
        {
            get
            {
                return Handle.AddrOfPinnedObject();
            }
        }

        public PinnedBuffer(byte[] bytes)
        {
            Data = bytes;
            Handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Handle.Free();
                Data = null;
            }
        }
    }
}
