using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A09 RID: 6665
	public class CompTargetable_SinglePawn : CompTargetable
	{
		// Token: 0x17001D68 RID: 7528
		// (get) Token: 0x0600A7CC RID: 42956 RVA: 0x003BB93D File Offset: 0x003B9B3D
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A7CD RID: 42957 RVA: 0x003BB940 File Offset: 0x003B9B40
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x0600A7CE RID: 42958 RVA: 0x003BB955 File Offset: 0x003B9B55
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			CompTargetable_SinglePawn.<GetTargets>d__3 <GetTargets>d__ = new CompTargetable_SinglePawn.<GetTargets>d__3(-2);
			<GetTargets>d__.<>3__targetChosenByPlayer = targetChosenByPlayer;
			return <GetTargets>d__;
		}
	}
}
