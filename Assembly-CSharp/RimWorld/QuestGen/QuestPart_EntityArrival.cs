using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A4 RID: 8868
	public class QuestPart_EntityArrival : QuestPart
	{
		// Token: 0x0600DAE9 RID: 56041 RVA: 0x00518B9B File Offset: 0x00516D9B
		public QuestPart_EntityArrival()
		{
		}

		// Token: 0x0600DAEA RID: 56042 RVA: 0x00518BA3 File Offset: 0x00516DA3
		public QuestPart_EntityArrival(string inSignal, Map map, Building_VoidMonolith monolith, int voidAwakeningBeganTick, List<CellRect> structureRects)
		{
			this.inSignal = inSignal;
			this.map = map;
			this.monolith = monolith;
			this.voidAwakeningBeganTick = voidAwakeningBeganTick;
			this.structureRects = structureRects;
		}

		// Token: 0x0600DAEB RID: 56043 RVA: 0x00518BD0 File Offset: 0x00516DD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Collections.Look<CellRect>(ref this.structureRects, "structureRects", LookMode.Value, Array.Empty<object>());
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.voidAwakeningBeganTick, "voidAwakeningBeganTick", 0, false);
		}

		// Token: 0x0600DAEC RID: 56044 RVA: 0x00518C40 File Offset: 0x00516E40
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!signal.tag.StartsWith(this.inSignal))
			{
				return;
			}
			VoidAwakeningUtility.WaveType waveType;
			float num;
			VoidAwakeningUtility.DecodeWaveType(signal.tag, out waveType, out num);
			if (waveType == VoidAwakeningUtility.WaveType.Twisted)
			{
				waveType = QuestPart_EntityArrival.TwistedWaveOptions.RandomElement<VoidAwakeningUtility.WaveType>();
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(this.map) * num;
			string str = "";
			string str2 = "";
			LookTargets lookTargets = null;
			switch (waveType)
			{
			case VoidAwakeningUtility.WaveType.Fleshbeast:
				lookTargets = this.SpawnFleshbeastBurrows(points, this.structureRects);
				str = "VoidAwakeningFleshbeastBurrowLabel".Translate();
				str2 = "VoidAwakeningFleshbeastBurrowText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.NoctolSightstealer:
				this.FireSightstealerNoctolAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.DevourerGorehulk:
				lookTargets = this.FireDevourerGorehulkAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			case VoidAwakeningUtility.WaveType.ShamblerGorehulk:
				lookTargets = this.FireShamblerGorehulkAssault(points);
				str = "VoidAwakeningEntityArrivalLabel".Translate();
				str2 = "VoidAwakeningEntityArrivalText".Translate();
				break;
			}
			if (!str.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.ThreatBig, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600DAED RID: 56045 RVA: 0x00518DB0 File Offset: 0x00516FB0
		private List<Thing> SpawnFleshbeastBurrows(float points, List<CellRect> structureRects)
		{
			List<Thing> list = new List<Thing>();
			LargeBuildingSpawnParms parms = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(ThingDefOf.PitBurrow);
			LargeBuildingSpawnParms parms2 = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(ThingDefOf.PitBurrow);
			Predicate<IntVec3> extraValidator = delegate(IntVec3 cell)
			{
				foreach (CellRect cellRect in structureRects)
				{
					if (cell.InHorDistOf(cellRect.CenterCell, 50f))
					{
						return false;
					}
				}
				return true;
			};
			List<CellRect> list2 = new List<CellRect>();
			list2.AddRange(structureRects);
			int num = 0;
			IntVec3 cell2;
			while (num < 6 && (LargeBuildingCellFinder.TryFindCell(out cell2, this.map, parms, list2, extraValidator, false) || LargeBuildingCellFinder.TryFindCell(out cell2, this.map, parms2, list2, null, false)))
			{
				Thing thing = FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell2, this.map, points / 6f, QuestPart_EntityArrival.PitBurrowEmergenceDelayRangeTicks, QuestPart_EntityArrival.FleshbeastSpawnDelayTicks, true);
				list.Add(thing);
				list2.Add(thing.OccupiedRect());
				num++;
			}
			return list;
		}

		// Token: 0x0600DAEE RID: 56046 RVA: 0x00518E8C File Offset: 0x0051708C
		private List<Pawn> FireSightstealerNoctolAssault(float points)
		{
			float points2 = Mathf.Max(points, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.SightstealersNoctols, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.SightstealersNoctols,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(incidentParms.faction, false, false, false, false, false, false, false), this.map, list);
			return list.ToList<Pawn>();
		}

		// Token: 0x0600DAEF RID: 56047 RVA: 0x00518F68 File Offset: 0x00517168
		private List<Pawn> FireShamblerGorehulkAssault(float points)
		{
			float points2 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Shamblers, null) * 1.05f);
			float points3 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Gorehulks, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Shamblers,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ShamblerAssault(), this.map, list);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Gorehulks,
				points = points3,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list2, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), this.map, list2);
			return list.Concat(list2).ToList<Pawn>();
		}

		// Token: 0x0600DAF0 RID: 56048 RVA: 0x005190C8 File Offset: 0x005172C8
		private List<Pawn> FireDevourerGorehulkAssault(float points)
		{
			float points2 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Devourers, null) * 1.05f);
			float points3 = Mathf.Max(points / 2f, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Gorehulks, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return null;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Devourers,
				points = points2,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_DevourerAssault(), this.map, list);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Gorehulks,
				points = points3,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list2, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_GorehulkAssault(), this.map, list2);
			return list.Concat(list2).ToList<Pawn>();
		}

		// Token: 0x0600DAF1 RID: 56049 RVA: 0x00519225 File Offset: 0x00517425
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
			this.monolith = null;
			this.structureRects.Clear();
		}

		// Token: 0x04008C23 RID: 35875
		private const float BurrowMinDistFromStructure = 50f;

		// Token: 0x04008C24 RID: 35876
		private const int BurrowCount = 6;

		// Token: 0x04008C25 RID: 35877
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x04008C26 RID: 35878
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(300, 1200);

		// Token: 0x04008C27 RID: 35879
		private static readonly List<VoidAwakeningUtility.WaveType> TwistedWaveOptions = new List<VoidAwakeningUtility.WaveType>
		{
			VoidAwakeningUtility.WaveType.DevourerGorehulk,
			VoidAwakeningUtility.WaveType.ShamblerGorehulk,
			VoidAwakeningUtility.WaveType.NoctolSightstealer
		};

		// Token: 0x04008C28 RID: 35880
		private string inSignal;

		// Token: 0x04008C29 RID: 35881
		private Map map;

		// Token: 0x04008C2A RID: 35882
		private List<CellRect> structureRects;

		// Token: 0x04008C2B RID: 35883
		private Building_VoidMonolith monolith;

		// Token: 0x04008C2C RID: 35884
		private int voidAwakeningBeganTick;
	}
}
