using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001587 RID: 5511
	public abstract class Need_Seeker : Need
	{
		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x0600870B RID: 34571 RVA: 0x0030CE5C File Offset: 0x0030B05C
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.pawn.Awake())
				{
					return 0;
				}
				float curInstantLevelPercentage = base.CurInstantLevelPercentage;
				if (curInstantLevelPercentage > base.CurLevelPercentage + 0.05f)
				{
					return 1;
				}
				if (curInstantLevelPercentage < base.CurLevelPercentage - 0.05f)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600870C RID: 34572 RVA: 0x0030CEA2 File Offset: 0x0030B0A2
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600870D RID: 34573 RVA: 0x0030CEAC File Offset: 0x0030B0AC
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += this.def.seekerRisePerHour * 0.06f;
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}

		// Token: 0x04005380 RID: 21376
		private const float GUIArrowTolerance = 0.05f;
	}
}
