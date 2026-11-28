using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D2 RID: 6354
	public class CompProperties_LabyrinthDoor : CompProperties_Interactable
	{
		// Token: 0x06009F53 RID: 40787 RVA: 0x003920E4 File Offset: 0x003902E4
		public CompProperties_LabyrinthDoor()
		{
			this.compClass = typeof(CompLabyrinthDoor);
		}

		// Token: 0x04005E43 RID: 24131
		public GraphicData jammed;

		// Token: 0x04005E44 RID: 24132
		public float unlockedChance = 0.3f;
	}
}
