using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XPKTool
{
	public class XPKCreator
	{
		public string[] AllFiles;

		public uint TotalFiles;

		public string FolderName;

		public string FolderDirectory;

		public Offsets OffsetsUtil;

		public List<XPKFile> Files = new List<XPKFile>();

		public FileStream fs;

		public BinaryWriter bw;

		static XPKCreator()
		{
		}

		public XPKCreator(string folderDirectory)
		{
			if (!Directory.Exists(folderDirectory))
			{
				Console.WriteLine(" [ERROR] Folder does not exists, closing program...");
				Console.WriteLine(" [INFO]  Drag and drop a valid folder to the program.");
				Console.ReadLine();
				Environment.Exit(0);
			}
			this.AllFiles = Directory.GetFiles(folderDirectory, "*.*", SearchOption.AllDirectories);
			this.TotalFiles = (uint)this.AllFiles.Length;
			this.FolderName = Path.GetFileName(folderDirectory);
			this.FolderDirectory = folderDirectory;
			string str = string.Concat(this.FolderName, ".xpk");
			if (this.TotalFiles == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(" [ERROR]: No file was found on the specified directory... Enter to close!");
				Console.ReadLine();
				Environment.Exit(0);
			}
			else
			{
				if (File.Exists(str))
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					File.Copy(str, str.Replace(".xpk", ".xpk.bak"), true);
					Console.WriteLine(string.Concat(" [INFO] Creating file backup to ", this.FolderName, ".xpk.bak"));
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				Console.WriteLine(string.Concat(new object[] { " [INFO] Packing ", this.TotalFiles, " files from folder: (", this.FolderName, ") to file ", this.FolderName, ".xpk" }));
				this.OffsetsUtil = new Offsets(this.TotalFiles);
				this.fs = new FileStream(str, FileMode.Create);
				this.bw = new BinaryWriter(this.fs);
				for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
				{
					this.Files.Add(new XPKFile());
				}
				int length = 0;
				uint num = 0;
				uint num1 = 0;
				string[] allFiles = this.AllFiles;
				for (int j = 0; j < (int)allFiles.Length; j++)
				{
					string str1 = allFiles[j];
					FileInfo fileInfo = new FileInfo(str1);
					string str2 = str1.Replace(string.Concat(folderDirectory, "\\"), "").ToString();
					this.Files[length].Name = str2;
					this.Files[length].Size = (uint)fileInfo.Length;
					XPKFile item = this.Files[length];
					DateTime creationTime = fileInfo.CreationTime;
					item.CreationDate = Convert.ToUInt32(Util.ConvertToTimestamp(creationTime.ToUniversalTime()));
					this.Files[length].NameOffset = num1;
					num1 = this.OffsetsUtil.CalcNextStringOffset(str2.Length, (int)num1);
					num += (uint)fileInfo.Length;
					length++;
				}
				this.OffsetsUtil.FILENAME_STRINGS_BLOCK_SIZE = num1;
				this.OffsetsUtil.FILEDATA_BLOCK_SIZE = num;
				this.WriteHeader();
				this.WriteFilenameStringsOffsetsTable();
				this.WriteFilenameStringsTable();
				this.WriteFileSizesTable();
				this.WriteHashTable();
				this.WriteFileOffsetsTable();
				this.WriteFilesData();
				this.bw.Close();
				this.fs.Close();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(string.Concat(" [SUCCESS] File ", this.FolderName, ".xpk successfully created! Enter to close."));
				Console.ReadLine();
				Environment.Exit(0);
			}
		}

		public byte[] GetFileDataBytes(string filePath)
		{
			return File.ReadAllBytes(filePath);
		}

		public void WriteFilenameStringsOffsetsTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.bw.Write(this.Files[i].NameOffset);
			}
			this.bw.Write(this.OffsetsUtil.FILENAME_STRINGS_BLOCK_SIZE);
		}

		public void WriteFilenameStringsTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.bw.Write(Encoding.Default.GetBytes(this.Files[i].Name));
				this.bw.Write((byte)0);
			}
			this.bw.Write(this.OffsetsUtil.FILEDATA_BLOCK_SIZE);
		}

		public void WriteFileOffsetsTable()
		{
			uint pADDINGTOFILEDATA = this.OffsetsUtil.PADDING_TO_FILEDATA;
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.bw.Write(pADDINGTOFILEDATA);
				pADDINGTOFILEDATA = this.OffsetsUtil.CalcNextFileDataOffset((int)this.Files[i].Size, (int)pADDINGTOFILEDATA);
			}
		}

		public void WriteFilesData()
		{
			string[] allFiles = this.AllFiles;
			for (int i = 0; i < (int)allFiles.Length; i++)
			{
				string str = allFiles[i];
				this.bw.Write(this.GetFileDataBytes(str));
			}
		}

		public void WriteFileSizesTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.bw.Write(this.Files[i].Size);
			}
		}

		public void WriteHashTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.bw.Write(this.Files[i].CreationDate);
			}
		}

		public void WriteHeader()
		{
			this.bw.Write(this.TotalFiles);
		}
	}
}