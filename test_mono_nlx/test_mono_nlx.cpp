// test_mono_nlx.cpp : Defines the entry point for the console application.
//

#include <mono/jit/jit.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/assembly.h>
//#include <mono/metadata/mono-config.h>
#include <mono/metadata/debug-helpers.h>
#include <stdlib.h>

#pragma warning(disable: 4996)
#include "../eglib/src/glib.h"
extern "C" {
#include "../mono/mini/aot-compiler.h"
}
#pragma warning(default: 4996)

#include "stdafx.h"

#if _WIN64
#pragma comment(lib, "C:\\terminus_code\\mono\\msvc\\x64\\lib\\Debug_SGen\\libmonoruntimesgen.lib")
#pragma comment(lib, "C:\\terminus_code\\mono\\msvc\\x64\\bin\\Debug_SGen\\monosgen-2.0.lib")
#else
#pragma comment(lib, "C:\\terminus_code\\mono\\msvc\\Win32\\lib\\Debug_SGen\\libmonoruntimesgen.lib")
#pragma comment(lib, "C:\\terminus_code\\mono\\msvc\\Win32\\bin\\Debug_SGen\\monosgen-2.0.lib")
#endif

/*
* Very simple mono embedding example.
* Compile with:
* 	gcc -o teste teste.c `pkg-config --cflags --libs mono-2` -lm
* 	mcs test.cs
* Run with:
* 	./teste test.exe
*/

static MonoString*
gimme() {
	return mono_string_new(mono_domain_get(), "All your monos are belong to us!");
}

// TODO: mono_aot_xxx
static int
execute_system(const char * command)
{
	int status;

#if _WIN32
	// We need an extra set of quotes around the whole command to properly handle commands 
	// with spaces since internally the command is called through "cmd /c.
	command = g_strdup_printf("\"%s\"", command);

	int size = MultiByteToWideChar(CP_UTF8, 0, command, -1, NULL, 0);
	wchar_t* wstr = (wchar_t*)g_malloc(sizeof(wchar_t) * size);
	MultiByteToWideChar(CP_UTF8, 0, command, -1, wstr, size);
	status = _wsystem(wstr);
	g_free(wstr);

	g_free((void*)command);
#elif defined (HAVE_SYSTEM)
	status = system(command);
#else
	g_assert_not_reached();
#endif

	return status;
}

static void main_function(MonoDomain *domain, const char *file)
{
	MonoAssembly *assembly;
	MonoImage *image;

	assembly = mono_domain_assembly_open(domain, file);
	if (!assembly)
		exit(2);

	image = mono_assembly_get_image(assembly);
	if (!image)
		exit(2);

	//execute_system("gcc -shared -o"
		//"C:\\terminus_code\\mono\\msvc\\x64\\bin\\lib\\mono\\4.5\\mscorlib.dll.dll.tmp C:\\Users\\Nate\\AppData\\Local\\Temp\\mono_aot_a19500.o");

	//mono_aot_set_binutils_prefix("x86_64-pc-mingw32-");
	//mono_compile_assembly(assembly, 0x161169ff, "full");

	/*
	* mono_jit_exec() will run the Main() method in the assembly.
	* The return value needs to be looked up from
	* System.Environment.ExitCode.
	*/
	//mono_jit_exec(domain, assembly, 0, NULL);

	MonoMethodDesc *methodDesc = mono_method_desc_new("ClassLibrary1.Class1:DoStuff()", true);
	if (!methodDesc)
		exit(2);

	MonoMethod *method = mono_method_desc_search_in_image(methodDesc, image);
	if (!method)
		exit(2);

	MonoObject *exception = NULL;
	MonoObject *result = mono_runtime_invoke(method, NULL, NULL, &exception);

	mono_method_desc_free(methodDesc);
}

int maint()
{
	MonoDomain *domain;
	const char *file;
	int retval;

	file = "C:\\terminus_code\\mono\\msvc\\ClassLibrary1\\bin\\Debug\\ClassLibrary1.dll";
	//--aot=full C:\terminus_code\mono\msvc\x64\bin\lib\mono\4.5\mscorlib.dll
	//file = "C:\\terminus_code\\mono\\msvc\\x64\\bin\\lib\\mono\\4.5\\mscorlib.dll";

	/*
	* Load the default Mono configuration file, this is needed
	* if you are planning on using the dllmaps defined on the
	* system configuration
	*/
	//mono_config_parse(NULL);
	mono_set_dirs("C:\\terminus_code\\monob\\lib", "C:\\terminus_code\\monob\\etc");
	mono_set_dirs("C:\\terminus_code\\mono\\msvc\\x64\\bin\\lib", "C:\\terminus_code\\monob\\etc");
	
	file = "C:\\terminus_code\\mono\\msvc\\x64\\bin\\lib\\mono\\4.5\\ClassLibrary1.dll";

	/*
	* mono_jit_init() creates a domain: each assembly is
	* loaded and run in a MonoDomain.
	*/
	//domain = 
	//static int blah = 0;
	//if (blah == 0)
	{
		mono_jit_set_aot_mode(MONO_AOT_MODE_FULL);

		domain = mono_jit_init(file);
		//blah = 1;
	}
	
	//domain = mono_domain_create_appdomain("EngineDomain", NULL);
	/*
	* We add our special internal call, so that C# code
	* can call us back.
	*/
	mono_add_internal_call("ClassLibrary1.Class1::GetString", gimme);

	main_function(domain, file);

	retval = mono_environment_exitcode_get();

	mono_domain_unload(domain);
	//mono_jit_cleanup(domain);
	return retval;
}

int main()
{
	for (;;)
		maint();

	return 0;
}

