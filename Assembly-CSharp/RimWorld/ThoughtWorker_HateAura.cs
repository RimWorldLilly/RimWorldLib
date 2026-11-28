using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6F RID: 3695
	public class ThoughtWorker_HateAura : ThoughtWorker
	{
		// Token: 0x060061FF RID: 25087 RVA: 0x002348B4 File Offset: 0x00232AB4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_HateAura.HateAuraLevel hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.None;
			if (p.Map != null)
			{
				List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.Mech_Apocriton);
				list.SortBy((Thing m) => m.Position.DistanceToSquared(m.Position));
				if (list.Count > 0)
				{
					float num = list[0].Position.DistanceTo(p.Position);
					if (num <= 6.9f)
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Intense;
					}
					else if (num <= 15.9f)
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Strong;
					}
					else
					{
						hateAuraLevel = ThoughtWorker_HateAura.HateAuraLevel.Distant;
					}
				}
			}
			switch (hateAuraLevel)
			{
			case ThoughtWorker_HateAura.HateAuraLevel.None:
				return false;
			case ThoughtWorker_HateAura.HateAuraLevel.Intense:
				return ThoughtState.ActiveAtStage(0);
			case ThoughtWorker_HateAura.HateAuraLevel.Strong:
				return ThoughtState.ActiveAtStage(1);
			case ThoughtWorker_HateAura.HateAuraLevel.Distant:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04003613 RID: 13843
		private const float IntenseDistance = 6.9f;

		// Token: 0x04003614 RID: 13844
		private const float StrongDistance = 15.9f;

		// Token: 0x02002EE6 RID: 12006
		private enum HateAuraLevel
		{
			// Token: 0x0400BE26 RID: 48678
			None,
			// Token: 0x0400BE27 RID: 48679
			Intense,
			// Token: 0x0400BE28 RID: 48680
			Strong,
			// Token: 0x0400BE29 RID: 48681
			Distant
		}
	}
}
