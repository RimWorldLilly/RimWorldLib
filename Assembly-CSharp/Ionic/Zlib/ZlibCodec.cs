using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02002426 RID: 9254
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17002916 RID: 10518
		// (get) Token: 0x0600E80F RID: 59407 RVA: 0x005660EC File Offset: 0x005642EC
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x0600E810 RID: 59408 RVA: 0x005660F4 File Offset: 0x005642F4
		public ZlibCodec()
		{
		}

		// Token: 0x0600E811 RID: 59409 RVA: 0x0056610C File Offset: 0x0056430C
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x0600E812 RID: 59410 RVA: 0x00566166 File Offset: 0x00564366
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x0600E813 RID: 59411 RVA: 0x00566174 File Offset: 0x00564374
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x0600E814 RID: 59412 RVA: 0x00566183 File Offset: 0x00564383
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x0600E815 RID: 59413 RVA: 0x00566194 File Offset: 0x00564394
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x0600E816 RID: 59414 RVA: 0x005661C9 File Offset: 0x005643C9
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x0600E817 RID: 59415 RVA: 0x005661EA File Offset: 0x005643EA
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x0600E818 RID: 59416 RVA: 0x00566211 File Offset: 0x00564411
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x0600E819 RID: 59417 RVA: 0x00566231 File Offset: 0x00564431
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600E81A RID: 59418 RVA: 0x0056623A File Offset: 0x0056443A
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600E81B RID: 59419 RVA: 0x0056624A File Offset: 0x0056444A
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600E81C RID: 59420 RVA: 0x0056625A File Offset: 0x0056445A
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600E81D RID: 59421 RVA: 0x00566271 File Offset: 0x00564471
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600E81E RID: 59422 RVA: 0x00566288 File Offset: 0x00564488
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x0600E81F RID: 59423 RVA: 0x005662DD File Offset: 0x005644DD
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x0600E820 RID: 59424 RVA: 0x005662FE File Offset: 0x005644FE
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x0600E821 RID: 59425 RVA: 0x0056631B File Offset: 0x0056451B
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x0600E822 RID: 59426 RVA: 0x0056633B File Offset: 0x0056453B
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x0600E823 RID: 59427 RVA: 0x0056635D File Offset: 0x0056455D
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x0600E824 RID: 59428 RVA: 0x00566394 File Offset: 0x00564594
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x0600E825 RID: 59429 RVA: 0x005664E0 File Offset: 0x005646E0
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x040093BC RID: 37820
		public byte[] InputBuffer;

		// Token: 0x040093BD RID: 37821
		public int NextIn;

		// Token: 0x040093BE RID: 37822
		public int AvailableBytesIn;

		// Token: 0x040093BF RID: 37823
		public long TotalBytesIn;

		// Token: 0x040093C0 RID: 37824
		public byte[] OutputBuffer;

		// Token: 0x040093C1 RID: 37825
		public int NextOut;

		// Token: 0x040093C2 RID: 37826
		public int AvailableBytesOut;

		// Token: 0x040093C3 RID: 37827
		public long TotalBytesOut;

		// Token: 0x040093C4 RID: 37828
		public string Message;

		// Token: 0x040093C5 RID: 37829
		internal DeflateManager dstate;

		// Token: 0x040093C6 RID: 37830
		internal InflateManager istate;

		// Token: 0x040093C7 RID: 37831
		internal uint _Adler32;

		// Token: 0x040093C8 RID: 37832
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x040093C9 RID: 37833
		public int WindowBits = 15;

		// Token: 0x040093CA RID: 37834
		public CompressionStrategy Strategy;
	}
}
