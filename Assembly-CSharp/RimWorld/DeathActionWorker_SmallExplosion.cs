using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200148D RID: 5261
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06008106 RID: 33030 RVA: 0x002E0245 File Offset: 0x002DE445
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06008107 RID: 33031 RVA: 0x002E024C File Offset: 0x002DE44C
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008108 RID: 33032 RVA: 0x002E0250 File Offset: 0x002DE450
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
