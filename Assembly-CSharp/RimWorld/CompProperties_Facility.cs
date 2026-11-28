using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED6 RID: 3798
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x06006396 RID: 25494 RVA: 0x0023BD40 File Offset: 0x00239F40
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x0023BD78 File Offset: 0x00239F78
		public override void ResolveReferences(ThingDef parentDef)
		{
			this.linkableBuildings = new List<ThingDef>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
				if (compProperties != null && compProperties.linkableFacilities != null)
				{
					for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
					{
						if (compProperties.linkableFacilities[j] == parentDef)
						{
							this.linkableBuildings.Add(allDefsListForReading[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x0023BDF8 File Offset: 0x00239FF8
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_Facility.<SpecialDisplayStats>d__14 <SpecialDisplayStats>d__ = new CompProperties_Facility.<SpecialDisplayStats>d__14(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040037DE RID: 14302
		[Unsaved(false)]
		public List<ThingDef> linkableBuildings;

		// Token: 0x040037DF RID: 14303
		public List<StatModifier> statOffsets;

		// Token: 0x040037E0 RID: 14304
		public int maxSimultaneous = 1;

		// Token: 0x040037E1 RID: 14305
		public bool mustBePlacedAdjacent;

		// Token: 0x040037E2 RID: 14306
		public bool mustBePlacedAdjacentCardinalToBedHead;

		// Token: 0x040037E3 RID: 14307
		public bool mustBePlacedAdjacentCardinalToAndFacingBedHead;

		// Token: 0x040037E4 RID: 14308
		public bool mustBePlacedFacingThingLinear;

		// Token: 0x040037E5 RID: 14309
		public bool canLinkToMedBedsOnly;

		// Token: 0x040037E6 RID: 14310
		public float maxDistance = 8f;

		// Token: 0x040037E7 RID: 14311
		public float minDistance;

		// Token: 0x040037E8 RID: 14312
		public bool showMaxSimultaneous = true;

		// Token: 0x040037E9 RID: 14313
		public bool requiresLOS = true;
	}
}
