using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	public class JobDriver_Replant : JobDriver_HaulToContainer
	{
		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x001D6D69 File Offset: 0x001D4F69
		protected override int Duration
		{
			get
			{
				return (int)(((MinifiedTree)base.ThingToCarry).InnerTree.def.plant.harvestWork / JobDriver_PlantWork.WorkDonePerTick(this.pawn, ((MinifiedTree)base.ThingToCarry).InnerTree));
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004C5A RID: 19546 RVA: 0x001D6DA7 File Offset: 0x001D4FA7
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return EffecterDefOf.Sow;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x001D6DAE File Offset: 0x001D4FAE
		protected override SoundDef WorkSustainer
		{
			get
			{
				return SoundDefOf.Interact_ConstructDirt;
			}
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x001D6DB5 File Offset: 0x001D4FB5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Replant.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_Replant.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x001D6DC5 File Offset: 0x001D4FC5
		protected override void ModifyPrepareToil(Toil toil)
		{
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
			}));
		}
	}
}
