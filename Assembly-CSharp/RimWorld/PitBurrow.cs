using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B2A RID: 6954
	public class PitBurrow : Crater
	{
		// Token: 0x17002021 RID: 8225
		// (get) Token: 0x0600B236 RID: 45622 RVA: 0x003EBEE7 File Offset: 0x003EA0E7
		protected override EffecterDef FilledInEffecter
		{
			get
			{
				return EffecterDefOf.ImpactSmallDustCloud;
			}
		}

		// Token: 0x17002022 RID: 8226
		// (get) Token: 0x0600B237 RID: 45623 RVA: 0x003EBEEE File Offset: 0x003EA0EE
		protected override SoundDef FilledInSound
		{
			get
			{
				return SoundDefOf.PitBurrow_Collapse;
			}
		}

		// Token: 0x0600B238 RID: 45624 RVA: 0x003EBEF8 File Offset: 0x003EA0F8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Pit burrow"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.collapseTick = Find.TickManager.TicksGame + PitBurrow.CollapseTicksRange.RandomInRange;
			if (this.emergingFleshbeasts.NullOrEmpty<Pawn>())
			{
				return;
			}
			this.FlushSpawnQueue();
		}

		// Token: 0x0600B239 RID: 45625 RVA: 0x003EBF58 File Offset: 0x003EA158
		private void FlushSpawnQueue()
		{
			CellRect cellRect = GenAdj.OccupiedRect(base.Position, Rot4.North, ThingDefOf.PitBurrow.Size).ContractedBy(2);
			List<PawnFlyer> list = new List<PawnFlyer>();
			List<IntVec3> list2 = new List<IntVec3>();
			foreach (Pawn pawn in this.emergingFleshbeasts)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				GenSpawn.Spawn(pawn, randomCell, base.Map, WipeMode.Vanish);
				IntVec3 intVec;
				if (CellFinder.TryFindRandomCellNear(base.Position, base.Map, this.def.size.x / 2 + 1, (IntVec3 c) => !c.Fogged(base.Map) && c.Walkable(base.Map) && !c.Impassable(base.Map), out intVec, -1))
				{
					pawn.rotationTracker.FaceCell(intVec);
					list.Add(PawnFlyer.MakeFlyer(ThingDefOf.PawnFlyer_Stun, pawn, intVec, null, null, false, new Vector3?(randomCell.ToVector3() + new Vector3(0f, 0f, -1f)), null, default(LocalTargetInfo)));
					list2.Add(randomCell);
				}
			}
			if (list2.Count == 0)
			{
				return;
			}
			SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList<Thing>(), list2, 1, 1f, null);
			spawnRequest.initialDelay = this.emergeDelay;
			if (this.assaultColony)
			{
				spawnRequest.lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), base.Map, null);
			}
			base.Map.deferredSpawner.AddRequest(spawnRequest, true);
			SoundDefOf.Pawn_Fleshbeast_EmergeFromPitGate.PlayOneShot(this);
			this.emergingFleshbeasts.Clear();
		}

		// Token: 0x0600B23A RID: 45626 RVA: 0x003EC10C File Offset: 0x003EA30C
		protected override void Tick()
		{
			base.Tick();
			if (Find.TickManager.TicksGame > this.collapseTick)
			{
				Messages.Message("MessagePitBurrowCollapsed".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NeutralEvent, false);
				this.Collapse();
			}
		}

		// Token: 0x0600B23B RID: 45627 RVA: 0x003EC168 File Offset: 0x003EA368
		private void Collapse()
		{
			EffecterDefOf.ImpactSmallDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
			this.FillIn();
		}

		// Token: 0x0600B23C RID: 45628 RVA: 0x003EC190 File Offset: 0x003EA390
		public override IEnumerable<Gizmo> GetGizmos()
		{
			PitBurrow.<GetGizmos>d__14 <GetGizmos>d__ = new PitBurrow.<GetGizmos>d__14(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B23D RID: 45629 RVA: 0x003EC1A0 File Offset: 0x003EA3A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.emergeDelay, "emergeDelay", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.emergingFleshbeasts, "emergingFleshbeasts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.assaultColony, "assaultColony", true, false);
			Scribe_Values.Look<int>(ref this.collapseTick, "collapseTick", 0, false);
		}

		// Token: 0x040066D1 RID: 26321
		public List<Pawn> emergingFleshbeasts;

		// Token: 0x040066D2 RID: 26322
		public int emergeDelay;

		// Token: 0x040066D3 RID: 26323
		public bool assaultColony = true;

		// Token: 0x040066D4 RID: 26324
		private int collapseTick;

		// Token: 0x040066D5 RID: 26325
		private const int SpawnIntervalSeconds = 1;

		// Token: 0x040066D6 RID: 26326
		private static readonly IntRange CollapseTicksRange = new IntRange(720000, 1200000);
	}
}
