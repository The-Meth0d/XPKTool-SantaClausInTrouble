using System;
using System.Collections.Generic;
using System.IO;

namespace XPKTool
{
	public class XPKArchive
	{
		public string Name;

		public string FilePath;

		public uint TotalFiles;

		public uint FileDataSizeOffset;

		public uint FileDataSize;

		public FileStream fs;

		public BinaryReader br;

		public List<XPKFile> Files = new List<XPKFile>();

		static XPKArchive()
		{
		}

		public XPKArchive()
		{
		}

		public void ExtractAll()
		{
			foreach (XPKFile file in this.Files)
			{
				Directory.CreateDirectory(string.Concat(this.Name, "\\", Path.GetDirectoryName(file.Name)));
				File.WriteAllBytes(string.Concat(this.Name, "\\", file.Name), this.GetFileData(file.DataOffset, (int)file.Size));
			}
		}

		public byte[] GetFileData(uint offset, int size)
		{
			this.br.BaseStream.Position = (long)offset;
			byte[] numArray = new byte[size];
			this.br.Read(numArray, 0, size);
			return numArray;
		}

		public void ReadFile(string filePath)
		{
			this.Name = Path.GetFileNameWithoutExtension(filePath);
			this.FilePath = filePath;
			this.fs = new FileStream(filePath, FileMode.Open);
			this.br = new BinaryReader(this.fs);
			this.TotalFiles = this.br.ReadUInt32();
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files.Add(new XPKFile());
			}
			this.ReadStringsOffsetTable();
			this.ReadStringsTable();
			this.ReadFileSizesTable();
			this.ReadHashTable();
			this.ReadFileDataOffsetsTable();
			this.ExtractAll();
			this.br.Close();
			this.fs.Close();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(string.Concat(new object[] { " [SUCCESS] File ", this.Name, ".xpk was successfully unpacked, ", this.TotalFiles, " files were extracted." }));
			Console.ReadLine();
		}

		public void ReadFileDataOffsetsTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files[i].DataOffset = this.br.ReadUInt32();
			}
		}

		public void ReadFileSizesTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files[i].Size = this.br.ReadUInt32();
			}
		}

		public void ReadHashTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files[i].CreationDate = this.br.ReadUInt32();
			}
		}

		public void ReadStringsOffsetTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files[i].NameOffset = this.br.ReadUInt32();
			}
			this.FileDataSizeOffset = this.br.ReadUInt32();
		}

		public void ReadStringsTable()
		{
			for (int i = 0; (long)i < (ulong)this.TotalFiles; i++)
			{
				this.Files[i].Name = Util.ReadNullTerminatedString(this.br);
			}
			this.FileDataSize = this.br.ReadUInt32();
		}
	}
}