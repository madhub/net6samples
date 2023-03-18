using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public static class NativeMethods
    {
        const string LibraryName = "Sharedlib";
        [DllImport(LibraryName, EntryPoint = "Decompress", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Decompress(IntPtr inBuf, ulong inBufsize,
                                     out IntPtr byteOut, out ulong size);

        [DllImport(LibraryName, EntryPoint = "FreeMemory", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FreeMemory(IntPtr intPtr);

        public static (int status, IntPtr buffer, ulong bufferSize) DecompressMethod(byte[] inBuffer, ulong inBufsize)
        {
            IntPtr outBuffer;
            ulong outBufferSize;
            // pin the buffer to avoid garbage collection
            using var buffer = new PinnedBuffer(inBuffer);
            int status = Decompress(buffer.Ptr, inBufsize, out outBuffer, out outBufferSize);
            return (status,outBuffer, outBufferSize);

        }

    }
    public class Results : IDisposable
    {
        private bool disposedValue;
        private readonly int status;
        private IntPtr buffer;
        private readonly ulong bufferSize;

        public Results(int status, IntPtr buffer, ulong bufferSize)
        {
            this.status = status;
            this.buffer = buffer;
            this.bufferSize = bufferSize;
        }
        public bool IsSuccess { get { return status == 0; } }
        public IntPtr BufferPtr 
        { 
            get { 
                return IntPtr.Zero;
                
            } 
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if ( buffer != IntPtr.Zero  )
                    {
                        NativeMethods.FreeMemory(buffer);
                        buffer = IntPtr.Zero;

                    }
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Results()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


}
