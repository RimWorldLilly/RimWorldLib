using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A08 RID: 6664
	public class CompTargetable_SingleCorpse : CompTargetable
	{
		// Token: 0x17001D67 RID: 7527
		// (get) Token: 0x0600A7C7 RID: 42951 RVA: 0x003BB8D3 File Offset: 0x003B9AD3
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A7C8 RID: 42952 RVA: 0x003BB8D6 File Offset: 0x003B9AD6
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetBuildings = false,
				canTargetItems = false,
				canTargetCorpses = true,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x0600A7C9 RID: 42953 RVA: 0x003BB900 File Offset: 0x003B9B00
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			CompTargetable_SingleCorpse.<GetTargets>d__3 <GetTargets>d__ = new CompTargetable_SingleCorpse.<GetTargets>d__3(-2);
			<GetTargets>d__.<>3__targetChosenByPlayer = targetChosenByPlayer;
			return <GetTargets>d__;
		}

		// Token: 0x0600A7CA RID: 42954 RVA: 0x003BB910 File Offset: 0x003B9B10
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return target.Thing is Corpse && base.ValidateTarget(target.Thing, showMessages);
		}
	}
}
