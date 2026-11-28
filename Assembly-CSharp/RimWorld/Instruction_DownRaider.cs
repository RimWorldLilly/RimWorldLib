using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0B RID: 7691
	public class Instruction_DownRaider : Lesson_Instruction
	{
		// Token: 0x0600C7DE RID: 51166 RVA: 0x004957E2 File Offset: 0x004939E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.coverCells, "coverCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0600C7DF RID: 51167 RVA: 0x00495800 File Offset: 0x00493A00
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = Find.TutorialState.sandbagsRect.ContractedBy(1);
			this.coverCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect.EdgeCells)
			{
				if (intVec.x != cellRect.CenterCell.x && intVec.z != cellRect.CenterCell.z)
				{
					this.coverCells.Add(intVec);
				}
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = base.Map;
			incidentParms.points = PawnKindDefOf.Drifter.combatPower;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.raidForceOneDowned = true;
			incidentParms.raidNeverFleeIndividual = true;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}

		// Token: 0x0600C7E0 RID: 51168 RVA: 0x004958F4 File Offset: 0x00493AF4
		private bool AllColonistsInCover()
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!this.coverCells.Contains(pawn.Position))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C7E1 RID: 51169 RVA: 0x00495964 File Offset: 0x00493B64
		public override void LessonOnGUI()
		{
			if (!this.AllColonistsInCover())
			{
				TutorUtility.DrawCellRectOnGUI(Find.TutorialState.sandbagsRect, this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x0600C7E2 RID: 51170 RVA: 0x00495990 File Offset: 0x00493B90
		public override void LessonUpdate()
		{
			if (!this.AllColonistsInCover())
			{
				for (int i = 0; i < this.coverCells.Count; i++)
				{
					Vector3 position = this.coverCells[i].ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
					Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
				}
			}
			if (base.Map.mapPawns.PawnsInFaction(Faction.OfPlayer).Any((Pawn p) => p.Downed))
			{
				foreach (Pawn pawn in base.Map.mapPawns.AllPawns)
				{
					if (pawn.HostileTo(Faction.OfPlayer))
					{
						HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
					}
				}
			}
			if ((from p in base.Map.mapPawns.AllPawnsSpawned
			where p.HostileTo(Faction.OfPlayer) && !p.Downed
			select p).Count<Pawn>() == 0)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x040071DC RID: 29148
		private List<IntVec3> coverCells;
	}
}
