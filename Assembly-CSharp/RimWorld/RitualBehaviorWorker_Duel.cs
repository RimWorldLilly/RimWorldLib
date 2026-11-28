using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001682 RID: 5762
	public class RitualBehaviorWorker_Duel : RitualBehaviorWorker
	{
		// Token: 0x0600907D RID: 36989 RVA: 0x00350916 File Offset: 0x0034EB16
		public RitualBehaviorWorker_Duel()
		{
		}

		// Token: 0x0600907E RID: 36990 RVA: 0x0035091E File Offset: 0x0034EB1E
		public RitualBehaviorWorker_Duel(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x0600907F RID: 36991 RVA: 0x00350927 File Offset: 0x0034EB27
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x06009080 RID: 36992 RVA: 0x00350930 File Offset: 0x0034EB30
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.target = target;
			string text = base.CanStartRitualNow(CS$<>8__locals1.target, ritual, selectedPawn, forcedForRole);
			if (text != null)
			{
				return text;
			}
			CS$<>8__locals1.room = CS$<>8__locals1.target.Cell.GetRoom(CS$<>8__locals1.target.Map);
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeLeader, RitualBehaviorWorker_Duel.RadiusRangeLeader), ref CS$<>8__locals1) < 1)
			{
				return "CantStartNotEnoughSpaceDuelSpeaker".Translate(RitualBehaviorWorker_Duel.RadiusRangeLeader.Named("MINRADIUS"));
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(new IntRange(RitualBehaviorWorker_Duel.RadiusRangeDuelists, RitualBehaviorWorker_Duel.RadiusRangeDuelists), ref CS$<>8__locals1) < 2)
			{
				return "CantStartNotEnoughSpaceDuelDuelists".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"), 2);
			}
			if (RitualBehaviorWorker_Duel.<CanStartRitualNow>g__StandableCellsInRange|8_0(RitualBehaviorWorker_Duel.RadiusRangeSpectators, ref CS$<>8__locals1) < 20)
			{
				return "CantStartNotEnoughSpaceDuelSpectators".Translate(RitualBehaviorWorker_Duel.RadiusRangeSpectators.Average.Named("MINRADIUS"), 20);
			}
			return null;
		}

		// Token: 0x06009081 RID: 36993 RVA: 0x00350A4B File Offset: 0x0034EC4B
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Duel(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x06009082 RID: 36994 RVA: 0x00350A64 File Offset: 0x0034EC64
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 4)
			{
				if (this.soundPlaying == null || this.soundPlaying.Ended)
				{
					TargetInfo selectedTarget = ritual.selectedTarget;
					this.soundPlaying = SoundDefOf.DuelMusic.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
				}
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x06009083 RID: 36995 RVA: 0x00350AD7 File Offset: 0x0034ECD7
		public override void Cleanup(LordJob_Ritual ritual)
		{
			this.soundPlaying = null;
		}

		// Token: 0x06009084 RID: 36996 RVA: 0x00350AE0 File Offset: 0x0034ECE0
		public override void DrawPreviewOnTarget(TargetInfo targetInfo)
		{
			base.DrawPreviewOnTarget(targetInfo);
			GenDraw.DrawRadiusRing(targetInfo.CenterCell, 5.9f);
		}

		// Token: 0x06009086 RID: 36998 RVA: 0x00350B14 File Offset: 0x0034ED14
		[CompilerGenerated]
		internal static int <CanStartRitualNow>g__StandableCellsInRange|8_0(IntRange range, ref RitualBehaviorWorker_Duel.<>c__DisplayClass8_0 A_1)
		{
			int num = 0;
			foreach (IntVec3 intVec in CellRect.CenteredOn(A_1.target.Cell, range.max))
			{
				float num2 = intVec.DistanceTo(A_1.target.Cell);
				if (num2 >= (float)range.min && num2 <= (float)range.max && intVec.Standable(A_1.target.Map) && intVec.GetRoom(A_1.target.Map) == A_1.room)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04005830 RID: 22576
		private Sustainer soundPlaying;

		// Token: 0x04005831 RID: 22577
		public static readonly IntRange RadiusRangeSpectators = new IntRange(5, 7);

		// Token: 0x04005832 RID: 22578
		public static readonly int RadiusRangeLeader = 3;

		// Token: 0x04005833 RID: 22579
		public static readonly int RadiusRangeDuelists = 2;
	}
}
