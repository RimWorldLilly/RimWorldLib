using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F2C RID: 7980
	public static class MassUtility
	{
		// Token: 0x0600CB92 RID: 52114 RVA: 0x004B0444 File Offset: 0x004AE644
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x0600CB93 RID: 52115 RVA: 0x004B0451 File Offset: 0x004AE651
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x0600CB94 RID: 52116 RVA: 0x004B0461 File Offset: 0x004AE661
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x0600CB95 RID: 52117 RVA: 0x004B0470 File Offset: 0x004AE670
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true, -1);
		}

		// Token: 0x0600CB96 RID: 52118 RVA: 0x004B048A File Offset: 0x004AE68A
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true, -1));
		}

		// Token: 0x0600CB97 RID: 52119 RVA: 0x004B04A5 File Offset: 0x004AE6A5
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x0600CB98 RID: 52120 RVA: 0x004B04BF File Offset: 0x004AE6BF
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x0600CB99 RID: 52121 RVA: 0x004B04D0 File Offset: 0x004AE6D0
		public static float GearMass(Pawn p)
		{
			float num = 0f;
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					num += wornApparel[i].GetStatValue(StatDefOf.Mass, true, 1);
				}
			}
			if (p.equipment != null)
			{
				foreach (ThingWithComps thing in p.equipment.AllEquipmentListForReading)
				{
					num += thing.GetStatValue(StatDefOf.Mass, true, 1);
				}
			}
			return num;
		}

		// Token: 0x0600CB9A RID: 52122 RVA: 0x004B0580 File Offset: 0x004AE780
		public static float InventoryMass(Pawn p)
		{
			float num = 0f;
			for (int i = 0; i < p.inventory.innerContainer.Count; i++)
			{
				Thing thing = p.inventory.innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Mass, true, -1);
			}
			return num;
		}

		// Token: 0x0600CB9B RID: 52123 RVA: 0x004B05DC File Offset: 0x004AE7DC
		public static float Capacity(Pawn p, StringBuilder explanation = null)
		{
			if (!MassUtility.CanEverCarryAnything(p))
			{
				return 0f;
			}
			float num = p.BodySize * 35f;
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("  - " + p.LabelShortCap + ": " + num.ToStringMassOffset());
			}
			return num;
		}

		// Token: 0x0600CB9C RID: 52124 RVA: 0x004B063A File Offset: 0x004AE83A
		public static bool CanEverCarryAnything(Pawn p)
		{
			return (p.RaceProps.ToolUser && !p.DevelopmentalStage.Baby() && !p.IsSubhuman) || p.RaceProps.packAnimal;
		}

		// Token: 0x040081C2 RID: 33218
		public const float MassCapacityPerBodySize = 35f;
	}
}
