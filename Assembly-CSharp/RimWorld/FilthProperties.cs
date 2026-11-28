using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF6 RID: 3830
	public class FilthProperties
	{
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x060063D5 RID: 25557 RVA: 0x0023C75E File Offset: 0x0023A95E
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		// Token: 0x04003859 RID: 14425
		public float cleaningWorkToReduceThickness = 35f;

		// Token: 0x0400385A RID: 14426
		public bool canFilthAttach;

		// Token: 0x0400385B RID: 14427
		public bool rainWashes;

		// Token: 0x0400385C RID: 14428
		public bool allowsFire = true;

		// Token: 0x0400385D RID: 14429
		public bool ignoreFilthMultiplierStat;

		// Token: 0x0400385E RID: 14430
		public int maxThickness = 100;

		// Token: 0x0400385F RID: 14431
		public FloatRange disappearsInDays = FloatRange.Zero;

		// Token: 0x04003860 RID: 14432
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		// Token: 0x04003861 RID: 14433
		public SoundDef cleaningSound;
	}
}
