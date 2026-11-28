using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020020AD RID: 8365
	public static class SkipUtility
	{
		// Token: 0x0600D2D3 RID: 53971 RVA: 0x004D89D4 File Offset: 0x004D6BD4
		public static Thing SkipTo(Thing thing, IntVec3 cell, Map dest)
		{
			if (thing.Spawned)
			{
				SkipUtility.SkipDeSpawn(thing);
			}
			Thing thing2 = SkipUtility.SkipSpawn(thing, cell, dest);
			Pawn pawn = thing2 as Pawn;
			if (pawn != null)
			{
				pawn.Notify_Teleported(true, true);
			}
			return thing2;
		}

		// Token: 0x0600D2D4 RID: 53972 RVA: 0x004D8A0C File Offset: 0x004D6C0C
		public static Job GetEntitySkipJob(Pawn pawn, IntVec3 cell, AbilityDef abilityDef = null)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return null;
			}
			Ability ability = pawn.abilities.GetAbility(abilityDef ?? AbilityDefOf.EntitySkip, true);
			if (ability == null)
			{
				return null;
			}
			return ability.GetJob(pawn, cell);
		}

		// Token: 0x0600D2D5 RID: 53973 RVA: 0x004D8A50 File Offset: 0x004D6C50
		public static bool CanEntitySkipNow(Pawn pawn, AbilityDef abilityDef = null)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Ability ability = pawn.abilities.GetAbility(abilityDef ?? AbilityDefOf.EntitySkip, true);
			return ability != null && !ability.OnCooldown;
		}

		// Token: 0x0600D2D6 RID: 53974 RVA: 0x004D8A8C File Offset: 0x004D6C8C
		public static void SkipDeSpawn(Thing thing)
		{
			if (!thing.Spawned)
			{
				Log.ErrorOnce("Cannot skip despawn spawn a thing which is already despawned.", 89689431);
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (pawn.carryTracker.CarriedThing != null && !pawn.Drafted)
				{
					Thing thing2;
					pawn.carryTracker.TryDropCarriedThing(thing.Position, ThingPlaceMode.Direct, out thing2, null);
				}
				if (pawn.drafter != null)
				{
					pawn.wasDraftedBeforeSkip = pawn.drafter.Drafted;
				}
			}
			EffecterDefOf.Skip_EntryNoDelay.Spawn(thing, thing.MapHeld, 1f).Cleanup();
			thing.DeSpawnOrDeselect(DestroyMode.Vanish);
		}

		// Token: 0x0600D2D7 RID: 53975 RVA: 0x004D8B24 File Offset: 0x004D6D24
		private static Thing SkipSpawn(Thing thing, IntVec3 cell, Map dest)
		{
			if (thing.Spawned)
			{
				Log.ErrorOnce("Cannot skip spawn a thing which is already spawned, use SkipTo or call SkipDeSpawn first.", 47256283);
				return null;
			}
			Thing thing2 = GenSpawn.Spawn(thing, cell, dest, thing.def.defaultPlacingRot, WipeMode.Vanish, false, false);
			EffecterDefOf.Skip_Exit.Spawn(thing2, dest, 1f).Cleanup();
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Lord lord;
				if (pawn.TryGetFormingCaravanLord(out lord) && lord.Map != pawn.Map)
				{
					CaravanFormingUtility.RemovePawnFromCaravan(pawn, pawn.GetLord(), false);
				}
				if (pawn.drafter != null && pawn.wasDraftedBeforeSkip)
				{
					pawn.drafter.Drafted = true;
				}
			}
			return thing2;
		}
	}
}
