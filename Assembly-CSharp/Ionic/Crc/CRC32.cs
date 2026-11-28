using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x02002429 RID: 9257
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class CRC32
	{
		// Token: 0x17002920 RID: 10528
		// (get) Token: 0x0600E840 RID: 59456 RVA: 0x005668F8 File Offset: 0x00564AF8
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x17002921 RID: 10529
		// (get) Token: 0x0600E841 RID: 59457 RVA: 0x00566900 File Offset: 0x00564B00
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x0600E842 RID: 59458 RVA: 0x00566909 File Offset: 0x00564B09
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x0600E843 RID: 59459 RVA: 0x00566914 File Offset: 0x00564B14
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x0600E844 RID: 59460 RVA: 0x005669A8 File Offset: 0x00564BA8
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x0600E845 RID: 59461 RVA: 0x005669B2 File Offset: 0x00564BB2
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((W ^ (uint)B) & 255U)] ^ W >> 8);
		}

		// Token: 0x0600E846 RID: 59462 RVA: 0x005669C8 File Offset: 0x00564BC8
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)num2]);
				}
				else
				{
					uint num3 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)num3]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x0600E847 RID: 59463 RVA: 0x00566A5C File Offset: 0x00564C5C
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)num]);
				return;
			}
			uint num2 = (this._register & 255U) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)num2]);
		}

		// Token: 0x0600E848 RID: 59464 RVA: 0x00566ABC File Offset: 0x00564CBC
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((num >= 0U) ? num : (num + 256U))]);
				}
				else
				{
					uint num2 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((num2 >= 0U) ? num2 : (num2 + 256U))]);
				}
			}
		}

		// Token: 0x0600E849 RID: 59465 RVA: 0x00566B44 File Offset: 0x00564D44
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765U) << 1 | (data >> 1 & 1431655765U);
			num = ((num & 858993459U) << 2 | (num >> 2 & 858993459U));
			num = ((num & 252645135U) << 4 | (num >> 4 & 252645135U));
			return num << 24 | (num & 65280U) << 8 | (num >> 8 & 65280U) | num >> 24;
		}

		// Token: 0x0600E84A RID: 59466 RVA: 0x00566BB0 File Offset: 0x00564DB0
		private static byte ReverseBits(byte data)
		{
			int num = (int)data * 131586;
			uint num2 = 17055760U;
			uint num3 = (uint)(num & (int)num2);
			uint num4 = (uint)(num << 2 & (int)((int)num2 << 1));
			return (byte)(16781313U * (num3 + num4) >> 24);
		}

		// Token: 0x0600E84B RID: 59467 RVA: 0x00566BE4 File Offset: 0x00564DE4
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1U) == 1U)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x0600E84C RID: 59468 RVA: 0x00566C58 File Offset: 0x00564E58
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0U;
			int num2 = 0;
			while (vec != 0U)
			{
				if ((vec & 1U) == 1U)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x0600E84D RID: 59469 RVA: 0x00566C84 File Offset: 0x00564E84
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x0600E84E RID: 59470 RVA: 0x00566CAC File Offset: 0x00564EAC
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1U;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0U)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0U);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x0600E84F RID: 59471 RVA: 0x00566D63 File Offset: 0x00564F63
		public CRC32() : this(false)
		{
		}

		// Token: 0x0600E850 RID: 59472 RVA: 0x00566D6C File Offset: 0x00564F6C
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x0600E851 RID: 59473 RVA: 0x00566D7A File Offset: 0x00564F7A
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x0600E852 RID: 59474 RVA: 0x00566D9D File Offset: 0x00564F9D
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x040093D7 RID: 37847
		private uint dwPolynomial;

		// Token: 0x040093D8 RID: 37848
		private long _TotalBytesRead;

		// Token: 0x040093D9 RID: 37849
		private bool reverseBits;

		// Token: 0x040093DA RID: 37850
		private uint[] crc32Table;

		// Token: 0x040093DB RID: 37851
		private const int BUFFER_SIZE = 8192;

		// Token: 0x040093DC RID: 37852
		private uint _register = uint.MaxValue;
	}
}
