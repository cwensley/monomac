//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011 Xamarin Inc.
// Copyright 2009-2010 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Options;
using System.Text;
using MonoMac.ObjCRuntime;

#if !MONOMAC
using MonoTouch.ObjCRuntime;
#endif

public class BindingTouch
{
#if MONOMAC
	static string baselibdll = "MonoMac.dll";
	static string RootNS = "MonoMac";
	static Type CoreObject = typeof(MonoMac.Foundation.NSObject);
	static string tool_name = "bmac";
	static string compiler = "csc";
	static string basedir = null;
	static string outdir = null;
	static string net_sdk = null;
#else
	static string baselibdll = "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll";
	static string RootNS = "MonoTouch";
	static Type CoreObject = typeof (MonoTouch.Foundation.NSObject);
	static string tool_name = "btouch";
	static string compiler = "/Developer/MonoTouch/usr/bin/smcs";
	static string net_sdk = null;
#endif

	public static string ToolName
	{
		get { return tool_name; }
	}

	static void ShowHelp(OptionSet os)
	{
		Console.WriteLine("{0} - Mono Objective-C API binder", tool_name);
		Console.WriteLine("Usage is:\n {0} [options] apifile1.cs [apifileN] [-s=core1.cs [-s=core2.cs]] [-x=extra1.cs [-x=extra2.cs]]", tool_name);

		os.WriteOptionDescriptions(Console.Out);
	}

	static int Main(string[] args)
	{
		try
		{
			return Main2(args);
		}
		catch (Exception ex)
		{
			ErrorHelper.Show(ex);
			return 1;
		}
	}

	static int Main2 (string [] args)
	{
		var touch = new BindingTouch ();
		return touch.Main3 (args);
	}

