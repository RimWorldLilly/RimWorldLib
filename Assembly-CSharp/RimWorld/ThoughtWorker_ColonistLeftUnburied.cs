using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E32 RID: 3634
	public class ThoughtWorker_ColonistLeftUnburied : ThoughtWorker
	{
		// Token: 0x06006158 RID: 24920 RVA: 0x002328D0 File Offset: 0x00230AD0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if ((float)corpse.Age > 90000f && Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
				{
					return true;
				}
			}
			return false;
		}
	}
}
