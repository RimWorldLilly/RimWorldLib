using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000091 RID: 145
	public class EventQueue
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00025A51 File Offset: 0x00023C51
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00025A59 File Offset: 0x00023C59
		public float Queued { get; private set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00025A62 File Offset: 0x00023C62
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x00025A6A File Offset: 0x00023C6A
		public float Threshold { get; private set; }

		// Token: 0x06000672 RID: 1650 RVA: 0x00025A73 File Offset: 0x00023C73
		public EventQueue(float threshold)
		{
			this.Threshold = threshold;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00025A82 File Offset: 0x00023C82
		public void Push(float amount)
		{
			this.Queued += amount;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00025A92 File Offset: 0x00023C92
		public bool Peek()
		{
			return this.Queued >= this.Threshold;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00025AA5 File Offset: 0x00023CA5
		public bool Pop()
		{
			if (this.Queued >= this.Threshold)
			{
				this.Queued -= this.Threshold;
				return true;
			}
			return false;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00025ACB File Offset: 0x00023CCB
		public int PopAll()
		{
			int result = Mathf.FloorToInt(this.Queued / this.Threshold);
			this.Queued %= this.Threshold;
			return result;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00025AF2 File Offset: 0x00023CF2
		public int Clear()
		{
			int result = Mathf.FloorToInt(this.Queued / this.Threshold);
			this.Queued = 0f;
			return result;
		}
	}
}
