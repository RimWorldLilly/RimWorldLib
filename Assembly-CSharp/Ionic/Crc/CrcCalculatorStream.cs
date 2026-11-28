using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x0200242A RID: 9258
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x0600E853 RID: 59475 RVA: 0x00566DA6 File Offset: 0x00564FA6
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600E854 RID: 59476 RVA: 0x00566DB6 File Offset: 0x00564FB6
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600E855 RID: 59477 RVA: 0x00566DC6 File Offset: 0x00564FC6
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600E856 RID: 59478 RVA: 0x00566DE2 File Offset: 0x00564FE2
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600E857 RID: 59479 RVA: 0x00566DFE File Offset: 0x00564FFE
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600E858 RID: 59480 RVA: 0x00566E1C File Offset: 0x0056501C
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._innerStream = stream;
			this._crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17002922 RID: 10530
		// (get) Token: 0x0600E859 RID: 59481 RVA: 0x00566E6C File Offset: 0x0056506C
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17002923 RID: 10531
		// (get) Token: 0x0600E85A RID: 59482 RVA: 0x00566E79 File Offset: 0x00565079
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17002924 RID: 10532
		// (get) Token: 0x0600E85B RID: 59483 RVA: 0x00566E86 File Offset: 0x00565086
		// (set) Token: 0x0600E85C RID: 59484 RVA: 0x00566E8E File Offset: 0x0056508E
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x0600E85D RID: 59485 RVA: 0x00566E98 File Offset: 0x00565098
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x0600E85E RID: 59486 RVA: 0x00566F06 File Offset: 0x00565106
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17002925 RID: 10533
		// (get) Token: 0x0600E85F RID: 59487 RVA: 0x00566F28 File Offset: 0x00565128
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17002926 RID: 10534
		// (get) Token: 0x0600E860 RID: 59488 RVA: 0x00566F35 File Offset: 0x00565135
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002927 RID: 10535
		// (get) Token: 0x0600E861 RID: 59489 RVA: 0x00566F38 File Offset: 0x00565138
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600E862 RID: 59490 RVA: 0x00566F45 File Offset: 0x00565145
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17002928 RID: 10536
		// (get) Token: 0x0600E863 RID: 59491 RVA: 0x00566F52 File Offset: 0x00565152
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x17002929 RID: 10537
		// (get) Token: 0x0600E864 RID: 59492 RVA: 0x00566F73 File Offset: 0x00565173
		// (set) Token: 0x0600E865 RID: 59493 RVA: 0x00566F80 File Offset: 0x00565180
		public override long Position
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600E866 RID: 59494 RVA: 0x00566F87 File Offset: 0x00565187
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E867 RID: 59495 RVA: 0x00566F8E File Offset: 0x0056518E
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E868 RID: 59496 RVA: 0x00566F95 File Offset: 0x00565195
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x0600E869 RID: 59497 RVA: 0x00566F9D File Offset: 0x0056519D
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x040093DD RID: 37853
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x040093DE RID: 37854
		private readonly Stream _innerStream;

		// Token: 0x040093DF RID: 37855
		private readonly CRC32 _crc32;

		// Token: 0x040093E0 RID: 37856
		private readonly long _lengthLimit = -99L;

		// Token: 0x040093E1 RID: 37857
		private bool _leaveOpen;
	}
}
