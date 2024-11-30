#include "pch.h"

// BE AWARE ===v
// in order to reference other projects you need to add:
// $(SolutionDir)Common;%(AdditionalIncludeDirectories)
// to project properties -> c/c++ -> additional include directories
#include "ExampleHooks.h"
#include <ZXString.h>
#include <Common.h>
#include <hooker.h>
#include <memedit.h>

// BE AWARE ===v
// in order to fix the detours.lib link error you need to replace
// project properties -> vc++ directories -> library directories 
// with $(SolutionDir)Common;$(LibraryPath)

/// ================ \\\

// executed after the client is unpacked
VOID MainFunc()
{
	Log(__FUNCTION__);

	#pragma region Buncha nops
	PatchNop(0x00465E15, 1);
	PatchNop(0x0046618B, 1);
	PatchNop(0x00466224, 1);
	PatchNop(0x004684D7, 1);
	PatchNop(0x00468544, 1);
	PatchNop(0x0046B85A, 1);
	PatchNop(0x0047C5C1, 1);
	PatchNop(0x0047C5C2, 1);
	PatchNop(0x004828BD, 1);
	PatchNop(0x004ED9EF, 1);

    /*
    AES REMOVED
    PatchNop(0x004FDD41, 42);
    */

	PatchNop(0x004FDDB2, 1);
	PatchNop(0x004FDDC0, 1);
	PatchNop(0x004FDE00, 1);
	PatchNop(0x004FDE0E, 1);
	PatchNop(0x004FDE2B, 1);
	PatchNop(0x004FDE39, 1);
	PatchNop(0x004FDE5D, 1);
	PatchNop(0x004FDE6B, 1);
	PatchNop(0x004FDE9C, 1);
	PatchNop(0x004FDEAA, 1);
	PatchNop(0x004FE00E, 1);
	PatchNop(0x004FE01C, 1);
	PatchNop(0x004FE069, 1);
	PatchNop(0x004FE077, 1);
	PatchNop(0x004FE0A1, 1);
	PatchNop(0x004FE0AF, 1);
	PatchNop(0x004FE0DB, 1);
	PatchNop(0x004FE0E9, 1);
	PatchNop(0x004FE122, 1);
	PatchNop(0x004FE130, 1);
	
	/*
	AES REMOVED
	PatchNop(0x004FE20E, 27);
	*/

	PatchNop(0x004FE279, 36);
	PatchNop(0x0051306F, 1);
	PatchNop(0x00513138, 1);
	PatchNop(0x00534265, 1);
	PatchNop(0x0053427F, 1);
	PatchNop(0x00534328, 1);
	PatchNop(0x0053433A, 1);
	PatchNop(0x00534609, 1);
	PatchNop(0x00534620, 1);
	PatchNop(0x00534637, 1);
	PatchNop(0x0053464E, 1);
	PatchNop(0x00534665, 1);
	PatchNop(0x0053467C, 1);
	PatchNop(0x00534693, 1);
	PatchNop(0x005346AA, 1);
	PatchNop(0x005346C1, 1);
	PatchNop(0x005346D8, 1);
	PatchNop(0x005346EF, 1);
	PatchNop(0x00534706, 1);
	PatchNop(0x0053471D, 1);
	PatchNop(0x00534734, 1);
	PatchNop(0x0053474B, 1);
	PatchNop(0x00534763, 1);
	PatchNop(0x00534910, 1);
	PatchNop(0x0053581E, 1);
	PatchNop(0x0056DBAF, 1);
	PatchNop(0x0056E2D1, 1);
	PatchNop(0x005D7ED1, 1);
	PatchNop(0x005DBF76, 1);
	PatchNop(0x005DBF83, 1);
	PatchNop(0x00609C50, 5);
	PatchNop(0x00609EEE, 5);
	PatchNop(0x0062A81D, 5);
	PatchNop(0x0062ACB9, 1);
	PatchNop(0x0062ACFB, 1);
	PatchNop(0x0062BC19, 1);
	PatchNop(0x0062BC58, 1);
	PatchNop(0x0062BCC0, 1);
	PatchNop(0x0062BD0F, 1);
	PatchNop(0x0062BD60, 1);
	PatchNop(0x0062BD89, 1);
	PatchNop(0x0062D504, 1);
	PatchNop(0x0062D50D, 1);
	PatchNop(0x0062D5BA, 1);
	PatchNop(0x0062D5C6, 1);
	PatchNop(0x0062D61D, 1);
	PatchNop(0x0062D724, 1);
	PatchNop(0x0062D8E1, 1);
	PatchNop(0x0062D9F9, 1);
	PatchNop(0x0062DBC7, 1);
	PatchNop(0x0062DC0A, 1);
	PatchNop(0x0062DCD6, 1);
	PatchNop(0x0062DCF7, 1);
	PatchNop(0x0062DFAC, 1);
	PatchNop(0x0062E201, 1);
	PatchNop(0x0062E28E, 1);
	PatchNop(0x0062E2A2, 1);
	PatchNop(0x0062EAB0, 1);
	PatchNop(0x0062EBCC, 1);
	PatchNop(0x0062EBF3, 1);
	PatchNop(0x0062EC16, 1);
	PatchNop(0x0062EC35, 1);
	PatchNop(0x0062EC5A, 1);
	PatchNop(0x0062ECB4, 1);
	PatchNop(0x0062ECFC, 1);
	PatchNop(0x0062EFDD, 1);
	PatchNop(0x0062EFE6, 1);
	PatchNop(0x0062F189, 1);
	PatchNop(0x0062F291, 1);
	PatchNop(0x0062F394, 1);
	PatchNop(0x0062F4B9, 1);
	PatchNop(0x0062F8B3, 1);
	PatchNop(0x0062F8E6, 1);
	PatchNop(0x0062FA00, 1);
	PatchNop(0x0062FA2D, 1);
	PatchNop(0x0062FA4B, 1);
	PatchNop(0x0062FB9F, 1);
	PatchNop(0x0062FBB2, 1);
	PatchNop(0x0062FC4E, 1);
	PatchNop(0x0062FCAF, 1);
	PatchNop(0x0062FCC2, 1);
	PatchNop(0x0062FD4B, 1);
	PatchNop(0x0062FD86, 1);
	PatchNop(0x006301A1, 1);
	PatchNop(0x0063021F, 1);
	PatchNop(0x0063026F, 1);
	PatchNop(0x006302DE, 1);
	PatchNop(0x00630440, 1);
	PatchNop(0x00630449, 1);
	PatchNop(0x006305BD, 1);
	PatchNop(0x006305DC, 1);
	PatchNop(0x00630761, 1);
	PatchNop(0x0063092F, 1);
	PatchNop(0x00630A35, 1);
	PatchNop(0x00630E57, 1);
	PatchNop(0x00631101, 1);
	PatchNop(0x00631138, 1);
	PatchNop(0x00631143, 1);
	PatchNop(0x0063125B, 1);
	PatchNop(0x00631483, 1);
	PatchNop(0x006314D4, 1);
	PatchNop(0x00631624, 1);
	PatchNop(0x00631750, 1);
	PatchNop(0x00631893, 1);
	PatchNop(0x0063195D, 1);
	PatchNop(0x00631A05, 1);
	PatchNop(0x00631B80, 1);
	PatchNop(0x0063224E, 1);
	PatchNop(0x0063225E, 1);
	PatchNop(0x00632300, 1);
	PatchNop(0x006323BF, 1);
	PatchNop(0x00632400, 1);
	PatchNop(0x00632446, 1);
	PatchNop(0x00632488, 1);
	PatchNop(0x006324AF, 1);
	PatchNop(0x006324C0, 1);
	PatchNop(0x006324D8, 1);
	PatchNop(0x00632578, 1);
	PatchNop(0x006325E9, 1);
	PatchNop(0x006325F8, 1);
	PatchNop(0x00632609, 1);
	PatchNop(0x006326BB, 1);
	PatchNop(0x0063272A, 1);
	PatchNop(0x00632731, 1);
	PatchNop(0x006327B2, 1);
	PatchNop(0x00632829, 1);
	PatchNop(0x0063289F, 1);
	PatchNop(0x00632A50, 1);
	PatchNop(0x00632A76, 1);
	PatchNop(0x00632AB6, 1);
	PatchNop(0x00632B05, 1);
	PatchNop(0x00632B19, 1);
	PatchNop(0x00632B45, 1);
	PatchNop(0x00632B70, 1);
	PatchNop(0x00632B93, 1);
	PatchNop(0x00632BBE, 1);
	PatchNop(0x00632C84, 1);
	PatchNop(0x00632DC3, 1);
	PatchNop(0x00632DE9, 1);
	PatchNop(0x00632DFD, 1);
	PatchNop(0x00632E2E, 1);
	PatchNop(0x00632E54, 1);
	PatchNop(0x00632E6F, 1);
	PatchNop(0x00632EA3, 1);
	PatchNop(0x00633605, 1);
	PatchNop(0x006336F7, 1);
	PatchNop(0x006337BA, 1);
	PatchNop(0x00633954, 1);
	PatchNop(0x0063396D, 1);
	PatchNop(0x0063399A, 1);
	PatchNop(0x006339D6, 1);
	PatchNop(0x006339EF, 1);
	PatchNop(0x00633A17, 1);
	PatchNop(0x00633A8A, 1);
	PatchNop(0x00633AAC, 1);
	PatchNop(0x00633AF0, 1);
	PatchNop(0x00633B02, 1);
	PatchNop(0x00633B97, 1);
	PatchNop(0x00633BC4, 1);
	PatchNop(0x00633E8B, 1);
	PatchNop(0x00633EFB, 1);
	PatchNop(0x006340B3, 1);
	PatchNop(0x0063412C, 1);
	PatchNop(0x00634137, 1);
	PatchNop(0x0063691D, 1);
	PatchNop(0x00636924, 1);
	PatchNop(0x00636D7D, 1);
	PatchNop(0x00636DA8, 1);
	PatchNop(0x00636DCB, 1);
	PatchNop(0x00638BA4, 1);
	PatchNop(0x00638BB4, 1);
	PatchNop(0x0063B4A8, 1);
	PatchNop(0x0063B4BD, 1);
	PatchNop(0x0063B548, 1);
	PatchNop(0x0063CB30, 1);
	PatchNop(0x0063D774, 1);
	PatchNop(0x0063D790, 1);
	PatchNop(0x0063D7A8, 1);
	PatchNop(0x0064037B, 1);
	PatchNop(0x00640381, 1);
	PatchNop(0x00640387, 1);
	PatchNop(0x0064038D, 1);
	PatchNop(0x00640393, 1);

	// Poly
	/*PatchNop(0x00712563, 1);
	PatchNop(0x00712564, 1);
	PatchNop(0x00712565, 1);
	PatchNop(0x00712566, 1);
	PatchNop(0x00712567, 1);
	PatchNop(0x00712571, 1);
	PatchNop(0x00712572, 1);
	PatchNop(0x00712573, 1);
	PatchNop(0x00712574, 1);
	PatchNop(0x00712575, 1);
	PatchNop(0x00712576, 1);
	PatchNop(0x00712577, 1);
	PatchNop(0x0071257C, 1);
	PatchNop(0x0071257D, 1);
	PatchNop(0x0071257E, 1);
	PatchNop(0x0071257F, 1);
	PatchNop(0x00712584, 1);
	PatchNop(0x00712585, 1);
	PatchNop(0x00712586, 1);
	PatchNop(0x00712587, 1);
	PatchNop(0x00712588, 1);
	PatchNop(0x007125CF, 1);
	PatchNop(0x007125D0, 1);
	PatchNop(0x007125D1, 1);
	PatchNop(0x007125D2, 1);
	PatchNop(0x007125D7, 1);
	PatchNop(0x007125D8, 1);
	PatchNop(0x007125D9, 1);
	PatchNop(0x007125DA, 1);
	PatchNop(0x007125DE, 1);
	PatchNop(0x007125DF, 1);
	PatchNop(0x007125E0, 1);
	PatchNop(0x007125E1, 1);
	PatchNop(0x007125E2, 1);
	PatchNop(0x007125EB, 1);
	PatchNop(0x007125EC, 1);
	PatchNop(0x007125ED, 1);
	PatchNop(0x007125EE, 1);
	PatchNop(0x007125EF, 1);
	PatchNop(0x007125F4, 1);
	PatchNop(0x007125F5, 1);
	PatchNop(0x007125F6, 1);
	PatchNop(0x007125F7, 1);
	PatchNop(0x007125F8, 1);
	PatchNop(0x007125FD, 1);
	PatchNop(0x007125FE, 1);
	PatchNop(0x007125FF, 1);
	PatchNop(0x00712603, 1);
	PatchNop(0x00712604, 1);
	PatchNop(0x00712605, 1);
	PatchNop(0x00712606, 1);
	PatchNop(0x0071260E, 1);
	PatchNop(0x0071260F, 1);
	PatchNop(0x00712610, 1);
	PatchNop(0x00712611, 1);
	PatchNop(0x0071269A, 1);
	PatchNop(0x0071269B, 1);
	PatchNop(0x0071269C, 1);
	PatchNop(0x0071269D, 1);
	PatchNop(0x007126F5, 1);
	PatchNop(0x007126F6, 1);
	PatchNop(0x007126F7, 1);
	PatchNop(0x007126F8, 1);
	PatchNop(0x007126FB, 1);
	PatchNop(0x007126FC, 1);
	PatchNop(0x007126FD, 1);
	PatchNop(0x007126FE, 1);
	PatchNop(0x007126FF, 1);
	PatchNop(0x00712706, 1);
	PatchNop(0x00712707, 1);
	PatchNop(0x00712708, 1);
	PatchNop(0x00712709, 1);
	PatchNop(0x0071271A, 1);
	PatchNop(0x0071271B, 1);
	PatchNop(0x0071271C, 1);
	PatchNop(0x0071271D, 1);
	PatchNop(0x0071271E, 1);
	PatchNop(0x00712725, 1);
	PatchNop(0x00712726, 1);
	PatchNop(0x00712727, 1);
	PatchNop(0x00712728, 1);
	PatchNop(0x00712729, 1);
	PatchNop(0x0071273E, 1);
	PatchNop(0x0071273F, 1);
	PatchNop(0x00712740, 1);
	PatchNop(0x00712741, 1);
	PatchNop(0x0071274D, 1);
	PatchNop(0x0071274E, 1);
	PatchNop(0x0071274F, 1);
	PatchNop(0x00712750, 1);
	PatchNop(0x00712772, 1);
	PatchNop(0x00712773, 1);
	PatchNop(0x00712774, 1);
	PatchNop(0x00712775, 1);
	PatchNop(0x00712776, 1);
	PatchNop(0x0071277D, 1);
	PatchNop(0x0071277E, 1);
	PatchNop(0x0071277F, 1);
	PatchNop(0x00712780, 1);
	PatchNop(0x00712788, 1);
	PatchNop(0x00712789, 1);
	PatchNop(0x0071278A, 1);
	PatchNop(0x0071278B, 1);
	PatchNop(0x00712793, 1);
	PatchNop(0x00712794, 1);
	PatchNop(0x00712795, 1);
	PatchNop(0x00712796, 1);
	PatchNop(0x00712797, 1);
	PatchNop(0x007127A7, 1);
	PatchNop(0x007127A8, 1);
	PatchNop(0x007127A9, 1);
	PatchNop(0x007127AA, 1);
	PatchNop(0x007127E9, 1);
	PatchNop(0x007127EA, 1);
	PatchNop(0x007127EB, 1);
	PatchNop(0x007127EC, 1);
	PatchNop(0x007127EE, 1);
	PatchNop(0x007127EF, 1);
	PatchNop(0x007127F0, 1);
	PatchNop(0x007127F1, 1);
	PatchNop(0x00712824, 1);
	PatchNop(0x00712825, 1);
	PatchNop(0x00712826, 1);
	PatchNop(0x00712827, 1);
	PatchNop(0x0071282A, 1);
	PatchNop(0x0071282B, 1);
	PatchNop(0x0071282C, 1);
	PatchNop(0x0071282D, 1);
	PatchNop(0x00712846, 1);
	PatchNop(0x00712847, 1);
	PatchNop(0x00712848, 1);
	PatchNop(0x00712849, 1);
	PatchNop(0x00712881, 1);
	PatchNop(0x00712882, 1);
	PatchNop(0x00712883, 1);
	PatchNop(0x00712884, 1);
	PatchNop(0x00712885, 1);
	PatchNop(0x007128A4, 1);
	PatchNop(0x007128A5, 1);
	PatchNop(0x007128A6, 1);
	PatchNop(0x007128AC, 1);
	PatchNop(0x007128AD, 1);
	PatchNop(0x007128AE, 1);
	PatchNop(0x007128B2, 1);
	PatchNop(0x007128B3, 1);
	PatchNop(0x007128B4, 1);
	PatchNop(0x007128B5, 1);
	PatchNop(0x007128B6, 1);
	PatchNop(0x007128BA, 1);
	PatchNop(0x007128BB, 1);
	PatchNop(0x007128BC, 1);
	PatchNop(0x007128BD, 1);
	PatchNop(0x007128CA, 1);
	PatchNop(0x007128CB, 1);
	PatchNop(0x007128CC, 1);
	PatchNop(0x007128CD, 1);
	PatchNop(0x007128D2, 1);
	PatchNop(0x007128D3, 1);
	PatchNop(0x007128D4, 1);
	PatchNop(0x007128D5, 1);
	PatchNop(0x007128D6, 1);
	PatchNop(0x007128DD, 1);
	PatchNop(0x007128DE, 1);
	PatchNop(0x007128DF, 1);
	PatchNop(0x007128E0, 1);
	PatchNop(0x007128E6, 1);
	PatchNop(0x007128E7, 1);
	PatchNop(0x007128E8, 1);
	PatchNop(0x007128E9, 1);
	PatchNop(0x007128F5, 1);
	PatchNop(0x007128F6, 1);
	PatchNop(0x007128F7, 1);
	PatchNop(0x007128F8, 1);
	PatchNop(0x00712929, 1);
	PatchNop(0x0071292A, 1);
	PatchNop(0x0071292B, 1);
	PatchNop(0x0071292C, 1);
	PatchNop(0x00712934, 1);
	PatchNop(0x00712935, 1);
	PatchNop(0x00712936, 1);
	PatchNop(0x00712937, 1);
	PatchNop(0x00712944, 1);
	PatchNop(0x00712945, 1);
	PatchNop(0x00712946, 1);
	PatchNop(0x00712947, 1);
	PatchNop(0x00712973, 1);
	PatchNop(0x00712974, 1);
	PatchNop(0x00712975, 1);
	PatchNop(0x00712976, 1);
	PatchNop(0x0071297F, 1);
	PatchNop(0x00712980, 1);
	PatchNop(0x00712981, 1);
	PatchNop(0x00712982, 1);
	PatchNop(0x007129A1, 1);
	PatchNop(0x007129A2, 1);
	PatchNop(0x007129A3, 1);
	PatchNop(0x007129CD, 1);
	PatchNop(0x007129CE, 1);
	PatchNop(0x007129CF, 1);
	PatchNop(0x007129D0, 1);
	PatchNop(0x007129D1, 1);
	PatchNop(0x007129E0, 1);
	PatchNop(0x007129E1, 1);
	PatchNop(0x007129E2, 1);
	PatchNop(0x007129E3, 1);
	PatchNop(0x007129E7, 1);
	PatchNop(0x007129E8, 1);
	PatchNop(0x007129E9, 1);
	PatchNop(0x007129EA, 1);
	PatchNop(0x007129EB, 1);
	PatchNop(0x007129EC, 1);
	PatchNop(0x007129ED, 1);
	PatchNop(0x00712A07, 1);
	PatchNop(0x00712A08, 1);
	PatchNop(0x00712A09, 1);
	PatchNop(0x00712A0A, 1);
	PatchNop(0x00712A0B, 1);
	PatchNop(0x00712A1B, 1);
	PatchNop(0x00712A1C, 1);
	PatchNop(0x00712A1D, 1);
	PatchNop(0x00712A1E, 1);
	PatchNop(0x00712A1F, 1);
	PatchNop(0x00712A55, 1);
	PatchNop(0x00712A56, 1);
	PatchNop(0x00712A57, 1);
	PatchNop(0x00712A58, 1);
	PatchNop(0x00712A59, 1);
	PatchNop(0x00712A6F, 1);
	PatchNop(0x00712A70, 1);
	PatchNop(0x00712A71, 1);
	PatchNop(0x00712A7B, 1);
	PatchNop(0x00712A7C, 1);
	PatchNop(0x00712A7D, 1);
	PatchNop(0x00712A7E, 1);
	PatchNop(0x00712A7F, 1);
	PatchNop(0x00712A92, 1);
	PatchNop(0x00712A93, 1);
	PatchNop(0x00712A94, 1);
	PatchNop(0x00712A95, 1);
	PatchNop(0x00712A96, 1);
	PatchNop(0x00712A9D, 1);
	PatchNop(0x00712A9E, 1);
	PatchNop(0x00712A9F, 1);
	PatchNop(0x00712AA0, 1);
	PatchNop(0x00712AAF, 1);
	PatchNop(0x00712AB0, 1);
	PatchNop(0x00712AB1, 1);
	PatchNop(0x00712AB2, 1);
	PatchNop(0x00712AB3, 1);
	PatchNop(0x00712AF8, 1);
	PatchNop(0x00712AF9, 1);
	PatchNop(0x00712AFA, 1);
	PatchNop(0x00712AFB, 1);
	PatchNop(0x00712AFC, 1);
	PatchNop(0x00712B63, 1);
	PatchNop(0x00712B64, 1);
	PatchNop(0x00712B65, 1);
	PatchNop(0x00712B73, 1);
	PatchNop(0x00712B74, 1);
	PatchNop(0x00712B75, 1);
	PatchNop(0x00712B76, 1);
	PatchNop(0x00712BAF, 1);
	PatchNop(0x00712BB0, 1);
	PatchNop(0x00712BB1, 1);
	PatchNop(0x00712BB2, 1);
	PatchNop(0x00712BCE, 1);
	PatchNop(0x00712BCF, 1);
	PatchNop(0x00712BD0, 1);
	PatchNop(0x00712BD1, 1);
	PatchNop(0x00712C27, 1);
	PatchNop(0x00712C28, 1);
	PatchNop(0x00712C29, 1);
	PatchNop(0x00712C2A, 1);
	PatchNop(0x00712C30, 1);
	PatchNop(0x00712C31, 1);
	PatchNop(0x00712C32, 1);
	PatchNop(0x00712C33, 1);
	PatchNop(0x00712C3D, 1);
	PatchNop(0x00712C3E, 1);
	PatchNop(0x00712C3F, 1);
	PatchNop(0x00712C7D, 1);
	PatchNop(0x00712C7E, 1);
	PatchNop(0x00712C7F, 1);
	PatchNop(0x00712C80, 1);
	PatchNop(0x00712C8C, 1);
	PatchNop(0x00712C8D, 1);
	PatchNop(0x00712C8E, 1);
	PatchNop(0x00712C8F, 1);
	PatchNop(0x00712C98, 1);
	PatchNop(0x00712C99, 1);
	PatchNop(0x00712C9A, 1);
	PatchNop(0x00712C9B, 1);
	PatchNop(0x00712CA3, 1);
	PatchNop(0x00712CA4, 1);
	PatchNop(0x00712CA5, 1);
	PatchNop(0x00712CA6, 1);
	PatchNop(0x00712CB5, 1);
	PatchNop(0x00712CB6, 1);
	PatchNop(0x00712CB7, 1);
	PatchNop(0x00712CB8, 1);
	PatchNop(0x00712CBE, 1);
	PatchNop(0x00712CBF, 1);
	PatchNop(0x00712CC0, 1);
	PatchNop(0x00712CC1, 1);
	PatchNop(0x00712CC2, 1);
	PatchNop(0x00712CC8, 1);
	PatchNop(0x00712CC9, 1);
	PatchNop(0x00712CCA, 1);
	PatchNop(0x00712CCB, 1);
	PatchNop(0x00712CCC, 1);
	PatchNop(0x00712CD6, 1);
	PatchNop(0x00712CD7, 1);
	PatchNop(0x00712CD8, 1);
	PatchNop(0x00712CD9, 1);
	PatchNop(0x00712CDA, 1);
	PatchNop(0x00712D34, 1);
	PatchNop(0x00712D35, 1);
	PatchNop(0x00712D36, 1);
	PatchNop(0x00712D37, 1);
	PatchNop(0x00712DA6, 1);
	PatchNop(0x00712DA7, 1);
	PatchNop(0x00712DA8, 1);
	PatchNop(0x00712DA9, 1);
	PatchNop(0x00712DE5, 1);
	PatchNop(0x00712DE6, 1);
	PatchNop(0x00712DE7, 1);
	PatchNop(0x00712E50, 1);
	PatchNop(0x00712E51, 1);
	PatchNop(0x00712E52, 1);
	PatchNop(0x00712E53, 1);
	PatchNop(0x00712E59, 1);
	PatchNop(0x00712E5A, 1);
	PatchNop(0x00712E5B, 1);
	PatchNop(0x00712E73, 1);
	PatchNop(0x00712E74, 1);
	PatchNop(0x00712E75, 1);
	PatchNop(0x00712E76, 1);
	PatchNop(0x00712E77, 1);
	PatchNop(0x00712E7A, 1);
	PatchNop(0x00712E7B, 1);
	PatchNop(0x00712E7C, 1);
	PatchNop(0x00712E7D, 1);
	PatchNop(0x00712EF5, 1);
	PatchNop(0x00712EF6, 1);
	PatchNop(0x00712EF7, 1);
	PatchNop(0x00712EF8, 1);
	PatchNop(0x00712EF9, 1);
	PatchNop(0x00712FA0, 1);
	PatchNop(0x00712FA1, 1);
	PatchNop(0x00712FA2, 1);
	PatchNop(0x00712FA3, 1);
	PatchNop(0x00712FA7, 1);
	PatchNop(0x00712FA8, 1);
	PatchNop(0x00712FA9, 1);
	PatchNop(0x00712FAA, 1);
	PatchNop(0x007130C9, 1);
	PatchNop(0x007130CA, 1);
	PatchNop(0x007130CB, 1);
	PatchNop(0x007130CC, 1);
	PatchNop(0x007130CF, 1);
	PatchNop(0x007130D0, 1);
	PatchNop(0x007130D1, 1);
	PatchNop(0x007130D2, 1);
	PatchNop(0x00713117, 1);
	PatchNop(0x00713118, 1);
	PatchNop(0x00713119, 1);
	PatchNop(0x00713144, 1);
	PatchNop(0x00713145, 1);
	PatchNop(0x00713146, 1);
	PatchNop(0x00713147, 1);
	PatchNop(0x00713148, 1);
	PatchNop(0x00713161, 1);
	PatchNop(0x00713162, 1);
	PatchNop(0x00713163, 1);
	PatchNop(0x00713164, 1);
	PatchNop(0x007131E5, 1);
	PatchNop(0x007131E6, 1);
	PatchNop(0x007131E7, 1);
	PatchNop(0x007131E8, 1);
	PatchNop(0x007131E9, 1);
	PatchNop(0x00713217, 1);
	PatchNop(0x00713218, 1);
	PatchNop(0x00713219, 1);
	PatchNop(0x0071321A, 1);
	PatchNop(0x00713264, 1);
	PatchNop(0x00713265, 1);
	PatchNop(0x00713266, 1);
	PatchNop(0x0071327E, 1);
	PatchNop(0x0071327F, 1);
	PatchNop(0x00713280, 1);
	PatchNop(0x00713281, 1);
	PatchNop(0x007132D6, 1);
	PatchNop(0x007132D7, 1);
	PatchNop(0x007132D8, 1);
	PatchNop(0x007132D9, 1);
	PatchNop(0x007132DA, 1);
	PatchNop(0x0071330A, 1);
	PatchNop(0x0071330B, 1);
	PatchNop(0x0071330C, 1);
	PatchNop(0x0071330D, 1);
	PatchNop(0x0071330E, 1);
	PatchNop(0x0071331B, 1);
	PatchNop(0x0071331C, 1);
	PatchNop(0x0071331D, 1);
	PatchNop(0x0071331E, 1);
	PatchNop(0x00713355, 1);
	PatchNop(0x00713356, 1);
	PatchNop(0x00713357, 1);
	PatchNop(0x00713358, 1);
	PatchNop(0x00713359, 1);
	PatchNop(0x00713369, 1);
	PatchNop(0x0071336A, 1);
	PatchNop(0x0071336B, 1);
	PatchNop(0x0071336C, 1);
	PatchNop(0x0071336D, 1);
	PatchNop(0x0071336E, 1);
	PatchNop(0x0071336F, 1);
	PatchNop(0x00713370, 1);
	PatchNop(0x00713371, 1);
	PatchNop(0x00713372, 1);
	PatchNop(0x0071337D, 1);
	PatchNop(0x0071337E, 1);
	PatchNop(0x0071337F, 1);
	PatchNop(0x00713385, 1);
	PatchNop(0x00713386, 1);
	PatchNop(0x00713387, 1);
	PatchNop(0x00713388, 1);
	PatchNop(0x00713389, 1);
	PatchNop(0x00713397, 1);
	PatchNop(0x00713398, 1);
	PatchNop(0x00713399, 1);
	PatchNop(0x0071339A, 1);
	PatchNop(0x00713411, 1);
	PatchNop(0x00713412, 1);
	PatchNop(0x00713413, 1);
	PatchNop(0x00713414, 1);
	PatchNop(0x00713415, 1);
	PatchNop(0x00713416, 1);
	PatchNop(0x00713417, 1);
	PatchNop(0x00713418, 1);
	PatchNop(0x00713437, 1);
	PatchNop(0x00713438, 1);
	PatchNop(0x00713439, 1);
	PatchNop(0x0071343A, 1);
	PatchNop(0x0071347A, 1);
	PatchNop(0x0071347B, 1);
	PatchNop(0x0071347C, 1);
	PatchNop(0x0071347D, 1);
	PatchNop(0x007134EA, 1);
	PatchNop(0x007134EB, 1);
	PatchNop(0x007134EC, 1);
	PatchNop(0x007134F6, 1);
	PatchNop(0x007134F7, 1);
	PatchNop(0x007134F8, 1);
	PatchNop(0x007134F9, 1);
	PatchNop(0x007134FA, 1);
	PatchNop(0x00713501, 1);
	PatchNop(0x00713502, 1);
	PatchNop(0x00713503, 1);
	PatchNop(0x0071352F, 1);
	PatchNop(0x00713530, 1);
	PatchNop(0x00713531, 1);
	PatchNop(0x00713538, 1);
	PatchNop(0x00713539, 1);
	PatchNop(0x0071353A, 1);
	PatchNop(0x0071353B, 1);
	PatchNop(0x00713549, 1);
	PatchNop(0x0071354A, 1);
	PatchNop(0x0071354B, 1);
	PatchNop(0x0071354C, 1);
	PatchNop(0x0071354D, 1);
	PatchNop(0x0071354E, 1);
	PatchNop(0x00713558, 1);
	PatchNop(0x00713559, 1);
	PatchNop(0x0071355A, 1);
	PatchNop(0x0071355B, 1);
	PatchNop(0x00713566, 1);
	PatchNop(0x00713567, 1);
	PatchNop(0x00713568, 1);
	PatchNop(0x0071356B, 1);
	PatchNop(0x0071356C, 1);
	PatchNop(0x0071356D, 1);
	PatchNop(0x00713571, 1);
	PatchNop(0x00713572, 1);
	PatchNop(0x00713573, 1);
	PatchNop(0x00713574, 1);
	PatchNop(0x00713594, 1);
	PatchNop(0x00713595, 1);
	PatchNop(0x00713596, 1);
	PatchNop(0x00713597, 1);
	PatchNop(0x007135B2, 1);
	PatchNop(0x007135B3, 1);
	PatchNop(0x007135B4, 1);
	PatchNop(0x007135B5, 1);
	PatchNop(0x007135B6, 1);
	PatchNop(0x007135D6, 1);
	PatchNop(0x007135D7, 1);
	PatchNop(0x007135D8, 1);
	PatchNop(0x007135D9, 1);
	PatchNop(0x00713618, 1);
	PatchNop(0x00713619, 1);
	PatchNop(0x0071361A, 1);
	PatchNop(0x0071361B, 1);
	PatchNop(0x0071362E, 1);
	PatchNop(0x0071362F, 1);
	PatchNop(0x00713630, 1);
	PatchNop(0x00713639, 1);
	PatchNop(0x0071363A, 1);
	PatchNop(0x0071363B, 1);
	PatchNop(0x0071363C, 1);
	PatchNop(0x00713644, 1);
	PatchNop(0x00713645, 1);
	PatchNop(0x00713646, 1);
	PatchNop(0x00713647, 1);
	PatchNop(0x0071366C, 1);
	PatchNop(0x0071366D, 1);
	PatchNop(0x0071366E, 1);
	PatchNop(0x0071366F, 1);
	PatchNop(0x00713674, 1);
	PatchNop(0x00713675, 1);
	PatchNop(0x00713676, 1);
	PatchNop(0x0071367D, 1);
	PatchNop(0x0071367E, 1);
	PatchNop(0x0071367F, 1);
	PatchNop(0x00713680, 1);
	PatchNop(0x00713683, 1);
	PatchNop(0x00713684, 1);
	PatchNop(0x00713685, 1);
	PatchNop(0x0071368E, 1);
	PatchNop(0x0071368F, 1);
	PatchNop(0x00713690, 1);
	PatchNop(0x00713691, 1);
	PatchNop(0x00713692, 1);
	PatchNop(0x00713695, 1);
	PatchNop(0x00713696, 1);
	PatchNop(0x00713697, 1);
	PatchNop(0x007136C7, 1);
	PatchNop(0x007136C8, 1);
	PatchNop(0x007136C9, 1);
	PatchNop(0x007136CA, 1);
	PatchNop(0x00713702, 1);
	PatchNop(0x00713703, 1);
	PatchNop(0x00713704, 1);
	PatchNop(0x00713705, 1);
	PatchNop(0x0071370F, 1);
	PatchNop(0x00713710, 1);
	PatchNop(0x00713711, 1);
	PatchNop(0x00713712, 1);
	PatchNop(0x00713713, 1);
	PatchNop(0x0071393E, 1);
	PatchNop(0x0071393F, 1);
	PatchNop(0x00713940, 1);
	PatchNop(0x00713941, 1);
	PatchNop(0x00713946, 1);
	PatchNop(0x00713947, 1);
	PatchNop(0x00713948, 1);
	PatchNop(0x00713949, 1);
	PatchNop(0x0071394E, 1);
	PatchNop(0x0071394F, 1);
	PatchNop(0x00713950, 1);
	PatchNop(0x00713951, 1);
	PatchNop(0x00713952, 1);
	PatchNop(0x00713955, 1);
	PatchNop(0x00713956, 1);
	PatchNop(0x00713957, 1);
	PatchNop(0x00713958, 1);
	PatchNop(0x00713959, 1);
	PatchNop(0x0071396E, 1);
	PatchNop(0x0071396F, 1);
	PatchNop(0x00713970, 1);
	PatchNop(0x00713997, 1);
	PatchNop(0x00713998, 1);
	PatchNop(0x00713999, 1);
	PatchNop(0x0071399A, 1);
	PatchNop(0x0071399E, 1);
	PatchNop(0x0071399F, 1);
	PatchNop(0x007139A0, 1);
	PatchNop(0x007139A1, 1);
	PatchNop(0x007139A2, 1);
	PatchNop(0x007139E3, 1);
	PatchNop(0x007139E4, 1);
	PatchNop(0x007139E5, 1);
	PatchNop(0x007139E6, 1);
	PatchNop(0x007139E7, 1);
	PatchNop(0x007139E8, 1);
	PatchNop(0x007139E9, 1);
	PatchNop(0x007139EA, 1);
	PatchNop(0x007139EF, 1);
	PatchNop(0x007139F0, 1);
	PatchNop(0x007139F1, 1);
	PatchNop(0x007139F2, 1);
	PatchNop(0x00713A1E, 1);
	PatchNop(0x00713A1F, 1);
	PatchNop(0x00713A20, 1);
	PatchNop(0x00713A21, 1);
	PatchNop(0x00713A22, 1);
	PatchNop(0x00713A47, 1);
	PatchNop(0x00713A48, 1);
	PatchNop(0x00713A49, 1);
	PatchNop(0x00713A4A, 1);
	PatchNop(0x00713ACD, 1);
	PatchNop(0x00713ACE, 1);
	PatchNop(0x00713ACF, 1);
	PatchNop(0x00713AD0, 1);
	PatchNop(0x00713AD1, 1);
	PatchNop(0x00713AE3, 1);
	PatchNop(0x00713AE4, 1);
	PatchNop(0x00713AE5, 1);
	PatchNop(0x00713AE6, 1);
	PatchNop(0x00713AE7, 1);
	PatchNop(0x00713AF6, 1);
	PatchNop(0x00713AF7, 1);
	PatchNop(0x00713AF8, 1);
	PatchNop(0x00713AF9, 1);
	PatchNop(0x00713B0E, 1);
	PatchNop(0x00713B0F, 1);
	PatchNop(0x00713B10, 1);
	PatchNop(0x00713B3F, 1);
	PatchNop(0x00713B40, 1);
	PatchNop(0x00713B41, 1);
	PatchNop(0x00713B42, 1);
	PatchNop(0x00713B43, 1);
	PatchNop(0x00713B44, 1);
	PatchNop(0x00713B45, 1);
	PatchNop(0x00713B46, 1);
	PatchNop(0x00713B47, 1);
	PatchNop(0x00713B4C, 1);
	PatchNop(0x00713B4D, 1);
	PatchNop(0x00713B4E, 1);
	PatchNop(0x00713B9C, 1);
	PatchNop(0x00713B9D, 1);
	PatchNop(0x00713B9E, 1);
	PatchNop(0x00713B9F, 1);
	PatchNop(0x00713BA0, 1);
	PatchNop(0x00713BA1, 1);
	PatchNop(0x00713BD0, 1);
	PatchNop(0x00713BD1, 1);
	PatchNop(0x00713BD2, 1);
	PatchNop(0x00713BD3, 1);
	PatchNop(0x00713C5E, 1);
	PatchNop(0x00713C5F, 1);
	PatchNop(0x00713C60, 1);
	PatchNop(0x00713C61, 1);
	PatchNop(0x00713DD6, 1);
	PatchNop(0x00713DD7, 1);
	PatchNop(0x00713DD8, 1);
	PatchNop(0x00713DD9, 1);
	PatchNop(0x00713DDA, 1);
	PatchNop(0x00713DE1, 1);
	PatchNop(0x00713DE2, 1);
	PatchNop(0x00713DE3, 1);
	PatchNop(0x00713DE4, 1);
	PatchNop(0x00713DE5, 1);
	PatchNop(0x00713DE8, 1);
	PatchNop(0x00713DE9, 1);
	PatchNop(0x00713DEA, 1);
	PatchNop(0x00713DEB, 1);
	PatchNop(0x00713DEC, 1);
	PatchNop(0x00713E11, 1);
	PatchNop(0x00713E12, 1);
	PatchNop(0x00713E13, 1);
	PatchNop(0x00713E14, 1);
	PatchNop(0x00713E15, 1);
	PatchNop(0x00713E1D, 1);
	PatchNop(0x00713E1E, 1);
	PatchNop(0x00713E1F, 1);
	PatchNop(0x00713E20, 1);
	PatchNop(0x00713E21, 1);
	PatchNop(0x00713E2F, 1);
	PatchNop(0x00713E30, 1);
	PatchNop(0x00713E31, 1);
	PatchNop(0x00713E32, 1);
	PatchNop(0x00713E34, 1);
	PatchNop(0x00713E35, 1);
	PatchNop(0x00713E36, 1);
	PatchNop(0x00713E37, 1);
	PatchNop(0x00713E38, 1);
	PatchNop(0x00713E56, 1);
	PatchNop(0x00713E57, 1);
	PatchNop(0x00713E58, 1);
	PatchNop(0x00713E59, 1);
	PatchNop(0x00713E64, 1);
	PatchNop(0x00713E65, 1);
	PatchNop(0x00713E66, 1);
	PatchNop(0x00713E67, 1);
	PatchNop(0x00713E68, 1);
	PatchNop(0x00713E6B, 1);
	PatchNop(0x00713E6C, 1);
	PatchNop(0x00713E6D, 1);
	PatchNop(0x00713E6E, 1);
	PatchNop(0x00713E75, 1);
	PatchNop(0x00713E76, 1);
	PatchNop(0x00713E77, 1);
	PatchNop(0x00713E78, 1);
	PatchNop(0x00713E79, 1);
	PatchNop(0x00713E7A, 1);
	PatchNop(0x00713E7B, 1);
	PatchNop(0x00713E7C, 1);
	PatchNop(0x00713E7D, 1);
	PatchNop(0x00713E7E, 1);
	PatchNop(0x00713E7F, 1);
	PatchNop(0x00713E80, 1);
	PatchNop(0x00713E81, 1);
	PatchNop(0x00713E82, 1);
	PatchNop(0x00713E87, 1);
	PatchNop(0x00713E88, 1);
	PatchNop(0x00713E89, 1);
	PatchNop(0x00713E9F, 1);
	PatchNop(0x00713EA0, 1);
	PatchNop(0x00713EA1, 1);
	PatchNop(0x00713EA2, 1);
	PatchNop(0x00713EA3, 1);
	PatchNop(0x00713EA6, 1);
	PatchNop(0x00713EA7, 1);
	PatchNop(0x00713EA8, 1);
	PatchNop(0x00713EA9, 1);
	PatchNop(0x00713EC7, 1);
	PatchNop(0x00713EC8, 1);
	PatchNop(0x00713EC9, 1);
	PatchNop(0x00713ECA, 1);
	PatchNop(0x00713ED6, 1);
	PatchNop(0x00713ED7, 1);
	PatchNop(0x00713ED8, 1);
	PatchNop(0x00713ED9, 1);
	PatchNop(0x00713EDA, 1);
	PatchNop(0x00713EE5, 1);
	PatchNop(0x00713EE6, 1);
	PatchNop(0x00713EE7, 1);
	PatchNop(0x00713EE8, 1);
	PatchNop(0x00713EE9, 1);
	PatchNop(0x00713EF6, 1);
	PatchNop(0x00713EF7, 1);
	PatchNop(0x00713EF8, 1);
	PatchNop(0x00713EFE, 1);
	PatchNop(0x00713EFF, 1);
	PatchNop(0x00713F00, 1);
	PatchNop(0x00713F01, 1);
	PatchNop(0x00713F49, 1);
	PatchNop(0x00713F4A, 1);
	PatchNop(0x00713F4B, 1);
	PatchNop(0x00713F4C, 1);
	PatchNop(0x00713F4D, 1);
	PatchNop(0x00713F57, 1);
	PatchNop(0x00713F58, 1);
	PatchNop(0x00713F59, 1);
	PatchNop(0x00713F5A, 1);
	PatchNop(0x00713F5B, 1);
	PatchNop(0x00713F76, 1);
	PatchNop(0x00713F77, 1);
	PatchNop(0x00713F78, 1);
	PatchNop(0x00713F79, 1);
	PatchNop(0x00713F95, 1);
	PatchNop(0x00713F96, 1);
	PatchNop(0x00713F97, 1);
	PatchNop(0x00713F98, 1);
	PatchNop(0x00713F9E, 1);
	PatchNop(0x00713F9F, 1);
	PatchNop(0x00713FA0, 1);
	PatchNop(0x00713FA1, 1);
	PatchNop(0x00713FAC, 1);
	PatchNop(0x00713FAD, 1);
	PatchNop(0x00713FAE, 1);
	PatchNop(0x00713FAF, 1);
	PatchNop(0x00713FB0, 1);
	PatchNop(0x00713FDF, 1);
	PatchNop(0x00713FE0, 1);
	PatchNop(0x00713FE1, 1);
	PatchNop(0x00713FE2, 1);
	PatchNop(0x00713FF4, 1);
	PatchNop(0x00713FF5, 1);
	PatchNop(0x00713FF6, 1);
	PatchNop(0x00713FF7, 1);
	PatchNop(0x00714007, 1);
	PatchNop(0x00714008, 1);
	PatchNop(0x00714009, 1);
	PatchNop(0x0071400A, 1);
	PatchNop(0x00714012, 1);
	PatchNop(0x00714013, 1);
	PatchNop(0x00714014, 1);
	PatchNop(0x00714015, 1);
	PatchNop(0x00714017, 1);
	PatchNop(0x00714018, 1);
	PatchNop(0x00714019, 1);
	PatchNop(0x0071401A, 1);
	PatchNop(0x0071401B, 1);
	PatchNop(0x00714053, 1);
	PatchNop(0x00714054, 1);
	PatchNop(0x00714055, 1);
	PatchNop(0x00714056, 1);
	PatchNop(0x0071405F, 1);
	PatchNop(0x00714060, 1);
	PatchNop(0x00714061, 1);
	PatchNop(0x00714062, 1);
	PatchNop(0x00714094, 1);
	PatchNop(0x00714095, 1);
	PatchNop(0x00714096, 1);
	PatchNop(0x00714097, 1);
	PatchNop(0x00714098, 1);*/
	#pragma endregion

	/*
	ENABLE WINDOWED MODE
	mov     dword ptr[ebp - 0x1C], 0
	*/
	/*char win_bytes[] = { 0xC7, 0x45, 0xE4, 0x00, 0x00, 0x00, 0x00 };
	WriteBytes(0x0060A9BC, win_bytes, sizeof(win_bytes));*/
	WriteValue<DWORD>(0x0060A9BF, 0);
	
	/*
	Fix X position of popup bubble
	mov     ecx, 464
	*/
	/*char popup_bytes[]  = {0xB9, 0xD0, 0x01, 0x00, 0x00};
	WriteBytes(0x00477A17, popup_bytes, sizeof(popup_bytes));*/
	WriteValue<DWORD>(0x00477A18, 0x01D0);

	/*
	Skip GM check on slash commands
	*/
	PatchNop(0x0047C5C1, 2);

	/*
	Remove GM create party restriction
	*/
	PatchJmp(0x0047CA8C, (BYTE*)0x0047CAD0);

	/*
	Skip char creation block for non-scania characters (bug?)
	*/
	PatchJmp(0x0049F394, (BYTE*)0x0049F39F);

	#pragma region Quest fixes
	/*
	Changed from ebp-44 which overwrote lpFileTime at ebp-48.
	Still unknown what this does.

	mov     [ebp-40], eax
	*/
	char quest_bytes[] = { 0x89, 0x45, 0xD8 };
	WriteBytes(0x0051316A, quest_bytes, sizeof(quest_bytes));
	//WriteValue(0x0051316C, 0xD8);

	/*
	adc    DWORD PTR [ebp-52], edx
	lea    eax,[ebp-56]
	push   eax
	*/
	char quest2_bytes[] = {0x11, 0x55, 0xCC, 0x8D, 0x45, 0xC8, 0x50};
	WriteBytes(0x0051319C, quest2_bytes, sizeof(quest2_bytes));
	
	/*
	from: lea     eax, [ebp-56]
	to:   lea     eax, [ebp-48]
	*/
	//WriteValue<BYTE>(0x005131A5, 0xD0);
	char quest3_bytes[] = { 0x8D, 0x45, 0xD0 };
	WriteBytes(0x005131A3, quest3_bytes, sizeof(quest3_bytes));


	/*
	from: jg PREPARE_NO_RESULT
	to:   jl PREPARE_NO_RESULT
	*/
	//WriteValue<BYTE>(0x005131B0, 0x8C);
	char quest4_bytes[] = { 0x0F, 0x8C };
	WriteBytes(0x005131AF, quest4_bytes, sizeof(quest4_bytes));
	#pragma endregion

    #pragma region Fix reactor not sending hit delay
	WriteValue<BYTE>(0x005168A9, 0x80);
	WriteValue<BYTE>(0x005168AC, 0xFF);
	WriteValue<BYTE>(0x005168AE, 0x45);
	WriteValue<BYTE>(0x005168B0, 0xFF);
	WriteValue<BYTE>(0x005168BB, 0xFF);
	char r_bytes[] = { 0xA7, 0xF1, 0xFF };
	WriteBytes(0x005168C3, r_bytes, sizeof(r_bytes));
    #pragma endregion

	// Change skill tab width to fit 3 jobs
	WriteValue<BYTE>(0x00574AC5, 0x3C);

	// Skip nexon ad on game close
	PatchJmp(0x00608ECE, (BYTE*)0x00608F3D);

	// Inc dmg cap to 2 bil
	char dmg_bytes[] = { 0xC0, 0xFF, 0xFF, 0xFF, 0xDF, 0x41 };
	//WriteBytes(0x0067605A, dmg_bytes, sizeof(dmg_bytes));

	// IV_SHIFT_KEY
	/*char IV_SHIFT_KEY[] = {
		0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0, 0x71, 0xBF, 0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1,
		0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD, 0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0,
		0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE, 0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA,
		0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67, 0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA,
		0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9, 0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA,
		0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B, 0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31,
		0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5, 0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A,
		0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57, 0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5,
		0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72, 0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62,
		0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53, 0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C,
		0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38, 0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C,
		0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6, 0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF,
		0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94, 0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3,
		0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE, 0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A,
		0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9, 0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9,
		0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56, 0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49
	};
	WriteBytes(0x006BCD68, IV_SHIFT_KEY, sizeof(IV_SHIFT_KEY));*/

	// AES
	/*char AES_USER_KEY[] = {
		0x13, 0x52, 0x2A, 0x5B,
		0x08, 0x02, 0x10, 0x60,
		0x06, 0x02, 0x43, 0x0F,
		0xB4, 0x4B, 0x35, 0x05,
		0x1B, 0x0A, 0x5F, 0x09,
		0x0F, 0x50, 0x0C, 0x1B,
		0x33, 0x55, 0x01, 0x09,
		0x52, 0xDE, 0xC7, 0x1E,
	};
	WriteBytes(0x006B77D8, AES_USER_KEY, sizeof(AES_USER_KEY));*/

	// below hooks only serve as examples -- they will not do anything as-is
	//WriteValue<BYTE>(0x006BA6D0, 0x00);

	//INITMAPLEHOOK(
	//	_CClientSocket__ConnectIP_cdecl, // pointer to original function
	//	_CClientSocket__ConnectIP_cdecl_t, // function type
	//	MapleHooks::CClientSocket__ConnectIP_Hook, // function to detour to
	//	0x0043F14D
	//);

	//INITMAPLEHOOK(
	//	_ExampleFunc_thiscall, // pointer to original function
	//	_ExampleFunc_thiscall_t, // function type
	//	MapleHooks::ExampleFunc_thiscall2, // function to detour to
	//	0x0 // maple address
	//);

	//// edit memory

	//WriteValue<BYTE>(0x0, 0xEB); // address to write to, value to write
	//WriteValue<DWORD>(0x0, 0x42069);
	//WriteValue<double>(0x0, 420.69);

	//PatchNop(0x0, 6); // address to write to, number of nops

	//PatchRetZero(0x0); // function start address to return zero at
}

// prolly don't edit this region if youre a noob
#pragma region EntryThread

// main thread
VOID MainProc()
{
	Log(__FUNCTION__);

	Common::CreateInstance
	(
		TRUE,			// true if you want to hook windows libraries (besides mutex) set this to false if you already edited your IP into the client (eg v83 localhosts)
		MainFunc,		// function to be executed after client is unpacked
		"127.0.0.1",	// IP to connect to (your server IP)
		"66.151.108.51"	// IP to redirect from (nexon IP)
	);
}

// dll entry point
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	{
		Log("DLL_PROCESS_ATTACH");

		DisableThreadLibraryCalls(hModule);
		CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&MainProc, NULL, 0, 0);
		break;
	}
	case DLL_PROCESS_DETACH:
	{
		Log("DLL_PROCESS_DETACH");
		Common::GetInstance()->~Common();
		break;
	}
	}
	return TRUE;
}

#pragma endregion