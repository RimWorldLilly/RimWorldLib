using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009DD RID: 2525
	public class JobDriver_Blind : JobDriver
	{
		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x001D7564 File Offset: 0x001D5764
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x001D758C File Offset: 0x001D578C
		public static void Blind(Pawn pawn, Pawn doer)
		{
			Lord lord = pawn.GetLord();
			IEnumerable<BodyPartRecord> enumerable = from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Eye
			select p;
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null && enumerable.Count<BodyPartRecord>() == 1)
				{
					lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
				}
			}
			foreach (BodyPartRecord bodyPartRecord in enumerable)
			{
				if (bodyPartRecord.def == BodyPartDefOf.Eye)
				{
					pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					break;
				}
			}
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, doer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x001D7680 File Offset: 0x001D5880
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			if (PawnUtility.IsBiologicallyBlind(pawn))
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotBlinded, pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x001D76AA File Offset: 0x001D58AA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x001D76CD File Offset: 0x001D58CD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Blind.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_Blind.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003019 RID: 12313
		private const TargetIndex TargetPawnIndex = TargetIndex.A;
	}
}
