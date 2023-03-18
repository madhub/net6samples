// See https://aka.ms/new-console-template for more information
using DemoApp;
using System;
using System.Buffers;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

//IntPtr ptr = IntPtr.Zero;
//int size;
//Demo(out ptr,out size);
//var rs = new Span<byte>(ptr.ToPointer(), size / sizeof(byte));

//byte[] bytes = new byte[10];
//Marshal.Copy(ptr, bytes, 0, bytes.Length);
//var str = Encoding.UTF8.GetString(bytes);
//Console.WriteLine(str);

//[DllImport("Sharedlib", EntryPoint = "Demo", CallingConvention = CallingConvention.StdCall)]
//static extern void Demo(out IntPtr ptr,out int size);

//void Test()
//{

//}

// int Demo(uint8_t* inbuf, uint64_t inBufSize, MemoryAllocator memoryAllocator, uint8_t** outBufPtr, int* outBufSize)

// Working Demo
//String someMessage = "Hello From C#";
//byte[] inputBuffer = Encoding.UTF8.GetBytes(someMessage);
//ulong inputBufferSize = (ulong)inputBuffer.Length;
//IntPtr outBuffer;
//ulong outBufferSize;

//try
//{
//    Decompress(inputBuffer, inputBufferSize, out outBuffer, out outBufferSize);
//    Memory<byte> buffer;
//    unsafe
//    {
//        Span<byte> span = new Span<byte>(outBuffer.ToPointer(), (int)outBufferSize);
//        buffer = new Memory<byte>(span);
        
//    }
//    String message1 = Encoding.UTF8.GetString(span);
//    Console.WriteLine(message1);

//    //byte[] decodedData = new byte[outBufferSize];
//    //Marshal.Copy(outBuffer, decodedData, 0, (int)outBufferSize);

//    //String message = Encoding.UTF8.GetString(decodedData, 0, (int)outBufferSize);
//    //Console.WriteLine(message);
//    //Console.WriteLine("");
//}
//catch (Exception exp)
//{

//	Console.WriteLine(exp);
//}

{
    String demoInput = "This is compressed Buffer";
    byte []compressedBuffer = Encoding.UTF8.GetBytes(demoInput);
    var decompressedData = DecompressData(compressedBuffer,(ulong) compressedBuffer.Length);
    unsafe
    {
        // Usage 1 - use it as stream
        byte* bytePtr  = (byte*)decompressedData.buffer.ToPointer();
        UnmanagedMemoryStream unmanagedMemoryStream = new UnmanagedMemoryStream(bytePtr, (long)decompressedData.bufferSize);

        // Usage 2 - Create Span object & use it as byte array
        Span<byte> span = new Span<byte>(decompressedData.buffer.ToPointer(), (int)decompressedData.bufferSize);
        String message = Encoding.UTF8.GetString(span);
        Console.WriteLine(message);
    }
    FreeMemory(decompressedData.buffer);

    Console.WriteLine("");

}

(IntPtr buffer, ulong bufferSize) DecompressData(byte[] inBuffer, ulong inputBufferSize)
{
    IntPtr outBuffer;
    ulong outBufferSize;
    // pin the buffer to avoid garbage collection
    using var buffer = new PinnedBuffer(inBuffer);
    Decompress(buffer.Ptr, inputBufferSize, out outBuffer, out outBufferSize);
    
    return(outBuffer, outBufferSize);
}





[DllImport("Sharedlib", EntryPoint = "Decompress", CallingConvention = CallingConvention.Cdecl)]
static extern int Decompress(IntPtr inBuf, ulong inBufsize,
                                    out IntPtr byteOut, out ulong size);

[DllImport("Sharedlib", EntryPoint = "FreeMemory", CallingConvention = CallingConvention.Cdecl)]
static extern int FreeMemory(IntPtr intPtr);

//IntPtr ptr = IntPtr.Zero;
//ulong outBufSize;


//static unsafe IntPtr AllocMem(ulong size)
//{
//    var bytes = new byte[size];
//    fixed (byte* pSource = bytes)
//    {
//        IntPtr intPtr = pSource;
//    }

//    return ;
//}

//[DllImport("Sharedlib", EntryPoint = "Demo", CallingConvention = CallingConvention.Cdecl)]
//static extern void Demo(    in IntPtr inBuf, in ulong inBufsize, 
//                            [MarshalAs(UnmanagedType.FunctionPtr)] MemoryAllocator memAllocator,
//                            out IntPtr ptr,out ulong size);
//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
//delegate IntPtr MemoryAllocator(ulong size);