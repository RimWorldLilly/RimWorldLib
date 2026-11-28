using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001819 RID: 6169
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		// Token: 0x06009A33 RID: 39475 RVA: 0x0037AD78 File Offset: 0x00378F78
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		// Token: 0x06009A34 RID: 39476 RVA: 0x0037AD89 File Offset: 0x00378F89
		protected override bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return pawn != null && !this.AssignedAnything(pawn) && this.CanAssignTo(pawn) && pawn.IsColonist;
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x06009A35 RID: 39477 RVA: 0x0037ADB0 File Offset: 0x00378FB0
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Where(delegate(Pawn p)
				{
					Faction faction;
					return p.royalty != null && (p.royalty.AllTitlesForReading.Any<RoyalTitle>() || p.royalty.CanUpdateTitleOfAnyFaction(out faction));
				})
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x06009A36 RID: 39478 RVA: 0x0037AE0C File Offset: 0x0037900C
		public override string CompInspectStringExtra()
		{
			if (base.AssignedPawnsForReading.Count == 0)
			{
				return "Owner".Translate() + ": " + "Nobody".Translate();
			}
			if (base.AssignedPawnsForReading.Count == 1)
			{
				return "Owner".Translate() + ": " + base.AssignedPawnsForReading[0].Label;
			}
			return "";
		}

		// Token: 0x06009A37 RID: 39479 RVA: 0x0037AE92 File Offset: 0x00379092
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		// Token: 0x06009A38 RID: 39480 RVA: 0x0037AEA2 File Offset: 0x003790A2
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		// Token: 0x06009A39 RID: 39481 RVA: 0x0037AEBB File Offset: 0x003790BB
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimThrone();
			if (uninstall && pawn != null && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
		}

		// Token: 0x06009A3A RID: 39482 RVA: 0x0037AEEC File Offset: 0x003790EC
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedThrone != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned throne. Removing.");
			}
		}
	}
}
