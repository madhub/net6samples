// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

IntPtr ptr = IntPtr.Zero;
int size;
Demo(out ptr,out size);
var rs = new Span<byte>(ptr.ToPointer(), size / sizeof(byte));

byte[] bytes = new byte[10];
Marshal.Copy(ptr, bytes, 0, bytes.Length);
var str = Encoding.UTF8.GetString(bytes);
Console.WriteLine(str);

[DllImport("Sharedlib", EntryPoint = "Demo", CallingConvention = CallingConvention.StdCall)]
static extern void Demo(out IntPtr ptr,out int size);

void Test()
{

}