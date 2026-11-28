using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A07 RID: 6663
	public class CompTargetable_SingleAnimal : CompTargetable
	{
		// Token: 0x17001D65 RID: 7525
		// (get) Token: 0x0600A7C1 RID: 42945 RVA: 0x003BB835 File Offset: 0x003B9A35
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D66 RID: 7526
		// (get) Token: 0x0600A7C2 RID: 42946 RVA: 0x003BB838 File Offset: 0x003B9A38
		public new CompProperties_Targetable_SingleAnimal Props
		{
			get
			{
				return (CompProperties_Targetable_SingleAnimal)this.props;
			}
		}

		// Token: 0x0600A7C3 RID: 42947 RVA: 0x003BB845 File Offset: 0x003B9A45
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				canTargetAnimals = true,
				canTargetHumans = false,
				canTargetMechs = false,
				validator = delegate(TargetInfo target)
				{
					if (!this.Props.allowWildMan)
					{
						Pawn pawn = target.Thing as Pawn;
						return pawn != null && !pawn.IsWildMan();
					}
					return true;
				}
			};
		}

		// Token: 0x0600A7C4 RID: 42948 RVA: 0x003BB881 File Offset: 0x003B9A81
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			CompTargetable_SingleAnimal.<GetTargets>d__5 <GetTargets>d__ = new CompTargetable_SingleAnimal.<GetTargets>d__5(-2);
			<GetTargets>d__.<>3__targetChosenByPlayer = targetChosenByPlayer;
			return <GetTargets>d__;
		}
	}
}
