using System;

namespace RimWorld
{
	// Token: 0x02000BBB RID: 3003
	public class WorkGiver_TakeRoamingAnimalsToPen : WorkGiver_TakeToPen
	{
		// Token: 0x06005695 RID: 22165 RVA: 0x00202DF6 File Offset: 0x00200FF6
		public WorkGiver_TakeRoamingAnimalsToPen()
		{
			this.targetRoamingAnimals = true;
			this.allowUnenclosedPens = true;
		}
	}
}
