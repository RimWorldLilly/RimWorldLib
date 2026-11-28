using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161B RID: 5659
	public class LandingOutcomeWorker_ThrusterBreakdown : LandingOutcomeWorker
	{
		// Token: 0x06008D04 RID: 36100 RVA: 0x003349C8 File Offset: 0x00332BC8
		public LandingOutcomeWorker_ThrusterBreakdown(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x06008D05 RID: 36101 RVA: 0x003349D4 File Offset: 0x00332BD4
		public override void ApplyOutcome(Gravship gravship)
		{
			List<ThingWithComps> list = (from comp in gravship.Engine.GravshipComponents
			where comp.parent.HasComp<CompGravshipThruster>()
			select comp.parent).ToList<ThingWithComps>();
			list = list.InRandomOrder(null).Take(Rand.Range(Mathf.Min(2, list.Count), Mathf.Min(4, list.Count))).ToList<ThingWithComps>();
			using (List<ThingWithComps>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CompBreakdownable compBreakdownable;
					if (enumerator.Current.TryGetComp(out compBreakdownable))
					{
						compBreakdownable.DoBreakdown();
					}
				}
			}
			TaggedString taggedString = "ThrustersBrokeDown".Translate() + ":\n" + (from comp in list
			select comp.LabelCap).ToLineList(" - ", false);
			base.SendStandardLetter(gravship.Engine, taggedString, new LookTargets(list));
		}
	}
}
