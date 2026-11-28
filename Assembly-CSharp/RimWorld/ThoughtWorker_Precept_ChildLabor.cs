using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD6 RID: 3542
	public abstract class ThoughtWorker_Precept_ChildLabor : ThoughtWorker_Precept
	{
		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06006085 RID: 24709
		protected abstract TimeAssignmentDef AssignmentDef { get; }

		// Token: 0x06006086 RID: 24710 RVA: 0x0022F9BC File Offset: 0x0022DBBC
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn pawn in p.MapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.RaceProps.Humanlike && pawn.DevelopmentalStage.Child() && pawn.timetable != null)
				{
					for (int i = 0; i < 24; i++)
					{
						if (pawn.timetable.GetAssignment(i) == this.AssignmentDef)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
