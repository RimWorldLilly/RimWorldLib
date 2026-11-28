using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F87 RID: 8071
	public class StatPart_GearStatFactor : StatPart
	{
		// Token: 0x0600CD3C RID: 52540 RVA: 0x004B78FC File Offset: 0x004B5AFC
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing || req.Thing == null)
			{
				return;
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					val *= pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1);
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				val *= pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
			}
		}

		// Token: 0x0600CD3D RID: 52541 RVA: 0x004B79B0 File Offset: 0x004B5BB0
		public override string ExplanationPart(StatRequest req)
		{
			if (!req.HasThing || req.Thing == null)
			{
				return null;
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null || !this.PawnWearingRelevantGear(pawn))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = pawn.apparel.WornApparel[i];
					float statValue = apparel.GetStatValue(this.apparelStat, true, -1);
					if (!Mathf.Approximately(statValue, 1f))
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(apparel, statValue));
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float statValue2 = pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
				if (!Mathf.Approximately(statValue2, 1f))
				{
					stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary, statValue2));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600CD3E RID: 52542 RVA: 0x004B7AD0 File Offset: 0x004B5CD0
		private string InfoTextLineFrom(Thing gear, float value)
		{
			return "    " + gear.LabelCap + ": " + value.ToStringByStyle(this.apparelStat.toStringStyle, ToStringNumberSense.Factor);
		}

		// Token: 0x0600CD3F RID: 52543 RVA: 0x004B7AFC File Offset: 0x004B5CFC
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					if (!Mathf.Approximately(pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1), 1f))
					{
						return true;
					}
				}
			}
			return this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && !Mathf.Approximately(pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1), 1f);
		}

		// Token: 0x0600CD40 RID: 52544 RVA: 0x004B7B9C File Offset: 0x004B5D9C
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			StatPart_GearStatFactor.<GetInfoCardHyperlinks>d__6 <GetInfoCardHyperlinks>d__ = new StatPart_GearStatFactor.<GetInfoCardHyperlinks>d__6(-2);
			<GetInfoCardHyperlinks>d__.<>4__this = this;
			<GetInfoCardHyperlinks>d__.<>3__req = req;
			return <GetInfoCardHyperlinks>d__;
		}

		// Token: 0x040082FF RID: 33535
		private StatDef apparelStat;

		// Token: 0x04008300 RID: 33536
		private bool includeWeapon;
	}
}
