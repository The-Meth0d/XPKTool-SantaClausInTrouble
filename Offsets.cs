using System;

namespace XPKTool
{
	public class Offsets
	{
		public uint TOTAL_FILES;

		public uint FIXED_BLOCK_SIZE;

		public uint HEADER_SIZE = 4;

		public uint FILENAMES_SIZE = 4;

		public uint FILEDATA_SIZE = 4;

		public uint FILENAME_OFFS_SIZE;

		public uint FILES_SIZES_SIZE;

		public uint HASHTABLE_SIZE;

		public uint FILEDATA_OFFSETS_SIZE;

		public uint FILENAME_STRINGS_BLOCK_SIZE;

		public uint FILEDATA_BLOCK_SIZE;

		public uint PADDING_TO_FILEDATA
		{
			get
			{
				uint hEADERSIZE = this.HEADER_SIZE + this.FILENAME_OFFS_SIZE + this.FILENAMES_SIZE + this.FILENAME_STRINGS_BLOCK_SIZE + this.FILEDATA_SIZE + this.FILES_SIZES_SIZE + this.HASHTABLE_SIZE + this.FILEDATA_OFFSETS_SIZE;
				return hEADERSIZE;
			}
		}

		static Offsets()
		{
		}

		public Offsets(uint totalFiles)
		{
			this.TOTAL_FILES = totalFiles;
			this.FIXED_BLOCK_SIZE = totalFiles * 4;
			this.FILENAME_OFFS_SIZE = this.FIXED_BLOCK_SIZE;
			this.FILES_SIZES_SIZE = this.FIXED_BLOCK_SIZE;
			this.HASHTABLE_SIZE = this.FIXED_BLOCK_SIZE;
			this.FILEDATA_OFFSETS_SIZE = this.FIXED_BLOCK_SIZE;
		}

		public uint CalcNextFileDataOffset(int fileSize, int currentTotalSize)
		{
			return (uint)(fileSize + currentTotalSize);
		}

		public uint CalcNextStringOffset(int stringSize, int currentTotalSize)
		{
			return (uint)(stringSize + 1 + currentTotalSize);
		}
	}
}