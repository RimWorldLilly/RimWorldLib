using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFD RID: 3581
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x060060E5 RID: 24805 RVA: 0x00230E35 File Offset: 0x0022F035
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p;
		}
	}
}
