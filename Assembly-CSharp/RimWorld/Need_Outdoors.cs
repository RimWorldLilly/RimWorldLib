using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200157E RID: 5502
	public class Need_Outdoors : Need
	{
		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x060086E8 RID: 34536 RVA: 0x0030C31B File Offset: 0x0030A51B
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

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x060086E9 RID: 34537 RVA: 0x0030C334 File Offset: 0x0030A534
		public OutdoorsCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.8f)
				{
					return OutdoorsCategory.Free;
				}
				if (this.CurLevel > 0.6f)
				{
					return OutdoorsCategory.NeedFreshAir;
				}
				if (this.CurLevel > 0.4f)
				{
					return OutdoorsCategory.CabinFeverLight;
				}
				if (this.CurLevel >= 0.2f)
				{
					return OutdoorsCategory.CabinFeverSevere;
				}
				if (this.CurLevel > 0.05f)
				{
					return OutdoorsCategory.Trapped;
				}
				return OutdoorsCategory.Entombed;
			}
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x060086EA RID: 34538 RVA: 0x0030C38D File Offset: 0x0030A58D
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x060086EB RID: 34539 RVA: 0x0030C398 File Offset: 0x0030A598
		private bool Disabled
		{
			get
			{
				return this.pawn.Dead || this.pawn.needs.PrefersIndoors;
			}
		}

		// Token: 0x060086EC RID: 34540 RVA: 0x0030C3BC File Offset: 0x0030A5BC
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x060086ED RID: 34541 RVA: 0x0030C42B File Offset: 0x0030A62B
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x060086EE RID: 34542 RVA: 0x0030C438 File Offset: 0x0030A638
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
			float b = 0.2f;
			bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
			RoofDef roofDef = this.pawn.Spawned ? this.pawn.Position.GetRoof(this.pawn.Map) : null;
			float num;
			if (!flag)
			{
				if (roofDef == null)
				{
					num = 5f;
				}
				else if (!roofDef.isThickRoof)
				{
					num = -0.32f;
				}
				else
				{
					num = -0.45f;
					b = 0f;
				}
			}
			else if (roofDef == null)
			{
				num = 8f;
			}
			else if (roofDef.isThickRoof)
			{
				num = -0.4f;
			}
			else
			{
				num = 1f;
			}
			if (this.pawn.InBed() && num < 0f)
			{
				num *= 0.2f;
			}
			num *= 0.0025f;
			float curLevel = this.CurLevel;
			if (num < 0f)
			{
				this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
			}
			else
			{
				this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
			}
			this.lastEffectiveDelta = this.CurLevel - curLevel;
		}

		// Token: 0x0400534E RID: 21326
		private const float Delta_IndoorsThickRoof = -0.45f;

		// Token: 0x0400534F RID: 21327
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04005350 RID: 21328
		private const float Delta_IndoorsThinRoof = -0.32f;

		// Token: 0x04005351 RID: 21329
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x04005352 RID: 21330
		private const float Delta_OutdoorsThinRoof = 1f;

		// Token: 0x04005353 RID: 21331
		private const float Delta_IndoorsNoRoof = 5f;

		// Token: 0x04005354 RID: 21332
		private const float Delta_OutdoorsNoRoof = 8f;

		// Token: 0x04005355 RID: 21333
		private const float DeltaFactor_InBed = 0.2f;

		// Token: 0x04005356 RID: 21334
		private float lastEffectiveDelta;
	}
}
