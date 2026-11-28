using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009CA RID: 2506
	public class JobDriver_ConstructFinishFrame : JobDriver, IBuildableDriver
	{
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x001D6730 File Offset: 0x001D4930
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x001D6758 File Offset: 0x001D4958
		private bool IsBuildingAttachment
		{
			get
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(this.Frame.def) as ThingDef;
				BuildingProperties buildingProperties = (thingDef != null) ? thingDef.building : null;
				return buildingProperties != null && buildingProperties.isAttachment;
			}
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x001D6792 File Offset: 0x001D4992
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x001D67B5 File Offset: 0x001D49B5
		public bool TryGetBuildableRect(out CellRect rect)
		{
			rect = this.Frame.OccupiedRect();
			return true;
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x001D67C9 File Offset: 0x001D49C9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ConstructFinishFrame.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_ConstructFinishFrame.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FFD RID: 12285
		private const int JobEndInterval = 5000;

		// Token: 0x04002FFE RID: 12286
		private const TargetIndex BuildingInd = TargetIndex.A;
	}
}
