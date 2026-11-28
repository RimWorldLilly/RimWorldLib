using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001593 RID: 5523
	public static class ApparelUtility
	{
		// Token: 0x06008797 RID: 34711 RVA: 0x00310368 File Offset: 0x0030E568
		public static bool IsRequirementActive(ApparelRequirement requirement, ApparelRequirementSource source, Pawn pawn, out string disabledByLabel)
		{
			bool flag = false;
			if (pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Nudist))
			{
				flag = true;
			}
			else if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersNudity())
			{
				flag = true;
			}
			if (flag)
			{
				disabledByLabel = "Nudism".Translate();
				return false;
			}
			if (source != ApparelRequirementSource.Title && pawn.royalty != null && !pawn.royalty.AllTitlesForReading.NullOrEmpty<RoyalTitle>())
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
					{
						disabledByLabel = royalTitle.def.GetLabelCapFor(pawn);
						return false;
					}
				}
			}
			disabledByLabel = null;
			return true;
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x00310460 File Offset: 0x0030E660
		public static Apparel GetApparelReplacedByNewApparel(Pawn pawn, Apparel newApparel)
		{
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, pawn.apparel.WornApparel[i].def, pawn.RaceProps.body))
				{
					return pawn.apparel.WornApparel[i];
				}
			}
			return null;
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x003104CC File Offset: 0x0030E6CC
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
			List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
			BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
			BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
			for (int k = 0; k < bodyPartGroups.Count; k++)
			{
				if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
				{
					return false;
				}
			}
			for (int l = 0; l < bodyPartGroups2.Count; l++)
			{
				if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600879A RID: 34714 RVA: 0x003105D0 File Offset: 0x0030E7D0
		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		// Token: 0x0600879B RID: 34715 RVA: 0x00310634 File Offset: 0x0030E834
		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			List<BodyPartGroupDef> groups = apparel.apparel.bodyPartGroups;
			int i;
			Func<BodyPartRecord, bool> <>9__0;
			int j;
			for (i = 0; i < groups.Count; i = j + 1)
			{
				IEnumerable<BodyPartRecord> source = notMissingParts;
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.IsInGroup(groups[i])));
				}
				if (source.Any(predicate))
				{
					return true;
				}
				j = i;
			}
			return false;
		}

		// Token: 0x02003398 RID: 13208
		public struct LayerGroupPair : IEquatable<ApparelUtility.LayerGroupPair>
		{
			// Token: 0x060130B8 RID: 78008 RVA: 0x0064EFF3 File Offset: 0x0064D1F3
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x060130B9 RID: 78009 RVA: 0x0064F003 File Offset: 0x0064D203
			public override bool Equals(object rhs)
			{
				return rhs is ApparelUtility.LayerGroupPair && this.Equals((ApparelUtility.LayerGroupPair)rhs);
			}

			// Token: 0x060130BA RID: 78010 RVA: 0x0064F01B File Offset: 0x0064D21B
			public bool Equals(ApparelUtility.LayerGroupPair other)
			{
				return other.layer == this.layer && other.group == this.group;
			}

			// Token: 0x060130BB RID: 78011 RVA: 0x0064F03B File Offset: 0x0064D23B
			public override int GetHashCode()
			{
				return (17 * 23 + this.layer.GetHashCode()) * 23 + this.group.GetHashCode();
			}

			// Token: 0x0400CF22 RID: 53026
			private readonly ApparelLayerDef layer;

			// Token: 0x0400CF23 RID: 53027
			private readonly BodyPartGroupDef group;
		}
	}
}
