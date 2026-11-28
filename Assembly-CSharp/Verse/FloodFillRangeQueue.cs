using System;

namespace Verse
{
	// Token: 0x020002ED RID: 749
	public class FloodFillRangeQueue
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x00088930 File Offset: 0x00086B30
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x00088938 File Offset: 0x00086B38
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x0008894C File Offset: 0x00086B4C
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new string[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded.ToString(),
					", MaxUsedSize= ",
					this.debugMaxUsedSpace.ToString(),
					", ClaimedSize=",
					this.array.Length.ToString(),
					", UnusedSpace=",
					(this.array.Length - this.debugMaxUsedSpace).ToString()
				});
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000889CD File Offset: 0x00086BCD
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000889F0 File Offset: 0x00086BF0
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			FloodFillRange[] array = this.array;
			int num = this.head;
			int num2 = this.count;
			this.count = num2 + 1;
			array[num + num2] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00088A90 File Offset: 0x00086C90
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04001385 RID: 4997
		private FloodFillRange[] array;

		// Token: 0x04001386 RID: 4998
		private int count;

		// Token: 0x04001387 RID: 4999
		private int head;

		// Token: 0x04001388 RID: 5000
		private int debugNumTimesExpanded;

		// Token: 0x04001389 RID: 5001
		private int debugMaxUsedSpace;
	}
}
