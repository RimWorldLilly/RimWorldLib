using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200148C RID: 5260
	public class DeathActionWorker_ToxCloud : DeathActionWorker
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06008103 RID: 33027 RVA: 0x002E0170 File Offset: 0x002DE370
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008104 RID: 33028 RVA: 0x002E0174 File Offset: 0x002DE374
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 0.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 1.9f;
			}
			else
			{
				radius = 2.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.ToxGas, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.ToxGas), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
