using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161A RID: 5658
	public class LandingOutcomeWorker_MinorGravshipCrash : LandingOutcomeWorker
	{
		// Token: 0x06008D01 RID: 36097 RVA: 0x00334843 File Offset: 0x00332A43
		public LandingOutcomeWorker_MinorGravshipCrash(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x06008D02 RID: 36098 RVA: 0x0033484C File Offset: 0x00332A4C
		public override void ApplyOutcome(Gravship gravship)
		{
			ThingWithComps parent = (from comp in gravship.Engine.GravshipComponents
			where comp.parent.HasComp<CompGravshipThruster>()
			select comp).RandomElement<CompGravshipFacility>().parent;
			float randomInRange = LandingOutcomeWorker_MinorGravshipCrash.DamageRadiusRange.RandomInRange;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			foreach (IntVec3 c in GenRadial.RadialCellsAround(parent.Position, randomInRange, true))
			{
				if (c.InBounds(parent.Map))
				{
					int randomInRange2 = LandingOutcomeWorker_MinorGravshipCrash.DamageAmountRange.RandomInRange;
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Crush, (float)randomInRange2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					List<Thing> thingList = c.GetThingList(parent.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (!hashSet.Contains(thing) && thing is Building)
						{
							thing.TakeDamage(dinfo);
							hashSet.Add(thing);
						}
					}
				}
			}
			base.SendStandardLetter(gravship.Engine, null, parent);
		}

		// Token: 0x04005686 RID: 22150
		private static readonly FloatRange DamageRadiusRange = new FloatRange(5.9f, 11.9f);

		// Token: 0x04005687 RID: 22151
		private static readonly IntRange DamageAmountRange = new IntRange(25, 30);
	}
}
