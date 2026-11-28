using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001585 RID: 5509
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x06008702 RID: 34562 RVA: 0x0030CBF3 File Offset: 0x0030ADF3
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x06008703 RID: 34563 RVA: 0x0030CBFB File Offset: 0x0030ADFB
		public RoomSizeCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RoomSizeCategory.VeryCramped;
				}
				if (this.CurLevel < 0.3f)
				{
					return RoomSizeCategory.Cramped;
				}
				if (this.CurLevel < 0.7f)
				{
					return RoomSizeCategory.Normal;
				}
				return RoomSizeCategory.Spacious;
			}
		}

		// Token: 0x06008704 RID: 34564 RVA: 0x0030CC2B File Offset: 0x0030AE2B
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x06008705 RID: 34565 RVA: 0x0030CC60 File Offset: 0x0030AE60
		public float SpacePerceptibleNow()
		{
			if (!this.pawn.Spawned)
			{
				return 1f;
			}
			IntVec3 position = this.pawn.Position;
			Need_RoomSize.tempScanRooms.Clear();
			for (int i = 0; i < 5; i++)
			{
				Room room = (position + GenRadial.RadialPattern[i]).GetRoom(this.pawn.Map);
				if (room != null)
				{
					if (i == 0 && room.PsychologicallyOutdoors)
					{
						return 1f;
					}
					if ((i == 0 || (!room.IsDoorway && !room.PsychologicallyOutdoors)) && !Need_RoomSize.tempScanRooms.Contains(room))
					{
						Need_RoomSize.tempScanRooms.Add(room);
					}
				}
			}
			float num = 0f;
			for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
			{
				IntVec3 loc = position + GenRadial.RadialPattern[j];
				if (Need_RoomSize.tempScanRooms.Contains(loc.GetRoom(this.pawn.Map)))
				{
					num += 1f;
				}
			}
			Need_RoomSize.tempScanRooms.Clear();
			return Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
		}

		// Token: 0x0400537A RID: 21370
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x0400537B RID: 21371
		private const float MinCramped = 0.01f;

		// Token: 0x0400537C RID: 21372
		private const float MinNormal = 0.3f;

		// Token: 0x0400537D RID: 21373
		private const float MinSpacious = 0.7f;

		// Token: 0x0400537E RID: 21374
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x0400537F RID: 21375
		private static readonly SimpleCurve RoomCellCountSpaceCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(9f, 0.25f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			},
			{
				new CurvePoint(42f, 0.71f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
