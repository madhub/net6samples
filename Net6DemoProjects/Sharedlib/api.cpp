#include "pch.h"
#include "api.h"
#include <cstdlib>
#include <iostream>
#include <iomanip>
using namespace std;
unsigned char name[] = "Hello World From C";
const int max_size = 120;
//void Demo(unsigned char** ptr1,int * size) {
//	*ptr1 = (unsigned char* )malloc(max_size);
//	_memccpy(*ptr1, name, 0, 11);
//	*size = max_size;
//}

int Demo(uint8_t* inbuf, uint64_t inBufSize, MemoryAllocator memoryAllocator, uint8_t** outBufPtr, int* outBufSize)
{
	uint8_t* memArea = memoryAllocator(max_size);
	_memccpy(memArea, name, 0, 18);
	*outBufPtr = memArea;
	*outBufSize = max_size;
	return 0;
}

int Decompress(const uint8_t* in, uint64_t in_len,
	uint8_t** out, uint64_t* out_len)
{
	string str(in, in + in_len);
	cout << "Received input from C#  "<<str.c_str()<<"  of size " << in_len << "\n";
	uint8_t* memArea = new uint8_t[max_size];
	_memccpy(memArea, name, 0, 18);
	//cout << "Allocation Success" << "\n";
	*out = memArea;
	//cout << "Store Success for ptr" << "\n";
	*out_len = max_size;
	//cout << "Store Success for outlen" << "\n";
	return 0;
}
int FreeMemory(const uint8_t* in)
{
	delete[] in;
	cout << "Memory is de-allocated" << "\n";
	return 0;
}