using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A05 RID: 6661
	public class CompTargetable_AllPawnsOnTheMap : CompTargetable
	{
		// Token: 0x17001D64 RID: 7524
		// (get) Token: 0x0600A7BC RID: 42940 RVA: 0x003BB7E6 File Offset: 0x003B99E6
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600A7BD RID: 42941 RVA: 0x003BB7E9 File Offset: 0x003B99E9
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x0600A7BE RID: 42942 RVA: 0x003BB7FE File Offset: 0x003B99FE
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			CompTargetable_AllPawnsOnTheMap.<GetTargets>d__3 <GetTargets>d__ = new CompTargetable_AllPawnsOnTheMap.<GetTargets>d__3(-2);
			<GetTargets>d__.<>4__this = this;
			return <GetTargets>d__;
		}
	}
}
