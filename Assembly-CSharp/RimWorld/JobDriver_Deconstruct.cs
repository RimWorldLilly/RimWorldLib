using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009CB RID: 2507
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x001D67E1 File Offset: 0x001D49E1
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x001D67E8 File Offset: 0x001D49E8
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x001D67EB File Offset: 0x001D49EB
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true, -1), 20f, 3000f);
			}
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x001D680E File Offset: 0x001D4A0E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Deconstruct.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Deconstruct.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001D681E File Offset: 0x001D4A1E
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001D6844 File Offset: 0x001D4A44
		protected override void TickActionInterval(int delta)
		{
			if (this.pawn.skills != null && base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta, false, false);
			}
		}

		// Token: 0x04002FFF RID: 12287
		private const float MaxDeconstructWork = 3000f;

		// Token: 0x04003000 RID: 12288
		private const float MinDeconstructWork = 20f;
	}
}
