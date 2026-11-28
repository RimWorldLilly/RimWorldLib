using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C5 RID: 6597
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x0600A59A RID: 42394 RVA: 0x003B0A05 File Offset: 0x003AEC05
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x040061BC RID: 25020
		public float massCapacity = 150f;

		// Token: 0x040061BD RID: 25021
		public float restEffectiveness;

		// Token: 0x040061BE RID: 25022
		public bool max1PerGroup;

		// Token: 0x040061BF RID: 25023
		public bool canChangeAssignedThingsAfterStarting;

		// Token: 0x040061C0 RID: 25024
		public bool showOverallStats = true;

		// Token: 0x040061C1 RID: 25025
		public SoundDef pawnLoadedSound;

		// Token: 0x040061C2 RID: 25026
		public SoundDef pawnExitSound;

		// Token: 0x040061C3 RID: 25027
		public bool shouldTickContents = true;

		// Token: 0x040061C4 RID: 25028
		public bool showMassInInspectString;
	}
}
