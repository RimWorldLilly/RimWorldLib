using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C3F RID: 3135
	public abstract class WorkGiver_StudyBase : WorkGiver_Scanner
	{
		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06005933 RID: 22835 RVA: 0x0020E1D1 File Offset: 0x0020C3D1
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x0020E1D4 File Offset: 0x0020C3D4
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return Find.StudyManager.GetStudiableThingsAndPlatforms(pawn.Map);
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x0020E1E8 File Offset: 0x0020C3E8
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			Thing thing = t.Thing;
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
				}
			}
			if (thing == null)
			{
				return 0f;
			}
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			return (float)(Find.TickManager.TicksGame - compStudiable.lastStudiedTick);
		}
	}
}
