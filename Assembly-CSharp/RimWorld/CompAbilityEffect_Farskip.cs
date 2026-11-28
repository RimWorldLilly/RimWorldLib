using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013FA RID: 5114
	public class CompAbilityEffect_Farskip : CompAbilityEffect
	{
		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06007ECE RID: 32462 RVA: 0x002D5196 File Offset: 0x002D3396
		private new CompProperties_AbilityFarskip Props
		{
			get
			{
				return (CompProperties_AbilityFarskip)this.props;
			}
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x002D51A4 File Offset: 0x002D33A4
		public override void Apply(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			MapParent mapParent = target.WorldObject as MapParent;
			Map targetMap = (mapParent != null) ? mapParent.Map : null;
			IntVec3 targetCell = IntVec3.Invalid;
			List<Pawn> list = this.PawnsToSkip().ToList<Pawn>();
			if (this.parent.pawn.Spawned)
			{
				foreach (Pawn pawn in list)
				{
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(pawn, pawn.Map, 1f), pawn.Position, 60, null);
				}
				SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (this.ShouldEnterMap(target))
			{
				Pawn pawn2 = this.AlliedPawnOnMap(targetMap);
				if (pawn2 != null)
				{
					targetCell = pawn2.Position;
				}
				else
				{
					targetCell = this.parent.pawn.Position;
				}
			}
			if (targetCell.IsValid)
			{
				Predicate<IntVec3> <>9__0;
				foreach (Pawn pawn3 in list)
				{
					if (pawn3.Spawned)
					{
						pawn3.teleporting = true;
						pawn3.ExitMap(false, Rot4.Invalid);
						AbilityUtility.DoClamor(pawn3.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						pawn3.teleporting = false;
					}
					IntVec3 targetCell2 = targetCell;
					Map targetMap2 = targetMap;
					int firstTryWithRadius = 4;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator = <>9__0) == null)
					{
						extraValidator = (<>9__0 = ((IntVec3 cell) => cell != targetCell && cell.GetRoom(targetMap) == targetCell.GetRoom(targetMap)));
					}
					IntVec3 intVec;
					CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell2, targetMap2, out intVec, firstTryWithRadius, extraValidator);
					GenSpawn.Spawn(pawn3, intVec, targetMap, WipeMode.Vanish);
					if (pawn3.drafter != null && pawn3.IsColonistPlayerControlled && !pawn3.Downed)
					{
						pawn3.drafter.Drafted = true;
					}
					pawn3.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, true, false);
					pawn3.Notify_Teleported(true, true);
					CompAbilityEffect_Teleport.SendSkipUsedSignal(pawn3, this.parent.pawn);
					if (pawn3.IsPrisoner)
					{
						pawn3.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(pawn3, pawn3.Map, 1f), pawn3.Position, 60, targetMap);
					SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(intVec, pawn3.Map, false));
					if ((pawn3.IsColonist || pawn3.RaceProps.packAnimal || pawn3.IsColonyMech) && pawn3.Map.IsPlayerHome)
					{
						pawn3.inventory.UnloadEverything = true;
					}
				}
				if (caravan != null)
				{
					caravan.Destroy();
					return;
				}
			}
			else
			{
				Caravan caravan2 = target.WorldObject as Caravan;
				if (caravan2 != null && caravan2.Faction == this.parent.pawn.Faction)
				{
					if (caravan != null)
					{
						caravan.pawns.TryTransferAllToContainer(caravan2.pawns, true);
						caravan2.Notify_Merged(new List<Caravan>
						{
							caravan
						});
						caravan.Destroy();
						return;
					}
					using (List<Pawn>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn4 = enumerator.Current;
							caravan2.AddPawn(pawn4, true);
							pawn4.ExitMap(false, Rot4.Invalid);
							AbilityUtility.DoClamor(pawn4.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						}
						return;
					}
				}
				if (caravan != null)
				{
					caravan.Tile = target.Tile;
					caravan.pather.StopDead();
					return;
				}
				CaravanMaker.MakeCaravan(list, this.parent.pawn.Faction, target.Tile, false);
				foreach (Pawn pawn5 in list)
				{
					pawn5.ExitMap(false, Rot4.Invalid);
				}
			}
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x002D5684 File Offset: 0x002D3884
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_Farskip.<GetPreCastActions>d__3 <GetPreCastActions>d__ = new CompAbilityEffect_Farskip.<GetPreCastActions>d__3(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x002D5694 File Offset: 0x002D3894
		private IEnumerable<Pawn> PawnsToSkip()
		{
			CompAbilityEffect_Farskip.<PawnsToSkip>d__4 <PawnsToSkip>d__ = new CompAbilityEffect_Farskip.<PawnsToSkip>d__4(-2);
			<PawnsToSkip>d__.<>4__this = this;
			return <PawnsToSkip>d__;
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x002D56A4 File Offset: 0x002D38A4
		private Pawn AlliedPawnOnMap(Map targetMap)
		{
			return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !p.NonHumanlikeOrWildMan() && p.IsColonist && p.HomeFaction == Faction.OfPlayer && !this.PawnsToSkip().Contains(p));
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x002D56C4 File Offset: 0x002D38C4
		private bool ShouldEnterMap(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			if (caravan != null && caravan.Faction == this.parent.pawn.Faction)
			{
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			return mapParent != null && mapParent.HasMap && (this.AlliedPawnOnMap(mapParent.Map) != null || mapParent.Map == this.parent.pawn.Map);
		}

		// Token: 0x06007ED4 RID: 32468 RVA: 0x002D5740 File Offset: 0x002D3940
		private bool ShouldJoinCaravan(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			return caravan != null && caravan.Faction == this.parent.pawn.Faction;
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x002D5778 File Offset: 0x002D3978
		public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return false;
			}
			Caravan caravan2 = target.WorldObject as Caravan;
			return (caravan == null || caravan != caravan2) && (this.ShouldEnterMap(target) || this.ShouldJoinCaravan(target)) && base.Valid(target, throwMessages);
		}

		// Token: 0x06007ED6 RID: 32470 RVA: 0x002D57D8 File Offset: 0x002D39D8
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			MapParent mapParent = target.WorldObject as MapParent;
			return (mapParent == null || mapParent.Map == null || this.AlliedPawnOnMap(mapParent.Map) != null) && base.CanApplyOn(target);
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x002D5814 File Offset: 0x002D3A14
		public override Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = this.PawnsToSkip().FirstOrDefault((Pawn p) => p.IsQuestLodger());
			if (pawn != null)
			{
				return Dialog_MessageBox.CreateConfirmation("FarskipConfirmTeleportingLodger".Translate(pawn.Named("PAWN")), confirmAction, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x002D5870 File Offset: 0x002D3A70
		public override string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return "CaravanImmobilizedByMass".Translate();
			}
			if (!this.Valid(target, false))
			{
				return "AbilityNeedAllyToSkip".Translate();
			}
			if (this.ShouldJoinCaravan(target))
			{
				return "AbilitySkipToJoinCaravan".Translate();
			}
			return "AbilitySkipToRandomAlly".Translate();
		}
	}
}
