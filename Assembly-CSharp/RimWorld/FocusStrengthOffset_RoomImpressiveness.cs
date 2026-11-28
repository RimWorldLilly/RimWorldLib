using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FC RID: 6396
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		// Token: 0x0600A05E RID: 41054 RVA: 0x003966A4 File Offset: 0x003948A4
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600A05F RID: 41055 RVA: 0x003966C2 File Offset: 0x003948C2
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
