using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001574 RID: 5492
	public class Need_Indoors : Need
	{
		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x060086B8 RID: 34488 RVA: 0x0030B162 File Offset: 0x00309362
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x060086B9 RID: 34489 RVA: 0x0030B16D File Offset: 0x0030936D
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x060086BA RID: 34490 RVA: 0x0030B184 File Offset: 0x00309384
		public IndoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > Need_Indoors.Thresholds[0])
				{
					return IndoorsCategory.ComfortablyIndoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[1])
				{
					return IndoorsCategory.JustOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[2])
				{
					return IndoorsCategory.Outdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[3])
				{
					return IndoorsCategory.LongOutdoors;
				}
				if (this.CurLevel > Need_Indoors.Thresholds[4])
				{
					return IndoorsCategory.VeryLongOutdoors;
				}
				return IndoorsCategory.BrutalOutdoors;
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x060086BB RID: 34491 RVA: 0x0030B1E7 File Offset: 0x003093E7
		private bool Disabled
		{
			get
			{
				return this.pawn.Dead || this.pawn.needs.PrefersOutdoors;
			}
		}

		// Token: 0x060086BC RID: 34492 RVA: 0x0030B208 File Offset: 0x00309408
		public Need_Indoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>(Need_Indoors.Thresholds);
		}

		// Token: 0x060086BD RID: 34493 RVA: 0x0030B221 File Offset: 0x00309421
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x060086BE RID: 34494 RVA: 0x0030B230 File Offset: 0x00309430
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float curLevel = this.CurLevel;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 0f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = 1f;
				}
				else
				{
					num = 2f;
				}
			}
			else if (roofDef == null)
			{
				num = -0.25f;
			}
			else if (roofDef.isThickRoof)
			{
				num = 0f;
			}
			else
			{
				num = -0.25f;
			}
			num *= 0.0025f;
			this.CurLevel = ((num < 0f) ? Mathf.Min(this.CurLevel, this.CurLevel + num) : Mathf.Min(this.CurLevel + num, 1f));
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x0400530C RID: 21260
		private static readonly float[] Thresholds = new float[]
		{
			0.8f,
			0.6f,
			0.4f,
			0.2f,
			0.05f
		};

		// Token: 0x0400530D RID: 21261
		private const float Delta_Indoors_ThickRoof = 2f;

		// Token: 0x0400530E RID: 21262
		private const float Delta_Indoors_ThinRoof = 1f;

		// Token: 0x0400530F RID: 21263
		private const float Delta_Indoors_NoRoof = 0f;

		// Token: 0x04005310 RID: 21264
		private const float Delta_Outdoors_ThickRoof = 0f;

		// Token: 0x04005311 RID: 21265
		private const float Delta_Outdoors_ThinRoof = -0.25f;

		// Token: 0x04005312 RID: 21266
		private const float Delta_Outdoors_NoRoof = -0.25f;

		// Token: 0x04005313 RID: 21267
		private float lastEffectiveDelta;
	}
}
