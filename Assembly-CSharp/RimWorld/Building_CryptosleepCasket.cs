using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AA8 RID: 6824
	public class Building_CryptosleepCasket : Building_Casket
	{
		// Token: 0x0600AC69 RID: 44137 RVA: 0x003CD206 File Offset: 0x003CB406
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AC6A RID: 44138 RVA: 0x003CD239 File Offset: 0x003CB439
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			Building_CryptosleepCasket.<GetFloatMenuOptions>d__2 <GetFloatMenuOptions>d__ = new Building_CryptosleepCasket.<GetFloatMenuOptions>d__2(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__myPawn = myPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AC6B RID: 44139 RVA: 0x003CD250 File Offset: 0x003CB450
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_CryptosleepCasket.<GetGizmos>d__3 <GetGizmos>d__ = new Building_CryptosleepCasket.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AC6C RID: 44140 RVA: 0x003CD260 File Offset: 0x003CB460
		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		// Token: 0x0600AC6D RID: 44141 RVA: 0x003CD320 File Offset: 0x003CB520
		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			Building_CryptosleepCasket.<>c__DisplayClass5_0 CS$<>8__locals1 = new Building_CryptosleepCasket.<>c__DisplayClass5_0();
			CS$<>8__locals1.traveler = traveler;
			CS$<>8__locals1.ignoreOtherReservations = ignoreOtherReservations;
			if (Building_CryptosleepCasket.cachedCaskets == null)
			{
				Building_CryptosleepCasket.cachedCaskets = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsCryptosleepCasket
				select def).ToList<ThingDef>();
			}
			foreach (ThingDef singleDef in Building_CryptosleepCasket.cachedCaskets)
			{
				Building_CryptosleepCasket.<>c__DisplayClass5_1 CS$<>8__locals2 = new Building_CryptosleepCasket.<>c__DisplayClass5_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.queuing = KeyBindingDefOf.QueueOrder.IsDownEvent;
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.PositionHeld, p.MapHeld, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals2.<FindCryptosleepCasketFor>g__Validator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}

		// Token: 0x04006423 RID: 25635
		private static List<ThingDef> cachedCaskets;
	}
}
