using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181C RID: 6172
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06009A54 RID: 39508 RVA: 0x0037B57C File Offset: 0x0037977C
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				IEnumerable<Pawn> second = from Corpse x in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				where x.InnerPawn.IsColonist
				select x.InnerPawn;
				return this.parent.Map.mapPawns.FreeColonistsSpawned.Concat(second);
			}
		}

		// Token: 0x06009A55 RID: 39509 RVA: 0x0037B61B File Offset: 0x0037981B
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x06009A56 RID: 39510 RVA: 0x0037B62B File Offset: 0x0037982B
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		// Token: 0x06009A57 RID: 39511 RVA: 0x0037B644 File Offset: 0x00379844
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimGrave();
		}

		// Token: 0x06009A58 RID: 39512 RVA: 0x0037B652 File Offset: 0x00379852
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		// Token: 0x06009A59 RID: 39513 RVA: 0x0037B667 File Offset: 0x00379867
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		// Token: 0x06009A5A RID: 39514 RVA: 0x0037B678 File Offset: 0x00379878
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		// Token: 0x06009A5B RID: 39515 RVA: 0x0037B68C File Offset: 0x0037988C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedGrave != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned grave. Removing.");
			}
		}
	}
}
