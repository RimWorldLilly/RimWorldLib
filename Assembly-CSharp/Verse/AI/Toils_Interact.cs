using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000842 RID: 2114
	public static class Toils_Interact
	{
		// Token: 0x06004288 RID: 17032 RVA: 0x001A3F38 File Offset: 0x001A2138
		public static Toil DestroyThing(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("DestroyThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (!thing.Destroyed)
				{
					if (thing.def.category == ThingCategory.Plant && thing.def.plant.IsTree && thing.def.plant.treeLoversCareIfChopped)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, actor.Named(HistoryEventArgsNames.Doer)), true);
					}
					thing.Destroy(DestroyMode.Vanish);
				}
			};
			return toil;
		}
	}
}
