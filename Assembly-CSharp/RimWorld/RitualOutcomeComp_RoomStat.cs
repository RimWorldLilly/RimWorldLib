using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C2 RID: 5826
	public class RitualOutcomeComp_RoomStat : RitualOutcomeComp_Quality
	{
		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x060091D4 RID: 37332 RVA: 0x00354A41 File Offset: 0x00352C41
		public override bool DataRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060091D5 RID: 37333 RVA: 0x00354A44 File Offset: 0x00352C44
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataRoomStatCached();
		}

		// Token: 0x060091D6 RID: 37334 RVA: 0x00354A4C File Offset: 0x00352C4C
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
			if (ritualOutcomeComp_DataRoomStatCached != null && ritualOutcomeComp_DataRoomStatCached.startingVal == null)
			{
				Room room = ritual.Spot.GetRoom(ritual.Map);
				if (room != null)
				{
					ritualOutcomeComp_DataRoomStatCached.startingVal = new float?(this.PostProcessedRoomStat(room));
				}
			}
		}

		// Token: 0x060091D7 RID: 37335 RVA: 0x00354AA0 File Offset: 0x00352CA0
		private float PostProcessedRoomStat(Room room)
		{
			return GenMath.RoundTo(Math.Min(room.GetStat(this.statDef), this.curve.Points[this.curve.PointsCount - 1].x), 0.1f);
		}

		// Token: 0x060091D8 RID: 37336 RVA: 0x00354AED File Offset: 0x00352CED
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.Spot.GetRoom(ritual.Map) != null;
		}

		// Token: 0x060091D9 RID: 37337 RVA: 0x00354B04 File Offset: 0x00352D04
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
			if (ritualOutcomeComp_DataRoomStatCached != null && ritualOutcomeComp_DataRoomStatCached.startingVal != null)
			{
				return ritualOutcomeComp_DataRoomStatCached.startingVal.Value;
			}
			Room room = ritual.Spot.GetRoom(ritual.Map);
			if (room != null)
			{
				return this.PostProcessedRoomStat(room);
			}
			return 0f;
		}

		// Token: 0x060091DA RID: 37338 RVA: 0x00354B58 File Offset: 0x00352D58
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float x = 0f;
			if (ritualTarget.Map != null)
			{
				Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
				RitualOutcomeComp_DataRoomStatCached ritualOutcomeComp_DataRoomStatCached = (RitualOutcomeComp_DataRoomStatCached)data;
				x = ((room == null) ? 0f : (((ritualOutcomeComp_DataRoomStatCached != null) ? ritualOutcomeComp_DataRoomStatCached.startingVal : null) ?? this.PostProcessedRoomStat(room)));
				if (room != null)
				{
					bool flag = !room.PsychologicallyOutdoors;
				}
			}
			float num = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = x.ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, num),
				quality = num,
				positive = (num > 0f),
				priority = 0f
			};
		}

		// Token: 0x0400587F RID: 22655
		public RoomStatDef statDef;
	}
}
