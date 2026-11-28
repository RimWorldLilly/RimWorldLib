using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001284 RID: 4740
	public class LabyrinthMapComponent : CustomMapComponent
	{
		// Token: 0x06007579 RID: 30073 RVA: 0x002938E7 File Offset: 0x00291AE7
		public LabyrinthMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600757A RID: 30074 RVA: 0x002938F0 File Offset: 0x00291AF0
		public override void MapComponentTick()
		{
			if (!this.closing && GenTicks.IsTickInterval(300) && this.abductorObelisk.DestroyedOrNull() && !this.map.mapPawns.AnyColonistSpawned)
			{
				PocketMapUtility.DestroyPocketMap(this.map);
			}
			this.TeleportPawnsClosing();
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x00293944 File Offset: 0x00291B44
		private void TeleportPawnsClosing()
		{
			LabyrinthMapComponent.<>c__DisplayClass11_0 CS$<>8__locals1 = new LabyrinthMapComponent.<>c__DisplayClass11_0();
			if (!this.closing || GenTicks.TicksGame < this.nextTeleportTick)
			{
				return;
			}
			LabyrinthMapComponent.<>c__DisplayClass11_0 CS$<>8__locals2 = CS$<>8__locals1;
			Map dest;
			if ((dest = this.sourceMap) == null)
			{
				Building building = this.abductorObelisk;
				dest = ((building != null) ? building.Map : null);
			}
			CS$<>8__locals2.dest = dest;
			this.nextTeleportTick = GenTicks.TicksGame + LabyrinthMapComponent.TeleportDelayTicks.RandomInRange;
			if (CS$<>8__locals1.dest == null)
			{
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome)
					{
						CS$<>8__locals1.dest = map;
						break;
					}
				}
			}
			if (CS$<>8__locals1.dest == null)
			{
				return;
			}
			IntVec3 cell;
			if (!CellFinderLoose.TryGetRandomCellWith((IntVec3 pos) => LabyrinthMapComponent.IsValidTeleportCell(pos, CS$<>8__locals1.dest), CS$<>8__locals1.dest, 1000, out cell))
			{
				return;
			}
			using (List<Pawn>.Enumerator enumerator2 = this.map.mapPawns.AllPawns.GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Pawn pawn = enumerator2.Current;
					Pawn pawn2 = SkipUtility.SkipTo(pawn, cell, CS$<>8__locals1.dest) as Pawn;
					if (pawn2 != null && PawnUtility.ShouldSendNotificationAbout(pawn2))
					{
						Messages.Message("MessagePawnReappeared".Translate(pawn2.Named("PAWN")), pawn2, MessageTypeDefOf.NeutralEvent, false);
					}
					pawn.inventory.UnloadEverything = true;
					return;
				}
			}
			foreach (Thing thing in ((IEnumerable<Thing>)this.map.spawnedThings))
			{
				if (thing.def.category == ThingCategory.Item)
				{
					SkipUtility.SkipTo(thing, cell, CS$<>8__locals1.dest);
					return;
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelLabyrinthExit".Translate(), "LetterLabyrinthExit".Translate(), LetterDefOf.NeutralEvent, null, 0, true);
			PocketMapUtility.DestroyPocketMap(this.map);
			if (this.abductorObelisk != null)
			{
				this.abductorObelisk.GetComp<CompObelisk_Abductor>().Notify_MapDestroyed();
				if (this.abductorObelisk.Spawned)
				{
					EffecterDefOf.Skip_EntryNoDelay.Spawn(this.abductorObelisk.Position, this.abductorObelisk.Map, 2f).Cleanup();
					this.abductorObelisk.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x00293BC0 File Offset: 0x00291DC0
		private static bool IsValidTeleportCell(IntVec3 pos, Map dest)
		{
			return !pos.Fogged(dest) && pos.Standable(dest) && dest.reachability.CanReachColony(pos);
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x00293BE2 File Offset: 0x00291DE2
		public void SetSpawnRooms(List<LayoutRoom> rooms)
		{
			this.spawnableRooms = rooms;
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x00293BEB File Offset: 0x00291DEB
		public void StartClosing()
		{
			this.closing = true;
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x00293BF4 File Offset: 0x00291DF4
		public Thing TeleportToLabyrinth(Thing thing)
		{
			IntVec3 dropPosition = this.GetDropPosition();
			Thing thing2 = SkipUtility.SkipTo(thing, dropPosition, this.map);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(ThoughtDefOf.ObeliskAbduction, null, null);
							}
						}
					}
				}
				if (PawnUtility.ShouldSendNotificationAbout(pawn))
				{
					Messages.Message("MessagePawnVanished".Translate(pawn.Named("PAWN")), thing2, MessageTypeDefOf.NeutralEvent, false);
				}
			}
			return thing2;
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x00293C90 File Offset: 0x00291E90
		private IntVec3 GetDropPosition()
		{
			foreach (CellRect cellRect in this.spawnableRooms.RandomElement<LayoutRoom>().rects)
			{
				LabyrinthMapComponent.tmpCells.AddRange(cellRect.ContractedBy(2));
			}
			IntVec3 root = LabyrinthMapComponent.tmpCells.RandomElement<IntVec3>();
			LabyrinthMapComponent.tmpCells.Clear();
			return CellFinder.StandableCellNear(root, this.map, 5f, null);
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x00293D24 File Offset: 0x00291F24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.closing, "closing", false, false);
			Scribe_Values.Look<int>(ref this.nextTeleportTick, "nextTeleportTick", 0, false);
			Scribe_References.Look<Building>(ref this.labyrinthObelisk, "labyrinthObelisk", false);
			Scribe_References.Look<Building>(ref this.abductorObelisk, "abductorObelisk", false);
			Scribe_References.Look<Map>(ref this.sourceMap, "sourceMap", false);
			Scribe_Deep.Look<LayoutStructureSketch>(ref this.structureSketch, "structureSketch", Array.Empty<object>());
			Scribe_Collections.Look<LayoutRoom>(ref this.spawnableRooms, "spawnableRects", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04004989 RID: 18825
		public Building labyrinthObelisk;

		// Token: 0x0400498A RID: 18826
		public Building abductorObelisk;

		// Token: 0x0400498B RID: 18827
		public Map sourceMap;

		// Token: 0x0400498C RID: 18828
		private LayoutStructureSketch structureSketch;

		// Token: 0x0400498D RID: 18829
		private List<LayoutRoom> spawnableRooms;

		// Token: 0x0400498E RID: 18830
		private bool closing;

		// Token: 0x0400498F RID: 18831
		private int nextTeleportTick;

		// Token: 0x04004990 RID: 18832
		private static readonly IntRange TeleportDelayTicks = new IntRange(6, 60);

		// Token: 0x04004991 RID: 18833
		private const int IntervalCheckCloseTicks = 300;

		// Token: 0x04004992 RID: 18834
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
