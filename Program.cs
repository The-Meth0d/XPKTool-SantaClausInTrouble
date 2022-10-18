using System;
using System.IO;

namespace XPKTool
{
	internal class Program
	{
		public XPKArchive xpkArchive;

		public XPKCreator xpkCreator;

		public Program()
		{
		}

		private static void Main(string[] args)
		{
			Console.Title = "Meth0d's XPK Tool (v1.0)";
			Program program = new Program();
			Program.Write("");
			Program.Write("XPK Tool - Packer/Unpacker for XPK Game Files");
			Program.Write("----------------------------------------------------------------------------");
			Program.Write("Developed by Meth0d - https://meth0d.org");
			Program.Write("Compatible with: Santa Claus In Trouble (1 & 2) and Rosso Rabbit In Trouble");
			Program.Write("----------------------------------------------------------------------------");
			Program.Write("Visit https://santa.meth0d.org to join our Modding Discord Server.");
			Program.Write("----------------------------------------------------------------------------");
			Program.Write("");
			if (args.Length != 0)
			{
				program.xpkArchive = new XPKArchive();
				if (!File.Exists(args[0]))
				{
					program.xpkCreator = new XPKCreator(args[0]);
				}
				else if (Path.GetExtension(args[0]) != ".xpk")
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Program.Write("[ERROR] Invalid file format, not a .XPK file... Enter to close.");
					Console.ReadLine();
					Environment.Exit(0);
				}
				else
				{
					program.xpkArchive.ReadFile(args[0]);
				}
			}
			else
			{
				Program.Write(" {{ TOOL USAGE }}");
				Program.Write(" - This is a drag and drop software based.\n  - You may drag and drop a file or a folder to the executable.");
				Program.Write(" * To unpack: Drop a .XPK FILE on the executable.");
				Program.Write(" * To repack: Drop a FOLDER on the executable.");
				Program.Write("");
				Program.Write("   Enter to close.");
				Console.ReadLine();
				Environment.Exit(0);
			}
		}

		public static void Write(string text)
		{
			Console.WriteLine(string.Concat(" ", text));
		}
	}
}