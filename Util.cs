using System;
using System.IO;

namespace XPKTool
{
	public class Util
	{
		public Util()
		{
		}

		public static long ConvertToTimestamp(DateTime value)
		{
			return (value.Ticks - 621355968000000000L) / (long)10000000;
		}

		public static string ReadNullTerminatedString(BinaryReader br)
		{
			string str = "";
			while (true)
			{
				char chr = br.ReadChar();
				char chr1 = chr;
				if (chr <= '\0')
				{
					break;
				}
				str = string.Concat(str, chr1.ToString());
			}
			return str;
		}
	}
}