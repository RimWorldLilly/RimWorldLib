using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A5 RID: 8869
	public class QuestPart_WanderingEntities : QuestPartActivable
	{
		// Token: 0x17002628 RID: 9768
		// (get) Token: 0x0600DAF3 RID: 56051 RVA: 0x0051929C File Offset: 0x0051749C
		private float MaxPoints
		{
			get
			{
				return QuestPart_WanderingEntities.MaxPointsByCurrentThreatPoints.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.map)) * QuestPart_WanderingEntities.MaxPointsFactorByDaysPassed.Evaluate((Find.TickManager.TicksGame - this.waveStartedTick).TicksToDays());
			}
		}

		// Token: 0x17002629 RID: 9769
		// (get) Token: 0x0600DAF4 RID: 56052 RVA: 0x005192D4 File Offset: 0x005174D4
		private float CurPoints
		{
			get
			{
				return this.pawns.Sum((Pawn p) => p.kindDef.combatPower);
			}
		}

		// Token: 0x1700262A RID: 9770
		// (get) Token: 0x0600DAF5 RID: 56053 RVA: 0x00519300 File Offset: 0x00517500
		private IEnumerable<PawnGroupKindDef> GroupKinds
		{
			get
			{
				return new QuestPart_WanderingEntities.<get_GroupKinds>d__13(-2);
			}
		}

		// Token: 0x0600DAF6 RID: 56054 RVA: 0x00519309 File Offset: 0x00517509
		public QuestPart_WanderingEntities()
		{
		}

		// Token: 0x0600DAF7 RID: 56055 RVA: 0x00519327 File Offset: 0x00517527
		public QuestPart_WanderingEntities(Map map, string waveStartedSignal)
		{
			this.map = map;
			this.waveStartedSignal = waveStartedSignal;
			this.inSignalEnable = waveStartedSignal;
		}

		// Token: 0x0600DAF8 RID: 56056 RVA: 0x0051935C File Offset: 0x0051755C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_Values.Look<string>(ref this.waveStartedSignal, "waveStartedSignal", null, false);
			Scribe_Values.Look<int>(ref this.waveStartedTick, "waveStartedTick", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn p) => p == null);
			}
		}

		// Token: 0x0600DAF9 RID: 56057 RVA: 0x00519400 File Offset: 0x00517600
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
		}

		// Token: 0x0600DAFA RID: 56058 RVA: 0x0051940F File Offset: 0x0051760F
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.waveStartedSignal)
			{
				return;
			}
			this.waveStartedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600DAFB RID: 56059 RVA: 0x0051943C File Offset: 0x0051763C
		public override void QuestPartTick()
		{
			if (Find.TickManager.TicksGame % 2500 != 0)
			{
				return;
			}
			this.pawns = (from p in this.pawns
			where !p.Dead && p.Spawned
			select p).ToList<Pawn>();
			if (this.CurPoints < this.MaxPoints)
			{
				this.SpawnEntities();
			}
		}

		// Token: 0x0600DAFC RID: 56060 RVA: 0x005194A8 File Offset: 0x005176A8
		private void SpawnEntities()
		{
			PawnGroupKindDef groupKind = this.GroupKinds.RandomElement<PawnGroupKindDef>();
			float num = this.MaxPoints * 0.041666668f;
			num = Mathf.Max(num, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.05f);
			IncidentParms incidentParms = new IncidentParms
			{
				target = this.map,
				raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn,
				sendLetter = false,
				faction = Faction.OfEntities
			};
			if (!incidentParms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(incidentParms))
			{
				return;
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = groupKind,
				points = num,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			PawnsArrivalModeDefOf.EdgeWalkInDistributedGroups.Worker.Arrive(list, incidentParms);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_VoidAwakeningWander(), this.map, list);
			this.pawns.AddRange(list);
		}

		// Token: 0x04008C2D RID: 35885
		private const int SpawnCheckInterval = 2500;

		// Token: 0x04008C2E RID: 35886
		private const float MaxPointFractionToSpawnPerInterval = 0.041666668f;

		// Token: 0x04008C2F RID: 35887
		private static readonly SimpleCurve MaxPointsByCurrentThreatPoints = new SimpleCurve
		{
			{
				new CurvePoint(500f, 750f),
				true
			},
			{
				new CurvePoint(2500f, 1500f),
				true
			},
			{
				new CurvePoint(5000f, 2000f),
				true
			},
			{
				new CurvePoint(10000f, 3000f),
				true
			}
		};

		// Token: 0x04008C30 RID: 35888
		private static readonly SimpleCurve MaxPointsFactorByDaysPassed = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.2f),
				true
			}
		};

		// Token: 0x04008C31 RID: 35889
		private Map map;

		// Token: 0x04008C32 RID: 35890
		private string waveStartedSignal;

		// Token: 0x04008C33 RID: 35891
		private int waveStartedTick = -99999;

		// Token: 0x04008C34 RID: 35892
		private List<Pawn> pawns = new List<Pawn>();
	}
}
