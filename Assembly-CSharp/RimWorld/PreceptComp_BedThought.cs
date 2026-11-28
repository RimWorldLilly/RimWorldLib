using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001633 RID: 5683
	public class PreceptComp_BedThought : PreceptComp_Thought
	{
		// Token: 0x06008E7E RID: 36478 RVA: 0x003461EC File Offset: 0x003443EC
		public override void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
			base.Notify_AddBedThoughts(pawn, precept);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.thought);
			if (this.requireFacility != null && !this.HasActiveLinkedFacility(pawn))
			{
				return;
			}
			Thought_Memory newThought = ThoughtMaker.MakeThought(this.thought, precept);
			pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06008E7F RID: 36479 RVA: 0x0034625C File Offset: 0x0034445C
		private bool HasActiveLinkedFacility(Pawn pawn)
		{
			Building_Bed building_Bed = pawn.CurrentBed();
			CompAffectedByFacilities compAffectedByFacilities = (building_Bed != null) ? building_Bed.TryGetComp<CompAffectedByFacilities>() : null;
			if (compAffectedByFacilities == null)
			{
				return false;
			}
			foreach (Thing thing in compAffectedByFacilities.LinkedFacilitiesListForReading)
			{
				if (thing.def == this.requireFacility.def && compAffectedByFacilities.IsFacilityActive(thing))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008E80 RID: 36480 RVA: 0x003462E4 File Offset: 0x003444E4
		public override IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			PreceptComp_BedThought.<ConfigErrors>d__4 <ConfigErrors>d__ = new PreceptComp_BedThought.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parent = parent;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005755 RID: 22357
		public PreceptComp_BedThought.FacilityData requireFacility;

		// Token: 0x02003481 RID: 13441
		public class FacilityData
		{
			// Token: 0x0400D233 RID: 53811
			public ThingDef def;
		}
	}
}
