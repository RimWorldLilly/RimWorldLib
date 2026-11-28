using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001689 RID: 5769
	public class RitualBehaviorWorker_Funeral : RitualBehaviorWorker
	{
		// Token: 0x060090A4 RID: 37028 RVA: 0x003512A5 File Offset: 0x0034F4A5
		public RitualBehaviorWorker_Funeral()
		{
		}

		// Token: 0x060090A5 RID: 37029 RVA: 0x003512AD File Offset: 0x0034F4AD
		public RitualBehaviorWorker_Funeral(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x060090A6 RID: 37030 RVA: 0x003512B8 File Offset: 0x0034F4B8
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null && building_Grave.Corpse != null && building_Grave.Corpse.InnerPawn.IsSlave)
				{
					return "CantStartFuneralForSlave".Translate(building_Grave.Corpse.InnerPawn);
				}
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
