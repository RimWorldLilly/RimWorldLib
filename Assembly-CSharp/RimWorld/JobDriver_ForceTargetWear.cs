using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAD RID: 2733
	public class JobDriver_ForceTargetWear : JobDriver
	{
		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005252 RID: 21074 RVA: 0x001EACC0 File Offset: 0x001E8EC0
		private Pawn TargetPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005253 RID: 21075 RVA: 0x001EACE8 File Offset: 0x001E8EE8
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005254 RID: 21076 RVA: 0x001EAD10 File Offset: 0x001E8F10
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

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005255 RID: 21077 RVA: 0x001EAD4C File Offset: 0x001E8F4C
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x001EAD72 File Offset: 0x001E8F72
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001EADA0 File Offset: 0x001E8FA0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.job.count = 1;
			if (this.pawn == this.TargetPawn)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(78, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Pawn ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to do ForceTargetWear with self as target; this should not happen.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			if (!this.pawn.Reserve(this.TargetPawn, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x001EAE80 File Offset: 0x001E9080
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetC = (Thing)this.Apparel.ParentHolder;
			}
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
				}
			}
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x001EAF55 File Offset: 0x001E9155
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ForceTargetWear.<MakeNewToils>d__16 <MakeNewToils>d__ = new JobDriver_ForceTargetWear.<MakeNewToils>d__16(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001EAF68 File Offset: 0x001E9168
		private SoundDef GetCurrentWearSound()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
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

		// Token: 0x0600525B RID: 21083 RVA: 0x001EB014 File Offset: 0x001E9214
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.TargetPawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.TargetPawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.TargetPawn.Faction != null && this.TargetPawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.TargetPawn.apparel.TryDrop(wornApparel[i], out apparel2, this.TargetPawn.PositionHeld, forbid))
					{
						Pawn targetPawn = this.TargetPawn;
						Log.Error(((targetPawn != null) ? targetPawn.ToString() : null) + " could not drop " + wornApparel[i].ToStringSafe<Apparel>());
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

		// Token: 0x040031C9 RID: 12745
		private int duration;

		// Token: 0x040031CA RID: 12746
		private int unequipBuffer;

		// Token: 0x040031CB RID: 12747
		private const TargetIndex PawnInd = TargetIndex.A;

		// Token: 0x040031CC RID: 12748
		private const TargetIndex ApparelInd = TargetIndex.B;

		// Token: 0x040031CD RID: 12749
		private const TargetIndex ApparelSourceIndex = TargetIndex.C;
	}
}
