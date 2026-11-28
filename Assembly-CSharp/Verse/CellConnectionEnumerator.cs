using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200035D RID: 861
	public struct CellConnectionEnumerator : IEnumerator<CellConnection>, IEnumerator, IDisposable
	{
		// Token: 0x06001A66 RID: 6758 RVA: 0x000A19B1 File Offset: 0x0009FBB1
		internal CellConnectionEnumerator(CellConnection bitmask)
		{
			this.bitmask = bitmask;
			this.current = CellConnection.Self;
			this.index = bitmask.BitIndex();
			this.end = bitmask.BitLoopEndIndex();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000A19D9 File Offset: 0x0009FBD9
		public void Dispose()
		{
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000A19DC File Offset: 0x0009FBDC
		public bool MoveNext()
		{
			while (this.index < this.end)
			{
				CellConnection[] flagFromBitIndex = CellConnectionExtensions.FlagFromBitIndex;
				int num = this.index;
				this.index = num + 1;
				CellConnection testFlag = flagFromBitIndex[num];
				if (this.bitmask.HasBit(testFlag))
				{
					this.current = testFlag;
					return true;
				}
			}
			this.current = CellConnection.Self;
			return false;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x000A1A30 File Offset: 0x0009FC30
		public CellConnection Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x000A1A38 File Offset: 0x0009FC38
		object IEnumerator.Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x000A1A45 File Offset: 0x0009FC45
		void IEnumerator.Reset()
		{
			this.current = CellConnection.Self;
			this.index = this.bitmask.BitIndex();
			this.end = this.bitmask.BitLoopEndIndex();
		}

		// Token: 0x04001587 RID: 5511
		private readonly CellConnection bitmask;

		// Token: 0x04001588 RID: 5512
		private CellConnection current;

		// Token: 0x04001589 RID: 5513
		private int index;

		// Token: 0x0400158A RID: 5514
		private int end;
	}
}
