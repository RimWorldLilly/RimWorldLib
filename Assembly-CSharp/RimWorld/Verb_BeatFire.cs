using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE1 RID: 8161
	public class Verb_BeatFire : Verb
	{
		// Token: 0x0600CF00 RID: 52992 RVA: 0x004C41B9 File Offset: 0x004C23B9
		public Verb_BeatFire()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire);
		}

		// Token: 0x0600CF01 RID: 52993 RVA: 0x004C41D0 File Offset: 0x004C23D0
		protected override bool TryCastShot()
		{
			Fire fire = (Fire)this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			if (casterPawn.stances.FullBodyBusy || fire.TicksSinceSpawn == 0)
			{
				return false;
			}
			fire.TakeDamage(new DamageInfo(DamageDefOf.Extinguish, 32f, 0f, -1f, this.caster, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			casterPawn.Drawer.Notify_MeleeAttackOn(fire);
			return true;
		}

		// Token: 0x040083BE RID: 33726
		private const int DamageAmount = 32;
	}
}
