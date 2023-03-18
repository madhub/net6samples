//#pragma once
//#define EXPORT extern "C" __declspec(dllexport)


//EXPORT void Demo(unsigned char** ptr1); 

#include<stdint.h>

#ifndef _Sharedlib_H_
	#define _Sharedlib_H_

#ifndef MYAPI
	#define MYAPI __declspec(dllexport)
#endif

#ifdef __cplusplus
extern "C" {
#endif
	
	typedef uint8_t* (*MemoryAllocator)(uint64_t size);
	MYAPI int __cdecl Demo(uint8_t* inbuf, uint64_t inBufSize, MemoryAllocator memoryAllocator, uint8_t** outBufPtr, uint64_t* outBufSize);
	//MYAPI void print_line(const char* str);
	MYAPI int __cdecl Decompress(	const uint8_t* in, uint64_t in_len,
					uint8_t** out, uint64_t* out_len);

	MYAPI int __cdecl FreeMemory(const uint8_t* in);


#ifdef __cplusplus
}
#endif

#endif // #ifndef _Sharedlib_H_
