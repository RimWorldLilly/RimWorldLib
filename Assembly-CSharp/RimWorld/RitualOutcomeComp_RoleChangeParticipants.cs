using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D8 RID: 5848
	public class RitualOutcomeComp_RoleChangeParticipants : RitualOutcomeComp_ParticipantCount
	{
		// Token: 0x0600924C RID: 37452 RVA: 0x00356D1B File Offset: 0x00354F1B
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataRoleChangeParticipants();
		}

		// Token: 0x0600924D RID: 37453 RVA: 0x00356D24 File Offset: 0x00354F24
		public override void Notify_AssignmentsChanged(RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = 0;
			Pawn pawn = assignments.FirstAssignedPawn("role_changer");
			if (pawn == null)
			{
				return;
			}
			foreach (Pawn pawn2 in pawn.Map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn2 != pawn && pawn2.Ideo == pawn.Ideo && !pawn2.Downed)
				{
					num++;
				}
			}
			((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount = num / 2;
		}

		// Token: 0x0600924E RID: 37454 RVA: 0x00356DB8 File Offset: 0x00354FB8
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return quality.ToStringPercent();
		}

		// Token: 0x0600924F RID: 37455 RVA: 0x00356DC0 File Offset: 0x00354FC0
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			return (int)this.Count(ritual, data) >= desiredParticipantCount;
		}

		// Token: 0x06009250 RID: 37456 RVA: 0x00356DEC File Offset: 0x00354FEC
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				Pawn p = (Pawn)keyValuePair.Key;
				if (base.Counts(ritual.assignments, p) && keyValuePair.Value >= (float)ritual.DurationTicks / 2f)
				{
					num++;
				}
			}
			return (float)num;
		}

		// Token: 0x06009251 RID: 37457 RVA: 0x00356E78 File Offset: 0x00355078
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			return string.Concat(new string[]
			{
				this.Count(ritual, data).ToString(),
				" / ",
				desiredParticipantCount.ToString(),
				" ",
				this.label,
				": +",
				this.QualityOffset(ritual, data).ToStringPercent()
			});
		}

		// Token: 0x06009252 RID: 37458 RVA: 0x00356EEC File Offset: 0x003550EC
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			bool flag = num >= desiredParticipantCount;
			int num2 = (flag > false) ? 1 : 0;
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = num.ToString() + " / " + desiredParticipantCount.ToString(),
				qualityChange = this.ExpectedOffsetDesc(flag, (float)num2),
				quality = (float)num2,
				positive = flag,
				priority = 4f
			};
		}
	}
}
