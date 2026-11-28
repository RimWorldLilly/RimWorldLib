using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02002419 RID: 9241
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x0600E7BE RID: 59326 RVA: 0x0056413A File Offset: 0x0056233A
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0600E7BF RID: 59327 RVA: 0x00564146 File Offset: 0x00562346
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0600E7C0 RID: 59328 RVA: 0x00564152 File Offset: 0x00562352
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0600E7C1 RID: 59329 RVA: 0x0056415E File Offset: 0x0056235E
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0600E7C2 RID: 59330 RVA: 0x0056416C File Offset: 0x0056236C
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17002903 RID: 10499
		// (get) Token: 0x0600E7C3 RID: 59331 RVA: 0x005641DB File Offset: 0x005623DB
		// (set) Token: 0x0600E7C4 RID: 59332 RVA: 0x005641E3 File Offset: 0x005623E3
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17002904 RID: 10500
		// (get) Token: 0x0600E7C5 RID: 59333 RVA: 0x005641EC File Offset: 0x005623EC
		// (set) Token: 0x0600E7C6 RID: 59334 RVA: 0x005641F4 File Offset: 0x005623F4
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17002905 RID: 10501
		// (get) Token: 0x0600E7C7 RID: 59335 RVA: 0x00564211 File Offset: 0x00562411
		// (set) Token: 0x0600E7C8 RID: 59336 RVA: 0x00564219 File Offset: 0x00562419
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17002906 RID: 10502
		// (get) Token: 0x0600E7C9 RID: 59337 RVA: 0x0056423A File Offset: 0x0056243A
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17002907 RID: 10503
		// (get) Token: 0x0600E7CA RID: 59338 RVA: 0x00564242 File Offset: 0x00562442
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x0600E7CB RID: 59339 RVA: 0x0056424C File Offset: 0x0056244C
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x0600E7CC RID: 59340 RVA: 0x00564304 File Offset: 0x00562504
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_98;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_98;
				}
				mustWait = true;
				IL_145:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_98:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_145;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x0600E7CD RID: 59341 RVA: 0x00564460 File Offset: 0x00562660
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x0600E7CE RID: 59342 RVA: 0x0056451C File Offset: 0x0056271C
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x0600E7CF RID: 59343 RVA: 0x00564583 File Offset: 0x00562783
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x0600E7D0 RID: 59344 RVA: 0x005645B8 File Offset: 0x005627B8
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x0600E7D1 RID: 59345 RVA: 0x0056461B File Offset: 0x0056281B
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x0600E7D2 RID: 59346 RVA: 0x00564631 File Offset: 0x00562831
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0600E7D3 RID: 59347 RVA: 0x0056463C File Offset: 0x0056283C
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x0600E7D4 RID: 59348 RVA: 0x00564704 File Offset: 0x00562904
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								Queue<int> toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x0600E7D5 RID: 59349 RVA: 0x005648A4 File Offset: 0x00562AA4
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object obj = this._latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				Queue<int> toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object obj = this._eLock;
				lock (obj)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x0600E7D6 RID: 59350 RVA: 0x005649C4 File Offset: 0x00562BC4
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x0600E7D7 RID: 59351 RVA: 0x00564A38 File Offset: 0x00562C38
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17002908 RID: 10504
		// (get) Token: 0x0600E7D8 RID: 59352 RVA: 0x00564AB0 File Offset: 0x00562CB0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002909 RID: 10505
		// (get) Token: 0x0600E7D9 RID: 59353 RVA: 0x00564AB3 File Offset: 0x00562CB3
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700290A RID: 10506
		// (get) Token: 0x0600E7DA RID: 59354 RVA: 0x00564AB6 File Offset: 0x00562CB6
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700290B RID: 10507
		// (get) Token: 0x0600E7DB RID: 59355 RVA: 0x00564AC3 File Offset: 0x00562CC3
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700290C RID: 10508
		// (get) Token: 0x0600E7DC RID: 59356 RVA: 0x00564ACA File Offset: 0x00562CCA
		// (set) Token: 0x0600E7DD RID: 59357 RVA: 0x00564AD7 File Offset: 0x00562CD7
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600E7DE RID: 59358 RVA: 0x00564ADE File Offset: 0x00562CDE
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E7DF RID: 59359 RVA: 0x00564AE5 File Offset: 0x00562CE5
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600E7E0 RID: 59360 RVA: 0x00564AEC File Offset: 0x00562CEC
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400934B RID: 37707
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x0400934C RID: 37708
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x0400934D RID: 37709
		private List<WorkItem> _pool;

		// Token: 0x0400934E RID: 37710
		private bool _leaveOpen;

		// Token: 0x0400934F RID: 37711
		private bool emitting;

		// Token: 0x04009350 RID: 37712
		private Stream _outStream;

		// Token: 0x04009351 RID: 37713
		private int _maxBufferPairs;

		// Token: 0x04009352 RID: 37714
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x04009353 RID: 37715
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04009354 RID: 37716
		private object _outputLock = new object();

		// Token: 0x04009355 RID: 37717
		private bool _isClosed;

		// Token: 0x04009356 RID: 37718
		private bool _firstWriteDone;

		// Token: 0x04009357 RID: 37719
		private int _currentlyFilling;

		// Token: 0x04009358 RID: 37720
		private int _lastFilled;

		// Token: 0x04009359 RID: 37721
		private int _lastWritten;

		// Token: 0x0400935A RID: 37722
		private int _latestCompressed;

		// Token: 0x0400935B RID: 37723
		private int _Crc32;

		// Token: 0x0400935C RID: 37724
		private CRC32 _runningCrc;

		// Token: 0x0400935D RID: 37725
		private object _latestLock = new object();

		// Token: 0x0400935E RID: 37726
		private Queue<int> _toWrite;

		// Token: 0x0400935F RID: 37727
		private Queue<int> _toFill;

		// Token: 0x04009360 RID: 37728
		private long _totalBytesProcessed;

		// Token: 0x04009361 RID: 37729
		private CompressionLevel _compressLevel;

		// Token: 0x04009362 RID: 37730
		private volatile Exception _pendingException;

		// Token: 0x04009363 RID: 37731
		private bool _handlingException;

		// Token: 0x04009364 RID: 37732
		private object _eLock = new object();

		// Token: 0x04009365 RID: 37733
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02003F27 RID: 16167
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400F939 RID: 63801
			None = 0U,
			// Token: 0x0400F93A RID: 63802
			NotUsed1 = 1U,
			// Token: 0x0400F93B RID: 63803
			EmitLock = 2U,
			// Token: 0x0400F93C RID: 63804
			EmitEnter = 4U,
			// Token: 0x0400F93D RID: 63805
			EmitBegin = 8U,
			// Token: 0x0400F93E RID: 63806
			EmitDone = 16U,
			// Token: 0x0400F93F RID: 63807
			EmitSkip = 32U,
			// Token: 0x0400F940 RID: 63808
			EmitAll = 58U,
			// Token: 0x0400F941 RID: 63809
			Flush = 64U,
			// Token: 0x0400F942 RID: 63810
			Lifecycle = 128U,
			// Token: 0x0400F943 RID: 63811
			Session = 256U,
			// Token: 0x0400F944 RID: 63812
			Synch = 512U,
			// Token: 0x0400F945 RID: 63813
			Instance = 1024U,
			// Token: 0x0400F946 RID: 63814
			Compress = 2048U,
			// Token: 0x0400F947 RID: 63815
			Write = 4096U,
			// Token: 0x0400F948 RID: 63816
			WriteEnter = 8192U,
			// Token: 0x0400F949 RID: 63817
			WriteTake = 16384U,
			// Token: 0x0400F94A RID: 63818
			All = 4294967295U
		}
	}
}
