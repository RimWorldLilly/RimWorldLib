using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006E1 RID: 1761
	public static class GenClamor
	{
		// Token: 0x060037CA RID: 14282 RVA: 0x00164E10 File Offset: 0x00163010
		public static void DoClamor(Thing source, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, source.Position, radius, type);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x00164E20 File Offset: 0x00163020
		public static void DoClamor(Thing source, IntVec3 position, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, position, radius, delegate(Thing _, Pawn hearer)
			{
				hearer.HearClamor(source, type);
			});
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x00164E5A File Offset: 0x0016305A
		public static void DoClamor(Thing source, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			GenClamor.DoClamor(source, source.Position, radius, clamorEffect);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x00164E6C File Offset: 0x0016306C
		public static void DoClamor(Thing source, IntVec3 position, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			if (source.MapHeld == null)
			{
				return;
			}
			Region region = position.GetRegion(source.MapHeld, RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null || r.door.Open, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
					if (num > 0f && pawn.Position.InHorDistOf(position, radius * num))
					{
						clamorEffect(source, pawn);
					}
				}
				return false;
			}, 15, RegionType.Set_Passable);
		}

		// Token: 0x02002A45 RID: 10821
		// (Invoke) Token: 0x060105D8 RID: 67032
		public delegate void ClamorEffect(Thing source, Pawn hearer);
	}
}
