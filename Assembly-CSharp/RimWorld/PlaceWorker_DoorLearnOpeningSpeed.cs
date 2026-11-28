using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203F RID: 8255
	public class PlaceWorker_DoorLearnOpeningSpeed : PlaceWorker
	{
		// Token: 0x0600D0D6 RID: 53462 RVA: 0x004CCE6C File Offset: 0x004CB06C
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Blueprint_Door blueprint_Door = (Blueprint_Door)loc.GetThingList(map).FirstOrDefault((Thing t) => t is Blueprint_Door);
			if (blueprint_Door != null && blueprint_Door.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.DoorOpenSpeed, blueprint_Door.stuffToUse) < 0.65f)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.DoorOpenSpeed, OpportunityType.Important);
			}
		}
	}
}
