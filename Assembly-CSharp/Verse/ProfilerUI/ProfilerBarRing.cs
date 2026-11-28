using System;

namespace Verse.ProfilerUI
{
	// Token: 0x02000788 RID: 1928
	public sealed class ProfilerBarRing
	{
		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x0017FFE5 File Offset: 0x0017E1E5
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		public float this[int newestOffset]
		{
			get
			{
				int num = this.newestIndex - newestOffset;
				if (num < 0)
				{
					num += this.values.Length;
				}
				return this.values[num];
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x0018001D File Offset: 0x0017E21D
		public void Clear()
		{
			this.newestIndex = -1;
			this.count = 0;
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0018002D File Offset: 0x0017E22D
		public void EnsureCapacity(int capacity)
		{
			if (this.values.Length == capacity)
			{
				return;
			}
			this.values = new float[capacity];
			this.Clear();
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x00180050 File Offset: 0x0017E250
		public void Push(float value)
		{
			this.newestIndex = ((this.newestIndex + 1 == this.values.Length) ? 0 : (this.newestIndex + 1));
			this.values[this.newestIndex] = value;
			if (this.count < this.values.Length)
			{
				this.count++;
			}
		}

		// Token: 0x0400278A RID: 10122
		private float[] values = Array.Empty<float>();

		// Token: 0x0400278B RID: 10123
		private int newestIndex = -1;

		// Token: 0x0400278C RID: 10124
		private int count;
	}
}
