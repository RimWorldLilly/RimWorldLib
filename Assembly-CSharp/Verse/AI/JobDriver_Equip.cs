using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000818 RID: 2072
	public class JobDriver_Equip : JobDriver
	{
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x0019E1E8 File Offset: 0x0019C3E8
		private Thing Target
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600412C RID: 16684 RVA: 0x0019E20C File Offset: 0x0019C40C
		private bool TargetIsOnOutfitStand
		{
			get
			{
				Thing target = this.Target;
				return target != null && !target.Spawned && target.ParentHolder is Building_OutfitStand;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x0019E23C File Offset: 0x0019C43C
		private Building_OutfitStand OutfitStand
		{
			get
			{
				return (Building_OutfitStand)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x0019E262 File Offset: 0x0019C462
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnOutfitStand)
			{
				this.job.targetB = (Building_OutfitStand)this.Target.ParentHolder;
			}
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0019E294 File Offset: 0x0019C494
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (EquipmentUtility.AlreadyBondedToWeapon(this.Target, this.pawn))
			{
				return false;
			}
			int maxPawns = 1;
			int stackCount = -1;
			bool targetIsOnOutfitStand = this.TargetIsOnOutfitStand;
			if (this.job.targetA.HasThing && this.job.targetA.Thing.Spawned && this.job.targetA.Thing.def.IsIngestible)
			{
				maxPawns = 10;
				stackCount = 1;
			}
			if (this.TargetIsOnOutfitStand)
			{
				return this.pawn.Reserve((Building_OutfitStand)this.job.targetA.Thing.ParentHolder, this.job, maxPawns, stackCount, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.job.targetA, this.job, maxPawns, stackCount, null, errorOnFailed, false);
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0019E36A File Offset: 0x0019C56A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Equip.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_Equip.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002B1F RID: 11039
		private const TargetIndex EquippableIndex = TargetIndex.A;

		// Token: 0x04002B20 RID: 11040
		private const TargetIndex OutfitStandIndex = TargetIndex.B;
	}
}
