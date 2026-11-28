using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001FF2 RID: 8178
	public class Verb_Spawn : Verb_CastBase
	{
		// Token: 0x0600CF4D RID: 53069 RVA: 0x004C5900 File Offset: 0x004C3B00
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			GenSpawn.Spawn(this.verbProps.spawnDef, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			if (this.verbProps.colonyWideTaleDef != null)
			{
				Pawn pawn = this.caster.Map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
				TaleRecorder.RecordTale(this.verbProps.colonyWideTaleDef, new object[]
				{
					this.caster,
					pawn
				});
			}
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}
	}
}
