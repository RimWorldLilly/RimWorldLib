using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE3 RID: 3299
	public class LordToil_HuntEnemies : LordToil
	{
		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06005D3D RID: 23869 RVA: 0x00223332 File Offset: 0x00221532
		private LordToilData_HuntEnemies Data
		{
			get
			{
				return (LordToilData_HuntEnemies)this.data;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06005D3E RID: 23870 RVA: 0x0022333F File Offset: 0x0022153F
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x00223342 File Offset: 0x00221542
		public LordToil_HuntEnemies(IntVec3 fallbackLocation)
		{
			this.data = new LordToilData_HuntEnemies();
			this.Data.fallbackLocation = fallbackLocation;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x00223364 File Offset: 0x00221564
		public override void UpdateAllDuties()
		{
			LordToilData_HuntEnemies data = this.Data;
			if (!data.fallbackLocation.IsValid)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.Spawned && RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out data.fallbackLocation) && data.fallbackLocation.IsValid)
					{
						break;
					}
				}
			}
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				pawn2.mindState.duty = new PawnDuty(DutyDefOf.HuntEnemiesIndividual);
				pawn2.mindState.duty.focusSecond = data.fallbackLocation;
			}
		}
	}
}
