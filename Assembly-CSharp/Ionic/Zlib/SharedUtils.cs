using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02002420 RID: 9248
	internal class SharedUtils
	{
		// Token: 0x0600E7EB RID: 59371 RVA: 0x00565122 File Offset: 0x00563322
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x0600E7EC RID: 59372 RVA: 0x0056512C File Offset: 0x0056332C
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x0600E7ED RID: 59373 RVA: 0x0056516D File Offset: 0x0056336D
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x0600E7EE RID: 59374 RVA: 0x0056517A File Offset: 0x0056337A
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
