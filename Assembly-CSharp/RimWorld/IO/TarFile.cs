using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002404 RID: 9220
	internal class TarFile : VirtualFile
	{
		// Token: 0x170028CE RID: 10446
		// (get) Token: 0x0600E6C0 RID: 59072 RVA: 0x0055CFBE File Offset: 0x0055B1BE
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170028CF RID: 10447
		// (get) Token: 0x0600E6C1 RID: 59073 RVA: 0x0055CFC6 File Offset: 0x0055B1C6
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x170028D0 RID: 10448
		// (get) Token: 0x0600E6C2 RID: 59074 RVA: 0x0055CFCE File Offset: 0x0055B1CE
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x170028D1 RID: 10449
		// (get) Token: 0x0600E6C3 RID: 59075 RVA: 0x0055CFD9 File Offset: 0x0055B1D9
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		// Token: 0x0600E6C4 RID: 59076 RVA: 0x0055CFE4 File Offset: 0x0055B1E4
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		// Token: 0x0600E6C5 RID: 59077 RVA: 0x0055D001 File Offset: 0x0055B201
		private TarFile()
		{
		}

		// Token: 0x0600E6C6 RID: 59078 RVA: 0x0055D009 File Offset: 0x0055B209
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		// Token: 0x0600E6C7 RID: 59079 RVA: 0x0055D019 File Offset: 0x0055B219
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		// Token: 0x0600E6C8 RID: 59080 RVA: 0x0055D02C File Offset: 0x0055B22C
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		// Token: 0x0600E6C9 RID: 59081 RVA: 0x0055D064 File Offset: 0x0055B264
		public override string[] ReadAllLines()
		{
			this.CheckAccess();
			List<string> list = new List<string>();
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					while (!streamReader.EndOfStream)
					{
						list.Add(streamReader.ReadLine());
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600E6CA RID: 59082 RVA: 0x0055D0E0 File Offset: 0x0055B2E0
		public override string ReadAllText()
		{
			this.CheckAccess();
			string result;
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x0600E6CB RID: 59083 RVA: 0x0055D144 File Offset: 0x0055B344
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		// Token: 0x04009281 RID: 37505
		public static readonly TarFile NotFound = new TarFile();

		// Token: 0x04009282 RID: 37506
		public byte[] data;

		// Token: 0x04009283 RID: 37507
		public string fullPath;

		// Token: 0x04009284 RID: 37508
		public string name;
	}
}
