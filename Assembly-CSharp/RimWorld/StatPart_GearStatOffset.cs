using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F88 RID: 8072
	public class StatPart_GearStatOffset : StatPart
	{
		// Token: 0x0600CD42 RID: 52546 RVA: 0x004B7BBC File Offset: 0x004B5DBC
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
					float num = pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true, -1);
					num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[i], this.apparelStat);
					if (this.subtract)
					{
						val -= num;
					}
					else
					{
						val += num;
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float num2 = pawn.equipment.Primary.GetStatValue(this.apparelStat, true, -1);
				num2 += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat);
				if (this.subtract)
				{
					val -= num2;
					return;
				}
				val += num2;
			}
		}

		// Token: 0x0600CD43 RID: 52547 RVA: 0x004B7CC8 File Offset: 0x004B5EC8
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
					Apparel gear = pawn.apparel.WornApparel[i];
					float num = this.GetStatValueForGear(gear);
					if (this.subtract)
					{
						num = -num;
					}
					if (!Mathf.Approximately(num, 0f))
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(gear, num));
					}
				}
			}
			if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float num2 = this.GetStatValueForGear(pawn.equipment.Primary);
				if (this.subtract)
				{
					num2 = -num2;
				}
				if (!Mathf.Approximately(num2, 0f))
				{
					stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary, num2));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600CD44 RID: 52548 RVA: 0x004B7DFA File Offset: 0x004B5FFA
		private string InfoTextLineFrom(Thing gear, float value)
		{
			return "    " + gear.LabelCap + ": " + value.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x0600CD45 RID: 52549 RVA: 0x004B7E23 File Offset: 0x004B6023
		private float GetStatValueForGear(Thing gear)
		{
			return gear.GetStatValue(this.apparelStat, true, -1) + StatWorker.StatOffsetFromGear(gear, this.apparelStat);
		}

		// Token: 0x0600CD46 RID: 52550 RVA: 0x004B7E40 File Offset: 0x004B6040
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = pawn.apparel.WornApparel[i];
					if (apparel.GetStatValue(this.apparelStat, true, -1) != 0f)
					{
						return true;
					}
					if (StatWorker.StatOffsetFromGear(apparel, this.apparelStat) != 0f)
					{
						return true;
					}
				}
			}
			return this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat) != 0f;
		}

		// Token: 0x0600CD47 RID: 52551 RVA: 0x004B7EEB File Offset: 0x004B60EB
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			StatPart_GearStatOffset.<GetInfoCardHyperlinks>d__8 <GetInfoCardHyperlinks>d__ = new StatPart_GearStatOffset.<GetInfoCardHyperlinks>d__8(-2);
			<GetInfoCardHyperlinks>d__.<>4__this = this;
			<GetInfoCardHyperlinks>d__.<>3__req = req;
			return <GetInfoCardHyperlinks>d__;
		}

		// Token: 0x04008301 RID: 33537
		private StatDef apparelStat;

		// Token: 0x04008302 RID: 33538
		private bool subtract;

		// Token: 0x04008303 RID: 33539
		private bool includeWeapon;
	}
}
