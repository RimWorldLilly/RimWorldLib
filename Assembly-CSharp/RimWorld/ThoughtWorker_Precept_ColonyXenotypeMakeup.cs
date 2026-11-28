using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE2 RID: 3554
	public class ThoughtWorker_Precept_ColonyXenotypeMakeup : ThoughtWorker_Precept
	{
		// Token: 0x060060A4 RID: 24740 RVA: 0x0022FFB8 File Offset: 0x0022E1B8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.Faction == null)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Ideo.PreferredXenotypes.Any<XenotypeDef>() && !p.Ideo.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return ThoughtState.Inactive;
			}
			List<Pawn> list = p.MapHeld.mapPawns.SpawnedPawnsInFaction(p.Faction);
			int num = 0;
			int num2 = 0;
			bool flag = p.IsSlave || p.IsPrisoner;
			foreach (Pawn pawn in list)
			{
				bool flag2 = pawn.IsSlave || pawn.IsPrisoner;
				if (pawn.genes != null && flag == flag2)
				{
					num++;
					if (!p.Ideo.IsPreferredXenotype(pawn))
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			float num3 = (float)num2 / (float)num;
			if (num3 < 0.33f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num3 < 0.66f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
