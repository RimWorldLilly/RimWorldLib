using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02002411 RID: 9233
	public class DeflateStream : Stream
	{
		// Token: 0x0600E760 RID: 59232 RVA: 0x005601A5 File Offset: 0x0055E3A5
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600E761 RID: 59233 RVA: 0x005601B1 File Offset: 0x0055E3B1
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600E762 RID: 59234 RVA: 0x005601BD File Offset: 0x0055E3BD
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600E763 RID: 59235 RVA: 0x005601C9 File Offset: 0x0055E3C9
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x170028EC RID: 10476
		// (get) Token: 0x0600E764 RID: 59236 RVA: 0x005601ED File Offset: 0x0055E3ED
		// (set) Token: 0x0600E765 RID: 59237 RVA: 0x005601FA File Offset: 0x0055E3FA
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x170028ED RID: 10477
		// (get) Token: 0x0600E766 RID: 59238 RVA: 0x0056021B File Offset: 0x0055E41B
		// (set) Token: 0x0600E767 RID: 59239 RVA: 0x00560228 File Offset: 0x0055E428
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x170028EE RID: 10478
		// (get) Token: 0x0600E768 RID: 59240 RVA: 0x00560294 File Offset: 0x0055E494
		// (set) Token: 0x0600E769 RID: 59241 RVA: 0x005602A1 File Offset: 0x0055E4A1
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x170028EF RID: 10479
		// (get) Token: 0x0600E76A RID: 59242 RVA: 0x005602C2 File Offset: 0x0055E4C2
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x170028F0 RID: 10480
		// (get) Token: 0x0600E76B RID: 59243 RVA: 0x005602D4 File Offset: 0x0055E4D4
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600E76C RID: 59244 RVA: 0x005602E8 File Offset: 0x0055E4E8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170028F1 RID: 10481
		// (get) Token: 0x0600E76D RID: 59245 RVA: 0x00560334 File Offset: 0x0055E534
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x170028F2 RID: 10482
		// (get) Token: 0x0600E76E RID: 59246 RVA: 0x00560359 File Offset: 0x0055E559
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028F3 RID: 10483
		// (get) Token: 0x0600E76F RID: 59247 RVA: 0x0056035C File Offset: 0x0055E55C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600E770 RID: 59248 RVA: 0x00560381 File Offset: 0x0055E581
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x170028F4 RID: 10484
		// (get) Token: 0x0600E771 RID: 59249 RVA: 0x005603A1 File Offset: 0x0055E5A1
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170028F5 RID: 10485
		// (get) Token: 0x0600E772 RID: 59250 RVA: 0x005603A8 File Offset: 0x0055E5A8
		// (set) Token: 0x0600E773 RID: 59251 RVA: 0x005603F4 File Offset: 0x0055E5F4
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600E774 RID: 59252 RVA: 0x005603FB File Offset: 0x0055E5FB
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600E775 RID: 59253 RVA: 0x0056041E File Offset: 0x0055E61E
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600E776 RID: 59254 RVA: 0x00560425 File Offset: 0x0055E625
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600E777 RID: 59255 RVA: 0x0056042C File Offset: 0x0055E62C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600E778 RID: 59256 RVA: 0x00560450 File Offset: 0x0055E650
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E779 RID: 59257 RVA: 0x00560498 File Offset: 0x0055E698
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E77A RID: 59258 RVA: 0x005604E0 File Offset: 0x0055E6E0
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600E77B RID: 59259 RVA: 0x00560524 File Offset: 0x0055E724
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x040092E2 RID: 37602
		internal ZlibBaseStream _baseStream;

		// Token: 0x040092E3 RID: 37603
		internal Stream _innerStream;

		// Token: 0x040092E4 RID: 37604
		private bool _disposed;
	}
}
