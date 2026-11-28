using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02002428 RID: 9256
	public class ZlibStream : Stream
	{
		// Token: 0x0600E826 RID: 59430 RVA: 0x0056656A File Offset: 0x0056476A
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600E827 RID: 59431 RVA: 0x00566576 File Offset: 0x00564776
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600E828 RID: 59432 RVA: 0x00566582 File Offset: 0x00564782
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600E829 RID: 59433 RVA: 0x0056658E File Offset: 0x0056478E
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17002917 RID: 10519
		// (get) Token: 0x0600E82A RID: 59434 RVA: 0x005665AB File Offset: 0x005647AB
		// (set) Token: 0x0600E82B RID: 59435 RVA: 0x005665B8 File Offset: 0x005647B8
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
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17002918 RID: 10520
		// (get) Token: 0x0600E82C RID: 59436 RVA: 0x005665D9 File Offset: 0x005647D9
		// (set) Token: 0x0600E82D RID: 59437 RVA: 0x005665E8 File Offset: 0x005647E8
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
					throw new ObjectDisposedException("ZlibStream");
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

		// Token: 0x17002919 RID: 10521
		// (get) Token: 0x0600E82E RID: 59438 RVA: 0x00566654 File Offset: 0x00564854
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700291A RID: 10522
		// (get) Token: 0x0600E82F RID: 59439 RVA: 0x00566666 File Offset: 0x00564866
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x0600E830 RID: 59440 RVA: 0x00566678 File Offset: 0x00564878
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

		// Token: 0x1700291B RID: 10523
		// (get) Token: 0x0600E831 RID: 59441 RVA: 0x005666C4 File Offset: 0x005648C4
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700291C RID: 10524
		// (get) Token: 0x0600E832 RID: 59442 RVA: 0x005666E9 File Offset: 0x005648E9
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700291D RID: 10525
		// (get) Token: 0x0600E833 RID: 59443 RVA: 0x005666EC File Offset: 0x005648EC
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600E834 RID: 59444 RVA: 0x00566711 File Offset: 0x00564911
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700291E RID: 10526
		// (get) Token: 0x0600E835 RID: 59445 RVA: 0x00566731 File Offset: 0x00564931
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700291F RID: 10527
		// (get) Token: 0x0600E836 RID: 59446 RVA: 0x00566738 File Offset: 0x00564938
		// (set) Token: 0x0600E837 RID: 59447 RVA: 0x00566784 File Offset: 0x00564984
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
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600E838 RID: 59448 RVA: 0x0056678B File Offset: 0x0056498B
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600E839 RID: 59449 RVA: 0x005667AE File Offset: 0x005649AE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E83A RID: 59450 RVA: 0x005667B5 File Offset: 0x005649B5
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E83B RID: 59451 RVA: 0x005667BC File Offset: 0x005649BC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600E83C RID: 59452 RVA: 0x005667E0 File Offset: 0x005649E0
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E83D RID: 59453 RVA: 0x00566828 File Offset: 0x00564A28
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600E83E RID: 59454 RVA: 0x00566870 File Offset: 0x00564A70
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0600E83F RID: 59455 RVA: 0x005668B4 File Offset: 0x00564AB4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x040093D5 RID: 37845
		internal ZlibBaseStream _baseStream;

		// Token: 0x040093D6 RID: 37846
		private bool _disposed;
	}
}
