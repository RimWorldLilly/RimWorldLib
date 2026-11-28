using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F61 RID: 3937
	public abstract class GatheringWorker
	{
		// Token: 0x0600653A RID: 25914 RVA: 0x00241630 File Offset: 0x0023F830
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x00241668 File Offset: 0x0023F868
		public virtual bool TryExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			if (organizer == null)
			{
				return false;
			}
			IntVec3 spot;
			if (!this.TryFindGatherSpot(organizer, out spot))
			{
				return false;
			}
			LordJob lordJob = this.CreateLordJob(spot, organizer);
			Faction faction = organizer.Faction;
			LordJob lordJob2 = lordJob;
			Map map2 = organizer.Map;
			object startingPawns;
			if (!lordJob.OrganizerIsStartingPawn)
			{
				startingPawns = null;
			}
			else
			{
				(startingPawns = new Pawn[1])[0] = organizer;
			}
			LordMaker.MakeNewLord(faction, lordJob2, map2, startingPawns);
			this.SendLetter(spot, organizer);
			return true;
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x002416D0 File Offset: 0x0023F8D0
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null, 0, true);
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x0024172F File Offset: 0x0023F92F
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		// Token: 0x0600653E RID: 25918
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		// Token: 0x0600653F RID: 25919
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		// Token: 0x04003B9B RID: 15259
		public GatheringDef def;
	}
}
