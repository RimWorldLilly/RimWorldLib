using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.QuestGen
{
	// Token: 0x0200229B RID: 8859
	public class QuestPart_Noctoliths : QuestPart
	{
		// Token: 0x0600DABD RID: 55997 RVA: 0x00517190 File Offset: 0x00515390
		public QuestPart_Noctoliths()
		{
		}

		// Token: 0x0600DABE RID: 55998 RVA: 0x005171BC File Offset: 0x005153BC
		public QuestPart_Noctoliths(MapParent mapParent, List<Thing> noctoliths, float points, string inSignalNoctolithDamaged, string inSignalNoctolithKilled)
		{
			this.mapParent = mapParent;
			this.noctoliths = noctoliths;
			this.points = points;
			this.inSignalNoctolithDamaged = inSignalNoctolithDamaged;
			this.inSignalNoctolithKilled = inSignalNoctolithKilled;
			for (int i = 0; i < 3; i++)
			{
				this.healthPercentages.Add(QuestPart_Noctoliths.HealthRangeToSpawnNoctols.RandomInRange);
			}
		}

		// Token: 0x0600DABF RID: 55999 RVA: 0x0051723C File Offset: 0x0051543C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return;
			}
			Map map = this.mapParent.Map;
			Thing thing;
			if (signal.tag == this.inSignalNoctolithDamaged && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Position.InBounds(map) && thing.def == ThingDefOf.Noctolith && this.damagedCount < 3 && (float)thing.HitPoints / (float)thing.MaxHitPoints <= this.healthPercentages[this.damagedCount] && !this.damagedNoctoliths.Contains(thing))
			{
				this.damagedNoctoliths.Add(thing);
				this.damagedCount++;
				this.SpawnNoctols(thing, map);
				return;
			}
			Thing item;
			if (signal.tag == this.inSignalNoctolithKilled && signal.args.TryGetArg<Thing>("SUBJECT", out item) && this.noctoliths.Contains(item))
			{
				this.killedCount++;
				if (!this.noctoliths.Any((Thing x) => !x.DestroyedOrNull()))
				{
					Find.LetterStack.ReceiveLetter("DarknessLiftingEarlyLetterLabel".Translate(), "DarknessLiftingEarlyLetterText".Translate(), LetterDefOf.PositiveEvent, null, 0, true);
					this.quest.End(QuestEndOutcome.Success, true, true);
					return;
				}
				if (this.killedCount == 1)
				{
					Find.LetterStack.ReceiveLetter("DarknessWaveringLetterLabel".Translate(), "DarknessWaveringLetterText".Translate(), LetterDefOf.PositiveEvent, null, 0, true);
				}
			}
		}

		// Token: 0x0600DAC0 RID: 56000 RVA: 0x005173EC File Offset: 0x005155EC
		private void SpawnNoctols(Thing noctolith, Map map)
		{
			IntVec3 position = noctolith.Position;
			float num = this.points * QuestPart_Noctoliths.DamagedNoctolithsToPointsScaleCurve.Evaluate((float)this.damagedCount);
			List<Pawn> noctolsForPoints = this.GetNoctolsForPoints(num, map);
			if (!noctolsForPoints.Any<Pawn>())
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			Lord lord = map.lordManager.lords.FirstOrDefault((Lord l) => l.LordJob is LordJob_AssaultColony && l.faction == Faction.OfEntities);
			if (lord == null)
			{
				flag2 = true;
				lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), map, null);
			}
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> <>9__3;
			foreach (Pawn pawn in noctolsForPoints)
			{
				IntVec3 root = position;
				Map map2 = map;
				int squareRadius = 40;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => base.<SpawnNoctols>g__ValidSpawnCell|1(c, true)));
				}
				IntVec3 loc;
				if (!CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out loc, -1))
				{
					IntVec3 root2 = position;
					Map map3 = map;
					int squareRadius2 = 40;
					Predicate<IntVec3> validator2;
					if ((validator2 = <>9__3) == null)
					{
						validator2 = (<>9__3 = ((IntVec3 c) => base.<SpawnNoctols>g__ValidSpawnCell|1(c, false)));
					}
					if (!CellFinder.TryFindRandomCellNear(root2, map3, squareRadius2, validator2, out loc, -1))
					{
						continue;
					}
				}
				PawnGroundSpawner pawnGroundSpawner = (PawnGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PawnGroundSpawner, null);
				pawnGroundSpawner.Init(pawn, QuestPart_Noctoliths.TunnelDelayTicks);
				GenSpawn.Spawn(pawnGroundSpawner, loc, map, WipeMode.Vanish);
				flag = true;
				lord.AddPawn(pawn);
			}
			SoundDefOf.Noctolith_Destroyed.PlayOneShotOnCamera(null);
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("NoctolAttackLetterLabel".Translate(), "NoctolAttackLetterText".Translate(), LetterDefOf.ThreatBig, new LookTargets(noctolsForPoints), null, null, null, null, 0, true);
				return;
			}
			if (flag2)
			{
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x0600DAC1 RID: 56001 RVA: 0x005175DC File Offset: 0x005157DC
		private List<Pawn> GetNoctolsForPoints(float points, Map map)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Noctols,
				tile = map.Tile,
				faction = Faction.OfEntities
			};
			pawnGroupMakerParms.points = ((points > 0f) ? points : StorytellerUtility.DefaultThreatPointsNow(map)) * QuestPart_Noctoliths.NoctolPointsFactorRange.RandomInRange;
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, pawnGroupMakerParms.faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			IEnumerable<Pawn> enumerable = PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
			if (enumerable == null)
			{
				return null;
			}
			return enumerable.ToList<Pawn>();
		}

		// Token: 0x0600DAC2 RID: 56002 RVA: 0x00517676 File Offset: 0x00515876
		public override void Cleanup()
		{
			base.Cleanup();
			this.damagedNoctoliths.Clear();
			this.healthPercentages.Clear();
		}

		// Token: 0x0600DAC3 RID: 56003 RVA: 0x00517694 File Offset: 0x00515894
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Values.Look<string>(ref this.inSignalNoctolithDamaged, "inSignalNoctolithDamaged", null, false);
			Scribe_Values.Look<string>(ref this.inSignalNoctolithKilled, "inSignalNoctolithKilled", null, false);
			Scribe_Values.Look<int>(ref this.damagedCount, "damagedCount", 0, false);
			Scribe_Values.Look<int>(ref this.killedCount, "killedCount", 0, false);
			Scribe_Collections.Look<Thing>(ref this.damagedNoctoliths, "damagedNoctoliths", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.noctoliths, "noctoliths", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.healthPercentages, "healthPercentages", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.damagedNoctoliths.RemoveAll((Thing x) => x == null);
				this.noctoliths.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04008BF3 RID: 35827
		private float points;

		// Token: 0x04008BF4 RID: 35828
		private string inSignalNoctolithDamaged;

		// Token: 0x04008BF5 RID: 35829
		private string inSignalNoctolithKilled;

		// Token: 0x04008BF6 RID: 35830
		private int damagedCount;

		// Token: 0x04008BF7 RID: 35831
		private int killedCount;

		// Token: 0x04008BF8 RID: 35832
		private List<Thing> damagedNoctoliths = new List<Thing>();

		// Token: 0x04008BF9 RID: 35833
		private List<Thing> noctoliths = new List<Thing>();

		// Token: 0x04008BFA RID: 35834
		private List<float> healthPercentages = new List<float>();

		// Token: 0x04008BFB RID: 35835
		private MapParent mapParent;

		// Token: 0x04008BFC RID: 35836
		private static readonly FloatRange NoctolPointsFactorRange = new FloatRange(0.8f, 1f);

		// Token: 0x04008BFD RID: 35837
		private static readonly FloatRange HealthRangeToSpawnNoctols = new FloatRange(0f, 0.99f);

		// Token: 0x04008BFE RID: 35838
		public static readonly SimpleCurve DamagedNoctolithsToPointsScaleCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.35f),
				true
			},
			{
				new CurvePoint(2f, 0.35f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			}
		};

		// Token: 0x04008BFF RID: 35839
		private static readonly IntRange TunnelDelayTicks = new IntRange(60, 120);
	}
}