	int Main3 (string [] args)
	{
		bool show_help = false;
		bool zero_copy = false;
		bool alpha = false;
		string tmpdir = null;
		string ns = null;
		string outfile = null;
		bool delete_temp = true, debug = false;
		bool verbose = false;
		bool unsafef = true;
		bool external = false;
		bool pmode = true;
		bool nostdlib = false;
		bool clean_mono_path = false;
		bool native_exception_marshalling = false;
		bool inline_selectors = false;
		List<string> sources;
		var resources = new List<string>();
#if !MONOMAC
		var linkwith = new List<string> ();
#endif
		var references = new List<string>();
		var libs = new List<string>();
		var core_sources = new List<string>();
		var extra_sources = new List<string>();
		var defines = new List<string>();
		bool binding_third_party = true;
		string generate_file_list = null;

		var os = new OptionSet() {
			{ "h|?|help", "Displays the help", v => show_help = true },
			{ "a", "Include alpha bindings", v => alpha = true },
			{ "outdir=", "Sets the output directory for the temporary binding files", v => { outdir = v; }},
			{ "o|out=", "Sets the name of the output library", v => outfile = v },
			{ "tmpdir=", "Sets the working directory for temp files", v => { tmpdir = v; delete_temp = false; }},
			{ "debug", "Generates a debugging build of the binding", v => debug = true },
			{ "sourceonly=", "Only generates the source", v => generate_file_list = v },
			{ "ns=", "Sets the namespace for storing helper classes", v => ns = v },
			{ "unsafe", "Sets the unsafe flag for the build", v=> unsafef = true },
#if MONOMAC
			{ "core", "Use this to build monomac.dll", v => binding_third_party = false },
#else
			{ "core", "Use this to build monotouch.dll", v => binding_third_party = false },
#endif
			{ "r=", "Adds a reference", v => references.Add (v) },
			{ "lib=", "Adds the directory to the search path for the compiler", v => libs.Add (v) },
			{ "compiler=", "Sets the compiler to use", v => compiler = v },
			{ "basedir=", "Sets the base directory for source files", v => basedir = v },
			{ "sdk=", "Sets the .NET SDK to use", v => net_sdk = v },
			{ "d=", "Defines a symbol", v => defines.Add (v) },
			{ "s=", "Adds a source file required to build the API", v => core_sources.Add (v) },
			{ "v", "Sets verbose mode", v => verbose = true },
			{ "x=", "Adds the specified file to the build, used after the core files are compiled", v => extra_sources.Add (v) },
			{ "e", "Generates smaller classes that can not be subclassed (previously called 'external mode')", v => external = true },
			{ "p", "Sets private mode", v => pmode = false },
			{ "baselib=", "Sets the base library", v => baselibdll = v },
			{ "use-zero-copy", v=> zero_copy = true },
			{ "nostdlib", "Does not reference mscorlib.dll library", l => nostdlib = true },
			{ "no-mono-path", "Launches compiler with empty MONO_PATH", l => clean_mono_path = true },
			{ "native-exception-marshalling", "Enable the marshalling support for Objective-C exceptions", l => native_exception_marshalling = true },
			{ "inline-selectors:", "If Selector.GetHandle is inlined and does not need to be cached (default: false)", v => inline_selectors = string.Equals ("true", v, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty (v) },
#if !MONOMAC
			{ "link-with=,", "Link with a native library {0:FILE} to the binding, embedded as a resource named {1:ID}",
				(path, id) => {
					if (path == null || path.Length == 0)
						throw new Exception ("-link-with=FILE,ID requires a filename.");
					
					if (id == null || id.Length == 0)
						id = Path.GetFileName (path);
					
					if (linkwith.Contains (id))
						throw new Exception ("-link-with=FILE,ID cannot assign the same resource id to multiple libraries.");
					
					resources.Add (string.Format ("{0},{1}", path, id));
					linkwith.Add (id);
				}
			},
#endif
		};

		try
		{
			sources = os.Parse(args);
		}
		catch (Exception e)
		{
			Console.Error.WriteLine("{0}: {1}", tool_name, e.Message);
			Console.Error.WriteLine("see {0} --help for more information", tool_name);
			return 1;
		}

		if (show_help || sources.Count == 0)
		{
			Console.WriteLine("Error: no api file provided");
			ShowHelp(os);
			return 0;
		}

		if (alpha)
			defines.Add("ALPHA");

		if (tmpdir == null)
			tmpdir = GetWorkDir();

		if (outfile == null)
			outfile = Path.GetFileNameWithoutExtension(sources[0]) + ".dll";

		string refs = (references.Count > 0 ? "-r:" + String.Join(" -r:", references.ToArray()) : "");

		try
		{
			var api_file = sources[0];
			var tmpass = Path.Combine(tmpdir, "temp.dll");

			var exitCode = CompileSource(
				tmpass,
				verbose,
				true,
				unsafef,
				nostdlib,
				clean_mono_path,
				sources.Concat(core_sources),
				defines,
				references.Concat(new[] { Environment.GetCommandLineArgs()[0] }),
				libs);

			if (exitCode != 0)
			{
				Console.WriteLine("{0}: API binding contains errors.", tool_name);
				return 1;
			}

			Assembly api;
			try
			{
				api = Assembly.LoadFrom(tmpass);
			}
			catch (Exception e)
			{
				if (verbose)
					Console.WriteLine(e);

				Console.Error.WriteLine("Error loading API definition from {0}", tmpass);
				return 1;
			}

			Assembly baselib;
			try
			{
				baselib = Assembly.LoadFrom(baselibdll);
			}
			catch (Exception e)
			{
				if (verbose)
					Console.WriteLine(e);

				Console.Error.WriteLine("Error loading base library {0}", baselibdll);
				return 1;
			}

#if !MONOMAC
			foreach (object attr in api.GetCustomAttributes (typeof (LinkWithAttribute), true)) {
				LinkWithAttribute linkWith = (LinkWithAttribute) attr;
				
				if (!linkwith.Contains (linkWith.LibraryName)) {
					Console.Error.WriteLine ("Missing native library {0}, please use `--link-with' to specify the path to this library.", linkWith.LibraryName);
					return 1;
				}
			}
#endif

			var types = new List<Type>();
			foreach (var t in api.GetTypes())
			{
				if (t.GetCustomAttributes(typeof(BaseTypeAttribute), true).Length > 0 ||
					t.GetCustomAttributes(typeof(StaticAttribute), true).Length > 0)
					types.Add(t);
			}

			var g = new Generator(pmode, external, debug, types.ToArray())
			{
				MessagingNS = ns == null ? Path.GetFileNameWithoutExtension(api_file) : ns,
				CoreMessagingNS = RootNS + ".ObjCRuntime",
				BindThirdPartyLibrary = binding_third_party,
				CoreNSObject = CoreObject,
				BaseDir = outdir != null ? outdir : tmpdir,
				ZeroCopyStrings = zero_copy,
				NativeExceptionMarshalling = native_exception_marshalling,
#if MONOMAC
				OnlyX86 = true,
#endif
				Alpha = alpha,
				InlineSelectors = inline_selectors,
			};

			foreach (var mi in baselib.GetType(RootNS + ".ObjCRuntime.Messaging").GetMethods())
			{
				if (mi.Name.IndexOf("_objc_msgSend") != -1)
					g.RegisterMethodName(mi.Name);
			}

			g.Go();

			if (generate_file_list != null)
			{
				using (var f = File.CreateText(generate_file_list))
				{
					g.GeneratedFiles.ForEach(x => f.WriteLine(x));
				}
				return 0;
			}

			exitCode = CompileSource(
				outfile,
				verbose,
				false,
				unsafef,
				nostdlib,
				clean_mono_path,
				g.GeneratedFiles.Concat(core_sources).Concat(sources.Skip(1)).Concat(extra_sources),
				null,
				references,
				null,
				resources
			);

			if (exitCode != 0)
			{
				Console.WriteLine("{0}: API binding contains errors.", tool_name);
				return 1;
			}
		}
		finally
		{
			if (delete_temp && Directory.Exists(tmpdir))
				Directory.Delete(tmpdir, true);
		}
		return 0;
	}

	private static int CompileSource(string destination, bool verbose, bool debug, bool unsafef, bool nostdlib, bool clean_mono_path, IEnumerable<string> sources, List<string> defines, IEnumerable<string> references = null, IEnumerable<string> libs = null, IEnumerable<string> resources = null)
	{
		if (compiler == "dotnet")
			return CompileSourceDotNet(destination, verbose, debug, unsafef, nostdlib, clean_mono_path, sources, defines, references, libs, resources);
		else
			return CompileSourceCSC(destination, verbose, debug, unsafef, nostdlib, clean_mono_path, sources, defines, references, libs, resources);
	}

	private static int CompileSourceDotNet(string destination, bool verbose, bool debug, bool unsafef, bool nostdlib, bool clean_mono_path, IEnumerable<string> sources, List<string> defines, IEnumerable<string> references = null, IEnumerable<string> libs = null, IEnumerable<string> resources = null)
	{
		// only fool-proof way to compile is with a .csproj, that way we don't have to resolve assemblies manually..
		var proj = new StringBuilder();
		string basePath = basedir;
		if (string.IsNullOrEmpty(basePath))
			basePath = Directory.GetCurrentDirectory();
		proj.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");

		proj.AppendLine("<PropertyGroup>");
		proj.AppendLine($"  <AssemblyName>{Path.GetFileNameWithoutExtension(destination)}</AssemblyName>");
		proj.AppendLine($"  <TargetFramework>net6.0</TargetFramework>");
		proj.AppendLine($"  <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>");
		proj.AppendLine($"  <EnableDefaultItems>false</EnableDefaultItems>");
		proj.AppendLine($"  <SelfContained>false</SelfContained>");
		proj.AppendLine($"  <OutputPath>{Path.GetDirectoryName(destination)}</OutputPath>");
		if (unsafef)
			proj.AppendLine("  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
		if (nostdlib)
			proj.AppendLine("  <NoStdLib>true</NoStdLib>");

		if (defines != null)
			proj.AppendLine($"  <DefineConstants>{string.Join(";", defines)}</DefineConstants>");
		if (libs != null)
			proj.AppendLine($"  <ReferencePath>{string.Join(";", libs)}</ReferencePath>");
		proj.AppendLine("</PropertyGroup>");

		proj.AppendLine("<ItemGroup>");

		string Resolve(string file)
		{
			if (!Path.IsPathRooted(file))
				return Path.Combine(basePath, file);
			return file;
		}

		if (sources != null)
		{
			foreach (var source in sources)
			{
				proj.AppendLine($"<Compile Include=\"{Resolve(source)}\" />");
			}
		}
		if (references != null)
		{
			foreach (var reference in references)
			{
				proj.AppendLine($"<Reference Include=\"{Resolve(reference)}\" />");
			}
		}

		if (!string.IsNullOrEmpty(baselibdll))
				proj.AppendLine($"<Reference Include=\"{Resolve(baselibdll)}\" />");

		
		if (resources != null)
		{
			foreach (var resource in resources)
			{
				var resourceInfo = resource.Split(',');
				proj.Append($"<EmbeddedResource Include=\"{Resolve(resourceInfo[0])}\" ");
				if (resourceInfo.Length > 1)
					proj.Append($"LogicalName=\"{resourceInfo[1]}\"");
				proj.AppendLine(" />");
			}
		}
		proj.AppendLine("</ItemGroup>");

		proj.Append("</Project>");

		var projName = Path.Combine(Path.GetDirectoryName(destination), Path.GetFileNameWithoutExtension(destination) + ".csproj");
		var logName = Path.Combine(outdir, Path.GetFileNameWithoutExtension(destination) + ".binlog");

		File.WriteAllText(projName, proj.ToString());

		var cargs = new StringBuilder();
		cargs.Append($"build \"{projName}\" /nologo /consoleLoggerParameters:NoSummary /p:Configuration={(debug ? "Debug" : "Release")} /bl:\"{logName}\"");

		var si = new ProcessStartInfo(compiler, cargs.ToString())
		{
			UseShellExecute = false,
		};
		if (si.Environment.ContainsKey("MSBUILD_EXE_PATH"))
			si.Environment.Remove("MSBUILD_EXE_PATH");

		//foreach (var env in si.Environment)
		//{
		//	Console.WriteLine($"{env.Key}={env.Value}");
		//}


		if (verbose)
			Console.WriteLine("{0} {1}", si.FileName, si.Arguments);

		var p = Process.Start(si);
		p.WaitForExit();

		return p.ExitCode;
	}

	private static int CompileSourceCSC(string destination, bool verbose, bool debug, bool unsafef, bool nostdlib, bool clean_mono_path, IEnumerable<string> sources, List<string> defines, IEnumerable<string> references = null, IEnumerable<string> libs = null, IEnumerable<string> resources = null)
	{
		// -nowarn:436 is to avoid conflicts in definitions between core.dll and the sources
		var cargs = new StringBuilder();

		if (!string.IsNullOrEmpty(net_sdk))
			cargs.Append($"-sdk:{net_sdk} ");
		if (debug)
			cargs.Append("-debug ");
		cargs.Append("-target:library -nowarn:436 ");
		if (unsafef)
			cargs.Append("-unsafe ");
		if (nostdlib)
			cargs.Append("-nostdlib ");
		cargs.Append($"-out:{destination} ");
		cargs.Append(string.Join(" ", sources));
		cargs.Append(" ");
		cargs.Append(string.Join(" ", references.Select(r => "-r:" + r)));
		cargs.Append(" ");
		cargs.Append(string.Join(" ", defines.Select(x => "-define:" + x)));
		cargs.Append(" ");
		cargs.Append(string.Join(" ", libs.Select(l => "-lib:" + l)));

		if (!string.IsNullOrEmpty(baselibdll))
			cargs.Append($"-r:{baselibdll} ");


		var si = new ProcessStartInfo(compiler, cargs.ToString())
		{
			UseShellExecute = false,
		};
		if (clean_mono_path)
		{
			// HACK: We are calling btouch with forced 2.1 path but we need working mono for compiler
			si.EnvironmentVariables.Remove("MONO_PATH");
		}

		if (verbose)
			Console.WriteLine("{0} {1}", si.FileName, si.Arguments);

		// throw new Exception("blar");

		var p = Process.Start(si);
		p.WaitForExit();
		return p.ExitCode;
	}

	static string GetWorkDir()
	{
		while (true)
		{
			string p = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			if (Directory.Exists(p))
				continue;

			var di = Directory.CreateDirectory(p);
			return di.FullName;
		}
	}
}

