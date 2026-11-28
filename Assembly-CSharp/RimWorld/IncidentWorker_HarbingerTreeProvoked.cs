using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117D RID: 4477
	public class IncidentWorker_HarbingerTreeProvoked : IncidentWorker
	{
		// Token: 0x06007052 RID: 28754 RVA: 0x002752D0 File Offset: 0x002734D0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			IntVec3 intVec;
			return IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(parms.target as Map, out intVec, null) && base.CanFireNowSub(parms);
		}

		// Token: 0x06007053 RID: 28755 RVA: 0x00275304 File Offset: 0x00273504
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 loc;
			if (!IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(map, out loc, null))
			{
				return false;
			}
			Plant plant = (Plant)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null);
			plant.Growth = 0.15f;
			GenSpawn.Spawn(plant, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, plant, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
