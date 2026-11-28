using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001586 RID: 5510
	public class Need_Sadism : Need
	{
		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x06008707 RID: 34567 RVA: 0x0030CE0A File Offset: 0x0030B00A
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x06008708 RID: 34568 RVA: 0x0030CE19 File Offset: 0x0030B019
		public bool IsCritical
		{
			get
			{
				return this.CurLevel < 0.1f;
			}
		}

		// Token: 0x06008709 RID: 34569 RVA: 0x0030CE28 File Offset: 0x0030B028
		public Need_Sadism(Pawn newPawn) : base(newPawn)
		{
		}

		// Token: 0x0600870A RID: 34570 RVA: 0x0030CE31 File Offset: 0x0030B031
		public override void NeedInterval()
		{
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.def.fallPerDay * 0.0025f;
		}
	}
}
