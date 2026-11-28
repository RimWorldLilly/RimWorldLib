using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001254 RID: 4692
	public class RoomContents_LargeEmptyGrayBox : RoomContents_LargeGrayBox
	{
		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06007454 RID: 29780 RVA: 0x0028B2C4 File Offset: 0x002894C4
		protected override int MaxCrates
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x0028B2C7 File Offset: 0x002894C7
		protected override void SpawnBox(IntVec3 cell, Map map)
		{
			RoomContents_GrayBox.SpawnBoxInRoom(cell, map, null, false);
		}
	}
}
