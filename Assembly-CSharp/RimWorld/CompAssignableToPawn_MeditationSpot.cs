using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001818 RID: 6168
	public class CompAssignableToPawn_MeditationSpot : CompAssignableToPawn
	{
		// Token: 0x06009A29 RID: 39465 RVA: 0x0037ABE0 File Offset: 0x00378DE0
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandMeditationSpotSetOwnerDesc".Translate();
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x06009A2A RID: 39466 RVA: 0x0037ABF1 File Offset: 0x00378DF1
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x06009A2B RID: 39467 RVA: 0x0037AC2C File Offset: 0x00378E2C
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

		// Token: 0x06009A2C RID: 39468 RVA: 0x0037ACB2 File Offset: 0x00378EB2
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedMeditationSpot != null;
		}

		// Token: 0x06009A2D RID: 39469 RVA: 0x0037ACC2 File Offset: 0x00378EC2
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimMeditationSpot((Building)this.parent);
		}

		// Token: 0x06009A2E RID: 39470 RVA: 0x0037ACDB File Offset: 0x00378EDB
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimMeditationSpot();
		}

		// Token: 0x06009A2F RID: 39471 RVA: 0x0037ACEC File Offset: 0x00378EEC
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedMeditationSpot != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned meditation spot. Removing.");
			}
		}
	}
}
