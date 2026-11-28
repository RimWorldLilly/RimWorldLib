using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200148B RID: 5259
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x060080FF RID: 33023 RVA: 0x002E0097 File Offset: 0x002DE297
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06008100 RID: 33024 RVA: 0x002E009E File Offset: 0x002DE29E
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008101 RID: 33025 RVA: 0x002E00A4 File Offset: 0x002DE2A4
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 2.9f;
			}
			else
			{
				radius = 4.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
