#include "pch.h"
#include "api.h"
#include <cstdlib>
unsigned char name[] = "Hello World";
const int max_size = 120;
void Demo(unsigned char** ptr1,int * size) {
	*ptr1 = (unsigned char* )malloc(max_size);
	_memccpy(*ptr1, name, 0, 11);
	*size = max_size;
}