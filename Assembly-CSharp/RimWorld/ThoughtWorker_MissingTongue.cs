using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E64 RID: 3684
	public class ThoughtWorker_MissingTongue : ThoughtWorker
	{
		// Token: 0x060061E5 RID: 25061 RVA: 0x002342A0 File Offset: 0x002324A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			List<BodyPartRecord> partsWithTag = p.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.Tongue);
			if (!partsWithTag.Any<BodyPartRecord>())
			{
				return ThoughtState.Inactive;
			}
			foreach (BodyPartRecord part in partsWithTag)
			{
				if (!p.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part) && p.health.hediffSet.PartIsMissing(part))
				{
					return true;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
