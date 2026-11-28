using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181B RID: 6171
	public class CompAssignableToPawn_DeathrestCasket : CompAssignableToPawn_Bed
	{
		// Token: 0x06009A4C RID: 39500 RVA: 0x0037B384 File Offset: 0x00379584
		public override string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06009A4D RID: 39501 RVA: 0x0037B387 File Offset: 0x00379587
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedDeathrestCasket != null;
		}

		// Token: 0x06009A4E RID: 39502 RVA: 0x0037B398 File Offset: 0x00379598
		public override void TryAssignPawn(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			pawn.ownership.ClaimDeathrestCasket(building_Bed);
			building_Bed.NotifyRoomAssignedPawnsChanged();
		}

		// Token: 0x06009A4F RID: 39503 RVA: 0x0037B3C4 File Offset: 0x003795C4
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			pawn.ownership.UnclaimDeathrestCasket();
			if (ownedBed != null)
			{
				ownedBed.NotifyRoomAssignedPawnsChanged();
			}
		}

		// Token: 0x06009A50 RID: 39504 RVA: 0x0037B3F4 File Offset: 0x003795F4
		public override AcceptanceReport CanAssignTo(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			if (pawn.BodySize > building_Bed.def.building.bed_maxBodySize)
			{
				return "TooLargeForBed".Translate();
			}
			if (building_Bed.ForSlaves && !pawn.IsSlave)
			{
				return "CannotAssignBedToColonist".Translate();
			}
			if (building_Bed.ForColonists && pawn.IsSlave)
			{
				return "CannotAssignBedToSlave".Translate();
			}
			CompDeathrestBindable compDeathrestBindable = this.parent.TryGetComp<CompDeathrestBindable>();
			if (compDeathrestBindable != null && compDeathrestBindable.BoundPawn != null && compDeathrestBindable.BoundPawn != pawn)
			{
				return "CannotAssignAlreadyBound".Translate(compDeathrestBindable.BoundPawn);
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null)
			{
				return "CannotAssignBedCannotDeathrest".Translate();
			}
			if (compDeathrestBindable != null && gene_Deathrest.BindingWillExceedStackLimit(compDeathrestBindable))
			{
				return "CannotAssignBedCannotBindToMoreBuildings".Translate(this.parent.def.Named("BUILDING"));
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009A51 RID: 39505 RVA: 0x0037B50C File Offset: 0x0037970C
		protected override void PostPostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.BiotechActive && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedDeathrestCasket != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.");
			}
		}
	}
}
