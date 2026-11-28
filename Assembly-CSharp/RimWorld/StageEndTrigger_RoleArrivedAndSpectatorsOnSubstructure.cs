using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001666 RID: 5734
	public class StageEndTrigger_RoleArrivedAndSpectatorsOnSubstructure : StageEndTrigger_RolesArrived
	{
		// Token: 0x0600900D RID: 36877 RVA: 0x0034ECB8 File Offset: 0x0034CEB8
		protected override bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			if (!base.ArrivedCheck(r, ritual))
			{
				return false;
			}
			if (Find.TickManager.TicksGame % 60 != 0)
			{
				return false;
			}
			Thing thing = ritual.selectedTarget.Thing;
			Building_GravEngine building_GravEngine;
			if (thing == null)
			{
				building_GravEngine = null;
			}
			else
			{
				CompPilotConsole compPilotConsole = thing.TryGetComp<CompPilotConsole>();
				building_GravEngine = ((compPilotConsole != null) ? compPilotConsole.engine : null);
			}
			Building_GravEngine building_GravEngine2 = building_GravEngine;
			if (building_GravEngine2 == null)
			{
				Log.Error("Engine could not be found in ritual end trigger");
				return true;
			}
			foreach (Pawn pawn in ritual.assignments.SpectatorsForReading)
			{
				if (!building_GravEngine2.ValidSubstructure.Contains(pawn.Position))
				{
					return false;
				}
			}
			if (building_GravEngine2.pawnsToBoard != null)
			{
				foreach (Pawn pawn2 in building_GravEngine2.pawnsToBoard)
				{
					if (!building_GravEngine2.ValidSubstructure.Contains(pawn2.Position))
					{
						return false;
					}
				}
			}
			if (building_GravEngine2.pawnsToLeave != null)
			{
				foreach (Pawn pawn3 in building_GravEngine2.pawnsToLeave)
				{
					if (building_GravEngine2.ValidSubstructure.Contains(pawn3.Position))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
