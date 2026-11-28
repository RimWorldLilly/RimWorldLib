using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB0 RID: 2736
	public class JobDriver_Wear : JobDriver
	{
		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005272 RID: 21106 RVA: 0x001EB878 File Offset: 0x001E9A78
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x001EB8A0 File Offset: 0x001E9AA0
		private bool TargetIsOnApparelSource
		{
			get
			{
				Apparel apparel = this.Apparel;
				if (apparel != null && !apparel.Spawned)
				{
					IApparelSource apparelSource = apparel.ParentHolder as IApparelSource;
					if (apparelSource != null)
					{
						return apparelSource is Thing;
					}
				}
				return false;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06005274 RID: 21108 RVA: 0x001EB8DC File Offset: 0x001E9ADC
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001EB902 File Offset: 0x001E9B02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001EB930 File Offset: 0x001E9B30
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001EB994 File Offset: 0x001E9B94
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetB = (Thing)this.Apparel.ParentHolder;
			}
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
				}
			}
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001EBA69 File Offset: 0x001E9C69
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Wear.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_Wear.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x001EBA7C File Offset: 0x001E9C7C
		private SoundDef GetCurrentWearSound()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					if (this.unequipBuffer < (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f))
					{
						return wornApparel[i].def.apparel.soundRemove;
					}
					break;
				}
				else
				{
					i--;
				}
			}
			return apparel.def.apparel.soundWear;
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x001EBB28 File Offset: 0x001E9D28
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
					{
						Pawn pawn = this.pawn;
						Log.Error(((pawn != null) ? pawn.ToString() : null) + " could not drop " + wornApparel[i].ToStringSafe<Apparel>());
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x040031D3 RID: 12755
		private int duration;

		// Token: 0x040031D4 RID: 12756
		private int unequipBuffer;

		// Token: 0x040031D5 RID: 12757
		private const TargetIndex ApparelInd = TargetIndex.A;

		// Token: 0x040031D6 RID: 12758
		private const TargetIndex ApparelSourceIndex = TargetIndex.B;
	}
}
