using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x0200070D RID: 1805
	public class MemoryMappedFileSpanWrapper : IDisposable
	{
		// Token: 0x06003906 RID: 14598 RVA: 0x0016E3F4 File Offset: 0x0016C5F4
		[return: TupleElementNames(new string[]
		{
			"file",
			"length"
		})]
		private static ValueTuple<MemoryMappedFile, long> OpenExistingMmf(string path)
		{
			return MemoryMappedFileSpanWrapper.OpenExistingMmf(new FileInfo(path), MemoryMappedFileAccess.Read, false);
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0016E404 File Offset: 0x0016C604
		[return: TupleElementNames(new string[]
		{
			"file",
			"length"
		})]
		private static ValueTuple<MemoryMappedFile, long> OpenExistingMmf(FileInfo info, MemoryMappedFileAccess access, bool suppressExistsCheck = false)
		{
			if (!suppressExistsCheck && !info.Exists)
			{
				throw new FileNotFoundException("File not found", info.FullName);
			}
			long length = info.Length;
			return new ValueTuple<MemoryMappedFile, long>(MemoryMappedFile.CreateFromFile(info.FullName, FileMode.Open, null, length, access), length);
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x0016E449 File Offset: 0x0016C649
		private MemoryMappedFileSpanWrapper([TupleElementNames(new string[]
		{
			"file",
			"length"
		})] ValueTuple<MemoryMappedFile, long> data, MemoryMappedFileAccess access)
		{
			this._access = access;
			this._length = data.Item2;
			this._file = data.Item1;
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0016E470 File Offset: 0x0016C670
		public MemoryMappedFileSpanWrapper(string path) : this(MemoryMappedFileSpanWrapper.OpenExistingMmf(path), MemoryMappedFileAccess.Read)
		{
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0016E47F File Offset: 0x0016C67F
		public MemoryMappedFileSpanWrapper(FileInfo info) : this(MemoryMappedFileSpanWrapper.OpenExistingMmf(info, MemoryMappedFileAccess.Read, false), MemoryMappedFileAccess.Read)
		{
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0016E490 File Offset: 0x0016C690
		public MemoryMappedFileSpanWrapper(FileInfo info, bool suppressExistsCheck) : this(MemoryMappedFileSpanWrapper.OpenExistingMmf(info, MemoryMappedFileAccess.Read, suppressExistsCheck), MemoryMappedFileAccess.Read)
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0016E4A1 File Offset: 0x0016C6A1
		public MemoryMappedFileSpanWrapper(string path, long newFileSize, bool create = true) : this(new ValueTuple<MemoryMappedFile, long>(MemoryMappedFile.CreateFromFile(path, create ? FileMode.Create : FileMode.Open, null, newFileSize, MemoryMappedFileAccess.ReadWrite), newFileSize), MemoryMappedFileAccess.ReadWrite)
		{
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x0600390D RID: 14605 RVA: 0x0016E4C0 File Offset: 0x0016C6C0
		public long FileSize
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0016E4C8 File Offset: 0x0016C6C8
		public void Dispose()
		{
			this.ReleaseView();
			MemoryMappedFile file = this._file;
			if (file == null)
			{
				return;
			}
			file.Dispose();
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0016E4E0 File Offset: 0x0016C6E0
		private void EnsureReadPointer()
		{
			if (this._pointer != null)
			{
				return;
			}
			this._accessor = this._file.CreateViewAccessor(0L, this._length, this._access);
			this._accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref this._pointer);
			if (this._pointer == null)
			{
				this._accessor.SafeMemoryMappedViewHandle.ReleasePointer();
				this._accessor.Dispose();
				this._file.Dispose();
				throw new InvalidOperationException("Failed to acquire pointer to memory mapped file");
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0016E568 File Offset: 0x0016C768
		private unsafe Span<byte> GetSpan(long offset, long length)
		{
			this.EnsureReadPointer();
			if (offset < 0L || offset >= this._length)
			{
				string paramName = "offset";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(offset);
				defaultInterpolatedStringHandler.AppendLiteral(" is out of range [0, ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._length);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new ArgumentOutOfRangeException(paramName, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (length < 0L || offset + length > this._length)
			{
				string paramName2 = "length";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(66, 4);
				defaultInterpolatedStringHandler2.AppendLiteral("Length ");
				defaultInterpolatedStringHandler2.AppendFormatted<long>(length);
				defaultInterpolatedStringHandler2.AppendLiteral(" is out of range [0, ");
				defaultInterpolatedStringHandler2.AppendFormatted<long>(this._length - offset);
				defaultInterpolatedStringHandler2.AppendLiteral(") given offset ");
				defaultInterpolatedStringHandler2.AppendFormatted<long>(offset);
				defaultInterpolatedStringHandler2.AppendLiteral(" and total file length ");
				defaultInterpolatedStringHandler2.AppendFormatted<long>(this._length);
				throw new ArgumentOutOfRangeException(paramName2, defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			if (length > 2147483647L)
			{
				string paramName3 = "length";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("Length must be less than int.MaxValue, was ");
				defaultInterpolatedStringHandler3.AppendFormatted<long>(length);
				throw new ArgumentOutOfRangeException(paramName3, defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			return new Span<byte>((void*)(this._pointer + offset), (int)length);
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x0016E6AF File Offset: 0x0016C8AF
		public Span<byte> GetSpan(long offset)
		{
			return this.GetSpan(offset, this._length - offset);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0016E6C0 File Offset: 0x0016C8C0
		public ReadOnlySpan<byte> GetReadOnlySpan(long offset, long length)
		{
			return this.GetSpan(offset, length);
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0016E6CF File Offset: 0x0016C8CF
		public T Read<[IsUnmanaged] T>(long offset) where T : struct, ValueType
		{
			return MemoryMarshal.Read<T>(this.GetReadOnlySpan(offset, (long)sizeof(T)));
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0016E6E4 File Offset: 0x0016C8E4
		public void Write<[IsUnmanaged] T>(long offset, T value) where T : struct, ValueType
		{
			MemoryMarshal.Write<T>(this.GetSpan(offset, (long)sizeof(T)), value);
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0016E6FB File Offset: 0x0016C8FB
		public void Write(long offset, ReadOnlySpan<byte> span)
		{
			span.CopyTo(this.GetSpan(offset, (long)span.Length));
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x0016E714 File Offset: 0x0016C914
		public void CopyTo(Stream other, long start, long length = -1L)
		{
			if (length == -1L)
			{
				length = this._length - start;
			}
			long num = length / 2147483647L;
			long num2 = length % 2147483647L;
			for (long num3 = 0L; num3 < num; num3 += 1L)
			{
				long offset = start + num3 * 2147483647L;
				other.Write(this.GetReadOnlySpan(offset, 2147483647L));
			}
			if (num2 > 0L)
			{
				other.Write(this.GetReadOnlySpan(start + num * 2147483647L, num2));
			}
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x0016E78C File Offset: 0x0016C98C
		private unsafe Span<byte> GetBufferForCopyFrom(long pos, long length)
		{
			if (this._pointer != null)
			{
				this.ReleaseView();
			}
			Span<byte> result;
			try
			{
				length = Math.Min(67108864L, length);
				this._accessor = this._file.CreateViewAccessor(pos, length, this._access);
				byte* pointer = null;
				this._accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
				result = new Span<byte>((void*)pointer, (int)length);
			}
			catch (Exception innerException)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to get buffer to write to MMF of size ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(this._length);
				defaultInterpolatedStringHandler.AppendLiteral(" at position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(pos);
				defaultInterpolatedStringHandler.AppendLiteral(" with length ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(length);
				throw new IOException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
			}
			return result;
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x0016E85C File Offset: 0x0016CA5C
		private void ReleaseView()
		{
			MemoryMappedViewAccessor accessor = this._accessor;
			if (accessor != null)
			{
				accessor.SafeMemoryMappedViewHandle.ReleasePointer();
			}
			MemoryMappedViewAccessor accessor2 = this._accessor;
			if (accessor2 != null)
			{
				accessor2.Dispose();
			}
			this._pointer = null;
			this._accessor = null;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0016E894 File Offset: 0x0016CA94
		public void CopyFrom(Stream other, long initialPos = 0L)
		{
			long num = initialPos;
			long num2 = this._length - initialPos;
			Span<byte> bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			for (;;)
			{
				int num3 = other.Read(bufferForCopyFrom);
				if (num3 == 0)
				{
					break;
				}
				num += (long)num3;
				num2 -= (long)num3;
				if (num2 <= 0L)
				{
					break;
				}
				bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			}
			this.ReleaseView();
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0016E8E0 File Offset: 0x0016CAE0
		public void CopyFrom(MemoryMappedFileSpanWrapper other, long readPos = 0L, long writePos = 0L)
		{
			long num = writePos;
			long num2 = Math.Min(this._length - writePos, other._length - readPos);
			Span<byte> bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			for (;;)
			{
				ReadOnlySpan<byte> readOnlySpan = other.GetReadOnlySpan(readPos, (long)bufferForCopyFrom.Length);
				readOnlySpan.CopyTo(bufferForCopyFrom);
				other.ReleaseView();
				this.ReleaseView();
				num += (long)readOnlySpan.Length;
				num2 -= (long)readOnlySpan.Length;
				readPos += (long)readOnlySpan.Length;
				if (num2 <= 0L)
				{
					break;
				}
				bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			}
			other.ReleaseView();
			this.ReleaseView();
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x0016E970 File Offset: 0x0016CB70
		public unsafe byte* GetDirectPointer()
		{
			return this._pointer;
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0016E978 File Offset: 0x0016CB78
		public MemoryMappedFileSpanWrapper.SimpleReadStream CreateReadStream(long start, long length = -1L)
		{
			if (length == -1L)
			{
				length = this._length - start;
			}
			return new MemoryMappedFileSpanWrapper.SimpleReadStream(this, start, length);
		}

		// Token: 0x0400259C RID: 9628
		private readonly MemoryMappedFileAccess _access;

		// Token: 0x0400259D RID: 9629
		private readonly MemoryMappedFile _file;

		// Token: 0x0400259E RID: 9630
		private MemoryMappedViewAccessor _accessor;

		// Token: 0x0400259F RID: 9631
		private unsafe byte* _pointer;

		// Token: 0x040025A0 RID: 9632
		private readonly long _length;

		// Token: 0x02002A71 RID: 10865
		public class SimpleReadStream : Stream
		{
			// Token: 0x06010669 RID: 67177 RVA: 0x005C694C File Offset: 0x005C4B4C
			internal SimpleReadStream(MemoryMappedFileSpanWrapper mmf, long start, long length)
			{
				this._mmf = mmf;
				this._pos = start;
				this._length = length;
			}

			// Token: 0x0601066A RID: 67178 RVA: 0x005C696C File Offset: 0x005C4B6C
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (this._pos + (long)count >= this._mmf._length)
				{
					int num = (int)(this._mmf._length - this._pos);
					if (num <= 0)
					{
						return 0;
					}
					count = num;
				}
				this._mmf.GetSpan(this._pos, (long)count).CopyTo(buffer.AsSpan(offset, count));
				this._pos += (long)count;
				return count;
			}

			// Token: 0x0601066B RID: 67179 RVA: 0x005C69E4 File Offset: 0x005C4BE4
			public override int Read(Span<byte> buffer)
			{
				if (this._pos + (long)buffer.Length >= this._mmf._length)
				{
					int num = (int)(this._mmf._length - this._pos);
					if (num <= 0)
					{
						return 0;
					}
					buffer = buffer.Slice(0, num);
				}
				this._mmf.GetReadOnlySpan(this._pos, (long)buffer.Length).CopyTo(buffer);
				this._pos += (long)buffer.Length;
				return buffer.Length;
			}

			// Token: 0x0601066C RID: 67180 RVA: 0x005C6A72 File Offset: 0x005C4C72
			public override int ReadByte()
			{
				if (this._pos >= this._length)
				{
					return -1;
				}
				int result = (int)this._mmf.Read<byte>(this._pos);
				this._pos += 1L;
				return result;
			}

			// Token: 0x0601066D RID: 67181 RVA: 0x005C6AA4 File Offset: 0x005C4CA4
			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
				case SeekOrigin.Begin:
					this._pos = offset;
					break;
				case SeekOrigin.Current:
					this._pos += offset;
					break;
				case SeekOrigin.End:
					this._pos = this._length + offset;
					break;
				default:
					throw new ArgumentOutOfRangeException("origin", origin, null);
				}
				return this._pos;
			}

			// Token: 0x0601066E RID: 67182 RVA: 0x005C6B06 File Offset: 0x005C4D06
			public override void Flush()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0601066F RID: 67183 RVA: 0x005C6B0D File Offset: 0x005C4D0D
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06010670 RID: 67184 RVA: 0x005C6B14 File Offset: 0x005C4D14
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17002BD2 RID: 11218
			// (get) Token: 0x06010671 RID: 67185 RVA: 0x005C6B1B File Offset: 0x005C4D1B
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17002BD3 RID: 11219
			// (get) Token: 0x06010672 RID: 67186 RVA: 0x005C6B1E File Offset: 0x005C4D1E
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17002BD4 RID: 11220
			// (get) Token: 0x06010673 RID: 67187 RVA: 0x005C6B21 File Offset: 0x005C4D21
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17002BD5 RID: 11221
			// (get) Token: 0x06010674 RID: 67188 RVA: 0x005C6B24 File Offset: 0x005C4D24
			public override long Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17002BD6 RID: 11222
			// (get) Token: 0x06010675 RID: 67189 RVA: 0x005C6B2C File Offset: 0x005C4D2C
			// (set) Token: 0x06010676 RID: 67190 RVA: 0x005C6B34 File Offset: 0x005C4D34
			public override long Position
			{
				get
				{
					return this._pos;
				}
				set
				{
					this._pos = value;
				}
			}

			// Token: 0x0400ADEC RID: 44524
			private readonly MemoryMappedFileSpanWrapper _mmf;

			// Token: 0x0400ADED RID: 44525
			private long _pos;

			// Token: 0x0400ADEE RID: 44526
			private readonly long _length;
		}
	}
}
