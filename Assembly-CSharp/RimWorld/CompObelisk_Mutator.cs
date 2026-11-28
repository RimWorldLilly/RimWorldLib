using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001923 RID: 6435
	public class CompObelisk_Mutator : CompObelisk_ExplodingSpawner
	{
		// Token: 0x17001BDE RID: 7134
		// (get) Token: 0x0600A169 RID: 41321 RVA: 0x0039B0BB File Offset: 0x003992BB
		protected override IntRange SpawnIntervalTicks
		{
			get
			{
				return new IntRange(30, 90);
			}
		}

		// Token: 0x0600A16A RID: 41322 RVA: 0x0039B0C8 File Offset: 0x003992C8
		public override void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false)
		{
			int num = triggeredByPlayer ? Rand.RangeInclusive(0, 3) : Rand.RangeInclusive(0, 5);
			switch (num)
			{
			case 0:
				this.DoMutation(interactor, HediffDefOf.Tentacle, "ObeliskTentacleLetterLabel", "ObeliskTentacleLetter", "MutatorObeliskFailedArmLetterLabel", "MutatorObeliskFailedArmLetter");
				return;
			case 1:
				this.DoMutation(interactor, HediffDefOf.FleshWhip, "ObeliskFleshWhipLetterLabel", "ObeliskFleshWhipLetter", "MutatorObeliskFailedArmLetterLabel", "MutatorObeliskFailedArmLetter");
				return;
			case 2:
				this.DoMutation(interactor, HediffDefOf.FleshmassLung, "ObeliskFleshmassLungLetterLabel", "ObeliskFleshmassLungLetter", "MutatorObeliskFailedLungLetterLabel", "MutatorObeliskFailedLungLetter");
				return;
			case 3:
				this.DoMutation(interactor, HediffDefOf.FleshmassStomach, "ObeliskFleshmassStomachLetterLabel", "ObeliskFleshmassStomachLetter", "MutatorObeliskFailedStomachLetterLabel", "MutatorObeliskFailedStomachLetter");
				return;
			case 4:
			{
				Pawn arg;
				Pawn t;
				if (this.TryMutatingRandomAnimal(out arg, out t))
				{
					this.lastInteractionEffectTick = Find.TickManager.TicksGame;
					Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("ObeliskAnimalMutationLetterLabel".Translate(), "ObeliskAnimalMutationLetter".Translate(interactor.Named("PAWN"), arg.Named("ANIMAL")), LetterDefOf.ThreatSmall, t, null, null, null), null, 0, true);
					return;
				}
				break;
			}
			case 5:
			{
				HarbingerTree t2;
				if (this.TryMutateRandomTree(out t2))
				{
					this.lastInteractionEffectTick = Find.TickManager.TicksGame;
					Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("ObeliskTreeMutationLetterLabel".Translate(), "ObeliskTreeMutationLetter".Translate(interactor.Named("PAWN")), LetterDefOf.NeutralEvent, t2, null, null, null), null, 0, true);
					return;
				}
				break;
			}
			default:
				Log.Error("Unhandled outcome in mutator obelisk trigger interaction " + num.ToString());
				break;
			}
		}

		// Token: 0x0600A16B RID: 41323 RVA: 0x0039B26C File Offset: 0x0039946C
		private void DoMutation(Pawn pawn, HediffDef mutation, string letterLabel, string letter, string failedLabel, string failedLetter)
		{
			if (FleshbeastUtility.TryGiveMutation(pawn, mutation))
			{
				this.lastInteractionEffectTick = Find.TickManager.TicksGame;
				Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter(letterLabel.Translate(), letter.Translate(pawn.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null), null, 0, true);
				return;
			}
			Find.LetterStack.ReceiveLetter(failedLabel.Translate(), failedLetter.Translate(pawn.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
		}

		// Token: 0x0600A16C RID: 41324 RVA: 0x0039B300 File Offset: 0x00399500
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			Find.LetterStack.ReceiveLetter("MutatorObeliskLetterLabel".Translate(), "MutatorObeliskLetter".Translate(), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x0600A16D RID: 41325 RVA: 0x0039B348 File Offset: 0x00399548
		public override void CompTick()
		{
			base.CompTick();
			if (!this.activated)
			{
				return;
			}
			if (base.ActivityComp.Deactivated)
			{
				return;
			}
			if (this.explodeTick > 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.nextSpawnTick)
			{
				return;
			}
			if (this.warmupComplete)
			{
				this.nextSpawnTick = Find.TickManager.TicksGame + this.SpawnIntervalTicks.RandomInRange;
				Pawn arg;
				Pawn pawn;
				IntVec3 cell;
				if (this.TryMutatingRandomAnimal(out arg, out pawn))
				{
					this.pointsRemaining -= pawn.kindDef.combatPower;
					base.Lord.AddPawn(pawn);
					Messages.Message("MutatorObeliskMutated".Translate(arg.Named("PAWN")), pawn, MessageTypeDefOf.ThreatSmall, true);
				}
				else if (CellFinder.TryFindRandomCell(this.parent.Map, new Predicate<IntVec3>(this.IsValidSpawnCell), out cell))
				{
					Pawn pawn2 = FleshbeastUtility.SpawnFleshbeastFromGround(cell, this.parent.Map, CompObelisk_Mutator.TunnelDelayTicks, -1f);
					base.Lord.AddPawn(pawn2);
					EffecterDefOf.ObeliskSpark.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
					this.pointsRemaining -= pawn2.kindDef.combatPower;
					Messages.Message("MutatorObeliskSpawned".Translate(), pawn2, MessageTypeDefOf.ThreatSmall, true);
				}
				if (this.pointsRemaining <= 0f)
				{
					base.PrepareExplosion();
				}
			}
		}

		// Token: 0x0600A16E RID: 41326 RVA: 0x0039B4E0 File Offset: 0x003996E0
		private bool TryMutatingRandomAnimal(out Pawn mutatedAnimal, out Pawn resultBeast)
		{
			resultBeast = null;
			mutatedAnimal = null;
			Pawn pawn3;
			(from pawn in this.parent.Map.mapPawns.AllPawnsSpawned
			where pawn.Faction == null && pawn.IsAnimal && !pawn.Position.Fogged(this.parent.Map)
			select pawn).TryRandomElement(out pawn3);
			if (pawn3 == null)
			{
				return false;
			}
			mutatedAnimal = pawn3;
			Pawn pawn2 = FleshbeastUtility.SpawnFleshbeastFromPawn(pawn3, false, false, Array.Empty<PawnKindDef>());
			resultBeast = pawn2;
			EffecterDefOf.ObeliskSpark.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			return true;
		}

		// Token: 0x0600A16F RID: 41327 RVA: 0x0039B568 File Offset: 0x00399768
		private bool TryMutateRandomTree(out HarbingerTree tree)
		{
			tree = null;
			CompObelisk_Mutator.TmpTrees.Clear();
			for (int i = 0; i < 6; i++)
			{
				int num = 20 * i;
				int num2 = Mathf.Max(20 * (i - 1) - 1, 1);
				foreach (IntVec3 c in GenRadial.RadialCellsAround(this.parent.Position, (float)num2, (float)num))
				{
					Plant plant;
					if (c.InBounds(this.parent.Map) && c.TryGetFirstThing(this.parent.Map, out plant) && plant.def.plant.IsTree && plant.def.plant.treeCategory != TreeCategory.Super && plant.def != ThingDefOf.Plant_Fibercorn)
					{
						CompObelisk_Mutator.TmpTrees.Add(plant);
					}
				}
				if (CompObelisk_Mutator.TmpTrees.Any<Plant>())
				{
					break;
				}
			}
			if (CompObelisk_Mutator.TmpTrees.Empty<Plant>())
			{
				return false;
			}
			Plant plant2 = CompObelisk_Mutator.TmpTrees.RandomElement<Plant>();
			CompObelisk_Mutator.TmpTrees.Clear();
			tree = (HarbingerTree)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null);
			GenSpawn.Spawn(tree, plant2.Position, plant2.Map, WipeMode.Vanish);
			plant2.Destroy(DestroyMode.Vanish);
			EffecterDefOf.ObeliskSpark.Spawn(tree.Position, tree.Map, 1f).Cleanup();
			return true;
		}

		// Token: 0x0600A170 RID: 41328 RVA: 0x0039B6E0 File Offset: 0x003998E0
		private bool IsValidSpawnCell(IntVec3 c)
		{
			return c.Standable(this.parent.Map) && !c.GetTerrain(this.parent.Map).IsWater && !c.Fogged(this.parent.Map);
		}

		// Token: 0x04005F5D RID: 24413
		private const int TreeSearchStepSize = 20;

		// Token: 0x04005F5E RID: 24414
		private static readonly IntRange TunnelDelayTicks = new IntRange(60, 120);

		// Token: 0x04005F5F RID: 24415
		private static readonly List<Plant> TmpTrees = new List<Plant>();
	}
}
