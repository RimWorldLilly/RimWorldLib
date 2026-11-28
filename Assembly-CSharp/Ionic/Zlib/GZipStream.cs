using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02002412 RID: 9234
	public class GZipStream : Stream
	{
		// Token: 0x170028F6 RID: 10486
		// (get) Token: 0x0600E77C RID: 59260 RVA: 0x00560568 File Offset: 0x0055E768
		// (set) Token: 0x0600E77D RID: 59261 RVA: 0x00560570 File Offset: 0x0055E770
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x170028F7 RID: 10487
		// (get) Token: 0x0600E77E RID: 59262 RVA: 0x0056058C File Offset: 0x0055E78C
		// (set) Token: 0x0600E77F RID: 59263 RVA: 0x00560594 File Offset: 0x0055E794
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x170028F8 RID: 10488
		// (get) Token: 0x0600E780 RID: 59264 RVA: 0x00560633 File Offset: 0x0055E833
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x0600E781 RID: 59265 RVA: 0x0056063B File Offset: 0x0055E83B
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600E782 RID: 59266 RVA: 0x00560647 File Offset: 0x0055E847
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600E783 RID: 59267 RVA: 0x00560653 File Offset: 0x0055E853
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600E784 RID: 59268 RVA: 0x0056065F File Offset: 0x0055E85F
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x170028F9 RID: 10489
		// (get) Token: 0x0600E785 RID: 59269 RVA: 0x0056067C File Offset: 0x0055E87C
		// (set) Token: 0x0600E786 RID: 59270 RVA: 0x00560689 File Offset: 0x0055E889
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x170028FA RID: 10490
		// (get) Token: 0x0600E787 RID: 59271 RVA: 0x005606AA File Offset: 0x0055E8AA
		// (set) Token: 0x0600E788 RID: 59272 RVA: 0x005606B8 File Offset: 0x0055E8B8
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x170028FB RID: 10491
		// (get) Token: 0x0600E789 RID: 59273 RVA: 0x00560724 File Offset: 0x0055E924
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170028FC RID: 10492
		// (get) Token: 0x0600E78A RID: 59274 RVA: 0x00560736 File Offset: 0x0055E936
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600E78B RID: 59275 RVA: 0x00560748 File Offset: 0x0055E948
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170028FD RID: 10493
		// (get) Token: 0x0600E78C RID: 59276 RVA: 0x005607A8 File Offset: 0x0055E9A8
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x170028FE RID: 10494
		// (get) Token: 0x0600E78D RID: 59277 RVA: 0x005607CD File Offset: 0x0055E9CD
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028FF RID: 10495
		// (get) Token: 0x0600E78E RID: 59278 RVA: 0x005607D0 File Offset: 0x0055E9D0
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600E78F RID: 59279 RVA: 0x005607F5 File Offset: 0x0055E9F5
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17002900 RID: 10496
		// (get) Token: 0x0600E790 RID: 59280 RVA: 0x00560815 File Offset: 0x0055EA15
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17002901 RID: 10497
		// (get) Token: 0x0600E791 RID: 59281 RVA: 0x0056081C File Offset: 0x0055EA1C
		// (set) Token: 0x0600E792 RID: 59282 RVA: 0x0056087D File Offset: 0x0055EA7D
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600E793 RID: 59283 RVA: 0x00560884 File Offset: 0x0055EA84
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0600E794 RID: 59284 RVA: 0x005608E3 File Offset: 0x0055EAE3
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600E795 RID: 59285 RVA: 0x005608EA File Offset: 0x0055EAEA
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600E796 RID: 59286 RVA: 0x005608F4 File Offset: 0x0055EAF4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600E797 RID: 59287 RVA: 0x00560954 File Offset: 0x0055EB54
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x0600E798 RID: 59288 RVA: 0x00560AF0 File Offset: 0x0055ECF0
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E799 RID: 59289 RVA: 0x00560B38 File Offset: 0x0055ED38
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E79A RID: 59290 RVA: 0x00560B80 File Offset: 0x0055ED80
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600E79B RID: 59291 RVA: 0x00560BC4 File Offset: 0x0055EDC4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x040092E5 RID: 37605
		public DateTime? LastModified;

		// Token: 0x040092E6 RID: 37606
		private int _headerByteCount;

		// Token: 0x040092E7 RID: 37607
		internal ZlibBaseStream _baseStream;

		// Token: 0x040092E8 RID: 37608
		private bool _disposed;

		// Token: 0x040092E9 RID: 37609
		private bool _firstReadDone;

		// Token: 0x040092EA RID: 37610
		private string _FileName;

		// Token: 0x040092EB RID: 37611
		private string _Comment;

		// Token: 0x040092EC RID: 37612
		private int _Crc32;

		// Token: 0x040092ED RID: 37613
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x040092EE RID: 37614
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
