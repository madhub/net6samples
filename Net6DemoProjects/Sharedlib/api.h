//#pragma once
//#define EXPORT extern "C" __declspec(dllexport)


//EXPORT void Demo(unsigned char** ptr1); 


#ifndef _Sharedlib_H_
	#define _Sharedlib_H_

#ifndef MYAPI
	#define MYAPI __declspec(dllexport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

	MYAPI void __cdecl Demo(unsigned char** ptr1, int* size);
	//MYAPI void print_line(const char* str);

#ifdef __cplusplus
}
#endif

#endif // #ifndef _Sharedlib_H_
