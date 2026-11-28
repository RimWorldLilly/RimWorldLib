using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163E RID: 5694
	public class PreceptComp_UnwillingToDo_WithDef : PreceptComp_UnwillingToDo
	{
		// Token: 0x06008EA3 RID: 36515 RVA: 0x00346CD8 File Offset: 0x00344ED8
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			if (this.eventDef != null && ev.def != this.eventDef)
			{
				return true;
			}
			Pawn pawn;
			ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn);
			ThingDef thingDef;
			ev.args.TryGetArg<ThingDef>(HistoryEventArgsNames.Building, out thingDef);
			if (thingDef == null)
			{
				return true;
			}
			if (this.nullifyingTraits != null && pawn != null && pawn.story != null)
			{
				if (!this.preceptDef.enabledForNPCFactions && !pawn.Faction.IsPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.nullifyingTraits.Count; i++)
				{
					if (this.nullifyingTraits[i].HasTrait(pawn))
					{
						return true;
					}
				}
			}
			return thingDef != this.buildingDef;
		}

		// Token: 0x06008EA4 RID: 36516 RVA: 0x00346D91 File Offset: 0x00344F91
		public override IEnumerable<string> GetDescriptions()
		{
			PreceptComp_UnwillingToDo_WithDef.<GetDescriptions>d__2 <GetDescriptions>d__ = new PreceptComp_UnwillingToDo_WithDef.<GetDescriptions>d__2(-2);
			<GetDescriptions>d__.<>4__this = this;
			return <GetDescriptions>d__;
		}

		// Token: 0x0400576B RID: 22379
		public ThingDef buildingDef;
	}
}
