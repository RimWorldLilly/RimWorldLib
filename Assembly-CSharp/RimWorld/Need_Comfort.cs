using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156E RID: 5486
	public class Need_Comfort : Need_Seeker
	{
		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x06008690 RID: 34448 RVA: 0x0030A697 File Offset: 0x00308897
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastComfortUseTick >= Find.TickManager.TicksGame - 15)
				{
					return Mathf.Clamp01(this.lastComfortUsed);
				}
				return 0f;
			}
		}

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x06008691 RID: 34449 RVA: 0x0030A6C0 File Offset: 0x003088C0
		public ComfortCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.1f)
				{
					return ComfortCategory.Uncomfortable;
				}
				if (this.CurLevel < 0.6f)
				{
					return ComfortCategory.Normal;
				}
				if (this.CurLevel < 0.7f)
				{
					return ComfortCategory.Comfortable;
				}
				if (this.CurLevel < 0.8f)
				{
					return ComfortCategory.VeryComfortable;
				}
				if (this.CurLevel < 0.9f)
				{
					return ComfortCategory.ExtremelyComfortable;
				}
				return ComfortCategory.LuxuriantlyComfortable;
			}
		}

		// Token: 0x06008692 RID: 34450 RVA: 0x0030A71C File Offset: 0x0030891C
		public Need_Comfort(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.1f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.9f);
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x0030A78B File Offset: 0x0030898B
		public void ComfortUsed(float comfort)
		{
			this.lastComfortUsed = comfort;
			this.lastComfortUseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040052EA RID: 21226
		public float lastComfortUsed;

		// Token: 0x040052EB RID: 21227
		public int lastComfortUseTick;

		// Token: 0x040052EC RID: 21228
		private const float MinNormal = 0.1f;

		// Token: 0x040052ED RID: 21229
		private const float MinComfortable = 0.6f;

		// Token: 0x040052EE RID: 21230
		private const float MinVeryComfortable = 0.7f;

		// Token: 0x040052EF RID: 21231
		private const float MinExtremelyComfortablee = 0.8f;

		// Token: 0x040052F0 RID: 21232
		private const float MinLuxuriantlyComfortable = 0.9f;

		// Token: 0x040052F1 RID: 21233
		public const int ComfortUseInterval = 15;
	}
}
