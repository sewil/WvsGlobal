#pragma once

// declare your hook functions

namespace MapleHooks
{
	void __cdecl CClientSocket__ConnectIP_Hook(char* cp, short host);
	int __fastcall ExampleFunc_thiscall2(void* pThis, void* edx, int nArg1);
}