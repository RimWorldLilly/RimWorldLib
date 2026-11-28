using System;

namespace Verse.AI
{
	// Token: 0x02000882 RID: 2178
	public class MentalState_Slaughterer : MentalState_SlaughterThing
	{
		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x001AAFA8 File Offset: 0x001A91A8
		protected override bool SlaughterTargetAvailable
		{
			get
			{
				return SlaughtererMentalStateUtility.FindAnimal(this.pawn) != null;
			}
		}
	}
}
