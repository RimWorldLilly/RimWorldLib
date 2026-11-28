using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181A RID: 6170
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06009A3E RID: 39486 RVA: 0x0037AF78 File Offset: 0x00379178
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				if (!this.parent.def.building.bed_humanlike)
				{
					return from x in this.parent.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
					where x.IsAnimal && !x.RaceProps.Dryad
					select x into p
					orderby p.kindDef.label, p.Label
					select p;
				}
				return this.parent.Map.mapPawns.FreeColonists.OrderByDescending(delegate(Pawn p)
				{
					if (!this.CanAssignTo(p).Accepted)
					{
						return 0;
					}
					if (this.IdeoligionForbids(p))
					{
						return 0;
					}
					return 1;
				}).ThenBy((Pawn p) => p.LabelShort);
			}
		}

		// Token: 0x06009A3F RID: 39487 RVA: 0x0037B084 File Offset: 0x00379284
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandBedSetOwnerDesc".Translate(this.parent.def.building.bed_humanlike ? FactionDefOf.PlayerColony.pawnSingular : "Animal".Translate().ToString());
		}

		// Token: 0x06009A40 RID: 39488 RVA: 0x0037B0E0 File Offset: 0x003792E0
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x06009A41 RID: 39489 RVA: 0x0037B0F0 File Offset: 0x003792F0
		public override void TryAssignPawn(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			pawn.ownership.ClaimBedIfNonMedical(building_Bed);
			building_Bed.NotifyRoomAssignedPawnsChanged();
			this.uninstalledAssignedPawns.Remove(pawn);
		}

		// Token: 0x06009A42 RID: 39490 RVA: 0x0037B12C File Offset: 0x0037932C
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			pawn.ownership.UnclaimBed();
			if (ownedBed != null)
			{
				ownedBed.NotifyRoomAssignedPawnsChanged();
			}
			if (uninstall && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
		}

		// Token: 0x06009A43 RID: 39491 RVA: 0x0037B178 File Offset: 0x00379378
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		// Token: 0x06009A44 RID: 39492 RVA: 0x0037B1B4 File Offset: 0x003793B4
		public override AcceptanceReport CanAssignTo(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			if (pawn.BodySize > building_Bed.def.building.bed_maxBodySize)
			{
				return "TooLargeForBed".Translate();
			}
			if (!pawn.DevelopmentalStage.Baby())
			{
				if (building_Bed.ForSlaves && !pawn.IsSlave)
				{
					return "CannotAssignBedToColonist".Translate();
				}
				if (building_Bed.ForColonists && pawn.IsSlave)
				{
					return "CannotAssignBedToSlave".Translate();
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009A45 RID: 39493 RVA: 0x0037B247 File Offset: 0x00379447
		protected override bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return pawn != null && !this.AssignedAnything(pawn) && this.CanAssignTo(pawn) && (pawn.IsPrisonerOfColony || pawn.IsColonist);
		}

		// Token: 0x06009A46 RID: 39494 RVA: 0x0037B278 File Offset: 0x00379478
		public override bool IdeoligionForbids(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive || base.Props.maxAssignedPawnsCount == 1)
			{
				return base.IdeoligionForbids(pawn);
			}
			foreach (Pawn pawn2 in base.AssignedPawns)
			{
				if (!BedUtility.WillingToShareBed(pawn, pawn2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009A47 RID: 39495 RVA: 0x0037B2EC File Offset: 0x003794EC
		protected virtual void PostPostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.OwnedBed != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.");
			}
		}

		// Token: 0x06009A48 RID: 39496 RVA: 0x0037B32A File Offset: 0x0037952A
		public override void PostExposeData()
		{
			base.PostExposeData();
			this.PostPostExposeData();
		}
	}
}
