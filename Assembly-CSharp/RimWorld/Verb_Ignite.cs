using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE8 RID: 8168
	public class Verb_Ignite : Verb
	{
		// Token: 0x0600CF1B RID: 53019 RVA: 0x004C4599 File Offset: 0x004C2799
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x0600CF1C RID: 53020 RVA: 0x004C45B0 File Offset: 0x004C27B0
		protected override bool TryCastShot()
		{
			Thing thing = this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			FireUtility.TryStartFireIn(thing.OccupiedRect().ClosestCellTo(casterPawn.Position), casterPawn.Map, 0.3f, casterPawn, null);
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			return true;
		}
	}
}
