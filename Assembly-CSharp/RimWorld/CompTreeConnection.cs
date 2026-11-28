using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019CA RID: 6602
	[StaticConstructorOnStartup]
	public class CompTreeConnection : ThingComp
	{
		// Token: 0x17001CEC RID: 7404
		// (get) Token: 0x0600A5D3 RID: 42451 RVA: 0x003B216E File Offset: 0x003B036E
		public CompProperties_TreeConnection Props
		{
			get
			{
				return (CompProperties_TreeConnection)this.props;
			}
		}

		// Token: 0x17001CED RID: 7405
		// (get) Token: 0x0600A5D4 RID: 42452 RVA: 0x003B217B File Offset: 0x003B037B
		public Pawn ConnectedPawn
		{
			get
			{
				return this.connectedPawn;
			}
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x0600A5D5 RID: 42453 RVA: 0x003B2183 File Offset: 0x003B0383
		public bool Connected
		{
			get
			{
				return this.ConnectedPawn != null;
			}
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x0600A5D6 RID: 42454 RVA: 0x003B218E File Offset: 0x003B038E
		public bool ConnectionTorn
		{
			get
			{
				return this.nextUntornTick >= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001CF0 RID: 7408
		// (get) Token: 0x0600A5D7 RID: 42455 RVA: 0x003B21A5 File Offset: 0x003B03A5
		public bool HasProductionMode
		{
			get
			{
				return this.desiredMode != null;
			}
		}

		// Token: 0x17001CF1 RID: 7409
		// (get) Token: 0x0600A5D8 RID: 42456 RVA: 0x003B21B0 File Offset: 0x003B03B0
		public int UntornInDurationTicks
		{
			get
			{
				return this.nextUntornTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001CF2 RID: 7410
		// (get) Token: 0x0600A5D9 RID: 42457 RVA: 0x003B21C3 File Offset: 0x003B03C3
		public GauranlenTreeModeDef Mode
		{
			get
			{
				return this.currentMode;
			}
		}

		// Token: 0x17001CF3 RID: 7411
		// (get) Token: 0x0600A5DA RID: 42458 RVA: 0x003B21CB File Offset: 0x003B03CB
		public PawnKindDef DryadKind
		{
			get
			{
				GauranlenTreeModeDef mode = this.Mode;
				return ((mode != null) ? mode.pawnKindDef : null) ?? PawnKindDefOf.Dryad_Basic;
			}
		}

		// Token: 0x17001CF4 RID: 7412
		// (get) Token: 0x0600A5DB RID: 42459 RVA: 0x003B21E8 File Offset: 0x003B03E8
		public int MaxDryads
		{
			get
			{
				if (!this.Connected)
				{
					return this.Props.maxDryadsWild;
				}
				return (int)this.Props.maxDryadsPerConnectionStrengthCurve.Evaluate(this.ConnectionStrength);
			}
		}

		// Token: 0x17001CF5 RID: 7413
		// (get) Token: 0x0600A5DC RID: 42460 RVA: 0x003B2215 File Offset: 0x003B0415
		private int SpawningDurationTicks
		{
			get
			{
				return (int)(60000f * this.Props.spawnDays);
			}
		}

		// Token: 0x17001CF6 RID: 7414
		// (get) Token: 0x0600A5DD RID: 42461 RVA: 0x003B2229 File Offset: 0x003B0429
		// (set) Token: 0x0600A5DE RID: 42462 RVA: 0x003B2231 File Offset: 0x003B0431
		public float DesiredConnectionStrength
		{
			get
			{
				return this.desiredConnectionStrength;
			}
			set
			{
				this.desiredConnectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001CF7 RID: 7415
		// (get) Token: 0x0600A5DF RID: 42463 RVA: 0x003B223F File Offset: 0x003B043F
		// (set) Token: 0x0600A5E0 RID: 42464 RVA: 0x003B2247 File Offset: 0x003B0447
		public float ConnectionStrength
		{
			get
			{
				return this.connectionStrength;
			}
			set
			{
				this.connectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001CF8 RID: 7416
		// (get) Token: 0x0600A5E1 RID: 42465 RVA: 0x003B2255 File Offset: 0x003B0455
		private Material PodMat
		{
			get
			{
				if (this.cachedPodMat == null)
				{
					this.cachedPodMat = MaterialPool.MatFrom("Things/Building/Misc/DryadFormingPod/DryadFormingPod", ShaderDatabase.Cutout);
				}
				return this.cachedPodMat;
			}
		}

		// Token: 0x17001CF9 RID: 7417
		// (get) Token: 0x0600A5E2 RID: 42466 RVA: 0x003B2280 File Offset: 0x003B0480
		private List<Thing> BuildingsReducingConnectionStrength
		{
			get
			{
				return GauranlenUtility.BuildingsAffectingConnectionStrengthAt(this.parent.Position, this.parent.Map, this.Props);
			}
		}

		// Token: 0x17001CFA RID: 7418
		// (get) Token: 0x0600A5E3 RID: 42467 RVA: 0x003B22A4 File Offset: 0x003B04A4
		public float ConnectionStrengthLossPerDay
		{
			get
			{
				float num = this.Props.connectionLossPerLevelCurve.Evaluate(this.ConnectionStrength);
				List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
				if (this.parent.Spawned && buildingsReducingConnectionStrength.Any<Thing>())
				{
					num += this.Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(this.ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
				}
				return num;
			}
		}

		// Token: 0x17001CFB RID: 7419
		// (get) Token: 0x0600A5E4 RID: 42468 RVA: 0x003B2300 File Offset: 0x003B0500
		public float ConnectionStrengthGainPerHourOfPruning
		{
			get
			{
				float num = this.Props.connectionStrengthGainPerHourPruningBase;
				if (StatDefOf.PruningSpeed.Worker.IsDisabledFor(this.ConnectedPawn))
				{
					num *= 0.75f;
				}
				else
				{
					num *= this.ConnectedPawn.GetStatValue(StatDefOf.PruningSpeed, true, -1);
				}
				if (this.Props.connectionStrengthGainPerPlantSkill != null)
				{
					num *= this.Props.connectionStrengthGainPerPlantSkill.Evaluate((float)this.ConnectedPawn.skills.GetSkill(SkillDefOf.Plants).Level);
				}
				return num;
			}
		}

		// Token: 0x17001CFC RID: 7420
		// (get) Token: 0x0600A5E5 RID: 42469 RVA: 0x003B238C File Offset: 0x003B058C
		private float MinConnectionStrengthForSingleDryad
		{
			get
			{
				foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve.Points)
				{
					if (curvePoint.y > 0f)
					{
						return curvePoint.x;
					}
				}
				return 0f;
			}
		}

		// Token: 0x0600A5E6 RID: 42470 RVA: 0x003B2404 File Offset: 0x003B0604
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!respawningAfterLoad)
			{
				this.lastPrunedTick = Find.TickManager.TicksGame;
				this.spawnTick = Find.TickManager.TicksGame + this.SpawningDurationTicks;
			}
		}

		// Token: 0x0600A5E7 RID: 42471 RVA: 0x003B2454 File Offset: 0x003B0654
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.leafEffecter = null;
			Pawn pawn = this.ConnectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.Notify_ConnectedThingDestroyed(this.parent);
				}
			}
			for (int i = this.dryads.Count - 1; i >= 0; i--)
			{
				Pawn_ConnectionsTracker connections2 = this.dryads[i].connections;
				if (connections2 != null)
				{
					connections2.Notify_ConnectedThingDestroyed(this.parent);
				}
				this.dryads[i].forceNoDeathNotification = true;
				this.dryads[i].Kill(null, null);
				this.dryads[i].forceNoDeathNotification = false;
			}
			if (this.Connected && this.ConnectedPawn.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelConnectedTreeDestroyed".Translate(this.parent.Named("TREE")), "LetterTextConnectedTreeDestroyed".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("CONNECTEDPAWN")), LetterDefOf.NegativeEvent, this.ConnectedPawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600A5E8 RID: 42472 RVA: 0x003B2594 File Offset: 0x003B0794
		public override void Notify_MapRemoved()
		{
			Pawn pawn = this.ConnectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.Notify_ConnectedThingLeftBehind(this.parent);
				}
			}
			for (int i = this.dryads.Count - 1; i >= 0; i--)
			{
				Pawn pawn2 = this.dryads[i];
				pawn2.forceNoDeathNotification = true;
				pawn2.Kill(null, null);
				pawn2.forceNoDeathNotification = false;
			}
		}

		// Token: 0x0600A5E9 RID: 42473 RVA: 0x003B2604 File Offset: 0x003B0804
		public override void CompTick()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (Find.TickManager.TicksGame >= this.spawnTick)
			{
				this.SpawnDryad();
			}
			if (this.leafEffecter == null)
			{
				this.leafEffecter = EffecterDefOf.GauranlenLeavesBatch.Spawn();
				this.leafEffecter.Trigger(this.parent, this.parent, -1);
			}
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.parent, this.parent);
			}
			if (this.Connected && Find.TickManager.TicksGame - this.lastPrunedTick > 1)
			{
				this.ConnectionStrength -= this.ConnectionStrengthLossPerDay / 60000f;
			}
			if (this.parent.IsHashIntervalTick(300))
			{
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.dryads.Count >= 3)
				{
					IntVec3 loc;
					if (this.gaumakerPod == null && this.TryGetGaumakerCell(out loc))
					{
						this.gaumakerPod = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GaumakerCocoon, null), loc, this.parent.Map, WipeMode.Vanish);
						return;
					}
				}
				else if (this.gaumakerPod != null && !this.gaumakerPod.Destroyed)
				{
					this.gaumakerPod.Destroy(DestroyMode.Vanish);
					this.gaumakerPod = null;
				}
			}
		}

		// Token: 0x0600A5EA RID: 42474 RVA: 0x003B2754 File Offset: 0x003B0954
		public void ConnectToPawn(Pawn pawn, float ritualQuality)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				return;
			}
			if (this.ConnectionTorn)
			{
				return;
			}
			this.connectedPawn = pawn;
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.ConnectionStrength = this.Props.initialConnectionStrengthRange.LerpThroughRange(ritualQuality);
			this.lastPrunedTick = 0;
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
				MentalState mentalState = this.dryads[i].MentalState;
				if (mentalState != null)
				{
					mentalState.RecoverFromState();
				}
			}
		}

		// Token: 0x0600A5EB RID: 42475 RVA: 0x003B27F8 File Offset: 0x003B09F8
		public void FinalizeMode()
		{
			this.currentMode = this.desiredMode;
			if (this.Connected)
			{
				MoteMaker.MakeStaticMote((this.ConnectedPawn.Position.ToVector3Shifted() + this.parent.Position.ToVector3Shifted()) / 2f, this.parent.Map, ThingDefOf.Mote_GauranlenCasteChanged, 1f, false, 0f);
			}
		}

		// Token: 0x0600A5EC RID: 42476 RVA: 0x003B2870 File Offset: 0x003B0A70
		public void Notify_PawnDied(Pawn p)
		{
			if (this.connectedPawn == p)
			{
				this.TearConnection();
				return;
			}
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (p == this.dryads[i])
				{
					if (this.Connected)
					{
						Pawn_NeedsTracker needs = this.ConnectedPawn.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									thoughts.memories.TryGainMemory(ThoughtDefOf.DryadDied, null, null);
								}
							}
						}
						this.ConnectionStrength -= this.Props.connectionStrengthLossPerDryadDeath;
					}
					this.dryads.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600A5ED RID: 42477 RVA: 0x003B291A File Offset: 0x003B0B1A
		public void RemoveDryad(Pawn oldDryad)
		{
			this.dryads.Remove(oldDryad);
		}

		// Token: 0x0600A5EE RID: 42478 RVA: 0x003B292C File Offset: 0x003B0B2C
		public bool ShouldReturnToTree(Pawn dryad)
		{
			if (this.dryads.NullOrEmpty<Pawn>() || !this.dryads.Contains(dryad))
			{
				return false;
			}
			int num = this.dryads.Count - this.MaxDryads;
			if (num <= 0)
			{
				return false;
			}
			this.tmpDryads.Clear();
			this.tmpDryads.AddRange(this.dryads);
			this.tmpDryads.SortBy((Pawn x) => x.kindDef == this.DryadKind, (Pawn x) => x.ageTracker.AgeChronologicalTicks);
			for (int i = 0; i < num; i++)
			{
				if (this.tmpDryads[i] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x0600A5EF RID: 42479 RVA: 0x003B29F8 File Offset: 0x003B0BF8
		public bool ShouldEnterGaumakerPod(Pawn dryad)
		{
			if (this.gaumakerPod == null || this.gaumakerPod.Destroyed)
			{
				return false;
			}
			if (this.dryads.NullOrEmpty<Pawn>() || this.dryads.Count < 3 || !this.dryads.Contains(dryad))
			{
				return false;
			}
			this.tmpDryads.Clear();
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (this.dryads[i].kindDef == PawnKindDefOf.Dryad_Gaumaker)
				{
					this.tmpDryads.Add(this.dryads[i]);
				}
			}
			if (this.tmpDryads.Count < 3)
			{
				this.tmpDryads.Clear();
				return false;
			}
			this.tmpDryads.SortBy((Pawn x) => -x.ageTracker.AgeChronologicalTicks);
			for (int j = 0; j < 3; j++)
			{
				if (this.tmpDryads[j] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x0600A5F0 RID: 42480 RVA: 0x003B2B10 File Offset: 0x003B0D10
		private void TearConnection()
		{
			Messages.Message("MessageConnectedPawnDied".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("PAWN"), 1800000.ToStringTicksToDays("F1").Named("DURATION")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
			}
			SoundDefOf.GauranlenConnectionTorn.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			this.nextUntornTick = Find.TickManager.TicksGame + 1800000;
			this.connectedPawn = null;
			this.currentMode = null;
		}

		// Token: 0x0600A5F1 RID: 42481 RVA: 0x003B2BE4 File Offset: 0x003B0DE4
		private void SpawnDryad()
		{
			this.spawnTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.spawnDays);
			Pawn pawn = this.GenerateNewDryad(this.Props.pawnKind);
			GenSpawn.Spawn(pawn, this.parent.Position, this.parent.Map, WipeMode.Vanish).Rotation = Rot4.South;
			EffecterDefOf.DryadSpawn.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(SoundInfo.InMap(pawn, MaintenanceType.None));
		}

		// Token: 0x0600A5F2 RID: 42482 RVA: 0x003B2C90 File Offset: 0x003B0E90
		public Pawn GenerateNewDryad(PawnKindDef dryadCaste)
		{
			Faction faction = null;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			Gender? fixedGender = new Gender?(Gender.Male);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(dryadCaste, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false));
			this.ResetDryad(pawn);
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.dryads.Add(pawn);
			return pawn;
		}

		// Token: 0x0600A5F3 RID: 42483 RVA: 0x003B2D58 File Offset: 0x003B0F58
		private void ResetDryad(Pawn dryad)
		{
			if (this.Connected)
			{
				Faction faction = dryad.Faction;
				Pawn pawn = this.ConnectedPawn;
				if (faction != ((pawn != null) ? pawn.Faction : null))
				{
					Pawn pawn2 = this.ConnectedPawn;
					dryad.SetFaction((pawn2 != null) ? pawn2.Faction : null, null);
				}
			}
			if (dryad.training != null)
			{
				foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefs)
				{
					if (dryad.training.CanAssignToTrain(trainableDef).Accepted)
					{
						dryad.training.SetWantedRecursive(trainableDef, true);
						dryad.training.Train(trainableDef, this.ConnectedPawn, true);
						if (trainableDef == TrainableDefOf.Release)
						{
							dryad.playerSettings.followDrafted = true;
						}
					}
				}
			}
		}

		// Token: 0x0600A5F4 RID: 42484 RVA: 0x003B2E2C File Offset: 0x003B102C
		public void Prune(int delta)
		{
			this.lastPrunedTick = Find.TickManager.TicksGame;
			this.ConnectionStrength += this.ConnectionStrengthGainPerHourOfPruning * (float)delta / 2500f;
		}

		// Token: 0x0600A5F5 RID: 42485 RVA: 0x003B2E5C File Offset: 0x003B105C
		public bool ShouldBePrunedNow(bool forced)
		{
			if (this.ConnectionStrength >= this.desiredConnectionStrength)
			{
				return false;
			}
			if (!forced)
			{
				if (this.ConnectionStrength >= this.desiredConnectionStrength - 0.03f)
				{
					return false;
				}
				if ((float)Find.TickManager.TicksGame < (float)this.lastPrunedTick + this.TimeBetweenAutoPruning)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A5F6 RID: 42486 RVA: 0x003B2EB0 File Offset: 0x003B10B0
		private float ClosestDistanceToBlockingBuilding(List<Thing> buildings)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < buildings.Count; i++)
			{
				float num2 = buildings[i].Position.DistanceTo(this.parent.Position);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x0600A5F7 RID: 42487 RVA: 0x003B2EF8 File Offset: 0x003B10F8
		private bool TryGetGaumakerCell(out IntVec3 cell)
		{
			cell = IntVec3.Invalid;
			return CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, this.parent.Map, ThingDefOf.Plant_PodGauranlen), out cell, -1) || CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, this.parent.Map, ThingDefOf.Plant_TreeGauranlen), out cell, -1);
		}

		// Token: 0x0600A5F8 RID: 42488 RVA: 0x003B2F6B File Offset: 0x003B116B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTreeConnection.<CompGetGizmosExtra>d__74 <CompGetGizmosExtra>d__ = new CompTreeConnection.<CompGetGizmosExtra>d__74(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A5F9 RID: 42489 RVA: 0x003B2F7C File Offset: 0x003B117C
		public override void PostDraw()
		{
			if (this.dryads.Count < this.MaxDryads)
			{
				Matrix4x4 matrix = default(Matrix4x4);
				Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor()) + this.Props.spawningPodOffset;
				float num = this.Props.spawningPodSizeRange.LerpThroughRange(1f - (float)this.spawnTick - (float)Find.TickManager.TicksGame / (float)this.SpawningDurationTicks);
				matrix.SetTRS(pos, Quaternion.identity, new Vector3(num, 1f, num));
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.PodMat, 0);
			}
		}

		// Token: 0x0600A5FA RID: 42490 RVA: 0x003B302F File Offset: 0x003B122F
		public override void PostDrawExtraSelectionOverlays()
		{
			Pawn pawn = this.connectedPawn;
			if (pawn == null)
			{
				return;
			}
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections == null)
			{
				return;
			}
			connections.DrawConnectionLine(this.parent);
		}

		// Token: 0x0600A5FB RID: 42491 RVA: 0x003B3054 File Offset: 0x003B1254
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			string text2 = string.Empty;
			if (this.dryads.Count < this.MaxDryads)
			{
				text2 = "SpawningDryadIn".Translate(this.Props.pawnKind.Named("DRYAD"), (this.spawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Resolve();
			}
			if (this.ConnectionTorn)
			{
				text += "ConnectionTorn".Translate(this.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			else
			{
				text = text + "ConnectedPawn".Translate().Resolve() + ": " + (this.Connected ? this.connectedPawn.NameFullColored : "Nobody".Translate().CapitalizeFirst()).Resolve();
			}
			if (this.Connected)
			{
				if (this.lastPrunedTick >= 0 && Find.TickManager.TicksGame - this.lastPrunedTick <= 60)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"PruningConnectionStrength".Translate(),
						": ",
						"PerHour".Translate(this.ConnectionStrengthGainPerHourOfPruning.ToStringPercent()).Resolve()
					});
				}
				if (this.Mode != null)
				{
					text += "\n" + "GauranlenTreeMode".Translate() + ": " + this.Mode.LabelCap;
				}
				if (this.HasProductionMode && this.Mode != this.desiredMode)
				{
					text = text + "\n" + "WaitingForConnectorToChangeCaste".Translate(this.ConnectedPawn.Named("CONNECTEDPAWN")).Resolve();
				}
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
				if (this.MaxDryads > 0)
				{
					string str = text;
					string str2 = "\n";
					string str3 = "DryadPlural".Translate();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.dryads.Count);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.MaxDryads);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					text = str + str2 + str3 + defaultInterpolatedStringHandler.ToStringAndClear();
					if (this.dryads.Count > 0)
					{
						text = text + ": " + (from x in this.dryads
						select x.NameShortColored.Resolve()).ToCommaList(false, false).CapitalizeFirst();
					}
				}
				else
				{
					text = text + "\n" + "NotEnoughConnectionStrengthForSingleDryad".Translate(this.MinConnectionStrengthForSingleDryad.ToStringPercent()).Colorize(ColorLibrary.RedReadable);
				}
				if (!this.HasProductionMode)
				{
					text = text + "\n" + "AlertGauranlenTreeWithoutDryadTypeLabel".Translate().Colorize(ColorLibrary.RedReadable);
				}
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.MaxDryads < 3)
				{
					text = text + "\n" + "ConnectionStrengthTooWeakForGaumakerPod".Translate().Colorize(ColorLibrary.RedReadable);
				}
				string text3 = this.AffectingBuildingsDescription("ConnectionStrengthAffectedBy");
				if (!text3.NullOrEmpty())
				{
					text = text + "\n" + text3;
				}
			}
			else if (!text2.NullOrEmpty())
			{
				text = text + "\n" + text2;
			}
			return text;
		}

		// Token: 0x0600A5FC RID: 42492 RVA: 0x003B3418 File Offset: 0x003B1618
		public string AffectingBuildingsDescription(string descKey)
		{
			List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
			if (buildingsReducingConnectionStrength.Count > 0)
			{
				IEnumerable<string> source = (from c in buildingsReducingConnectionStrength
				select GenLabel.ThingLabel(c, 1, false, true)).Distinct<string>();
				TaggedString taggedString = descKey.Translate() + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
				if (source.Count<string>() > 3)
				{
					taggedString += " " + "Etc".Translate();
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x0600A5FD RID: 42493 RVA: 0x003B34B5 File Offset: 0x003B16B5
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompTreeConnection.<SpecialDisplayStats>d__79 <SpecialDisplayStats>d__ = new CompTreeConnection.<SpecialDisplayStats>d__79(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A5FE RID: 42494 RVA: 0x003B34C8 File Offset: 0x003B16C8
		private string ConnectionStrengthToMaxDryadsDesc()
		{
			string text = "MaxDryadsBasedOnConnectionStrength".Translate() + ":\n -  " + "Unconnected".Translate() + ": " + this.Props.maxDryadsWild.ToString();
			foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve)
			{
				text = text + ("\n -  " + "ConnectionStrengthDisplay".Translate(curvePoint.x.ToStringPercent()) + ": ") + curvePoint.y.ToString();
			}
			return text;
		}

		// Token: 0x0600A5FF RID: 42495 RVA: 0x003B35A8 File Offset: 0x003B17A8
		public float PruningHoursToMaintain(float desired)
		{
			float num = this.Props.connectionLossPerLevelCurve.Evaluate(desired);
			List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
			if (buildingsReducingConnectionStrength.Any<Thing>())
			{
				num += this.Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(this.ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
			}
			return num / this.ConnectionStrengthGainPerHourOfPruning;
		}

		// Token: 0x0600A600 RID: 42496 RVA: 0x003B35F8 File Offset: 0x003B17F8
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			return MeditationUtility.CountsAsArtificialBuilding(def, faction) && GenAdj.OccupiedRect(pos, rotation, def.size).ClosestCellTo(this.parent.Position).InHorDistOf(this.parent.Position, this.Props.radiusToBuildingForConnectionStrengthLoss);
		}

		// Token: 0x0600A601 RID: 42497 RVA: 0x003B3654 File Offset: 0x003B1854
		public override void PostExposeData()
		{
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.currentMode, "currentMode");
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.desiredMode, "desiredMode");
			Scribe_Values.Look<int>(ref this.nextUntornTick, "nextUntornTick", -1, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastPrunedTick, "lastPrunedTick", 0, false);
			Scribe_Values.Look<float>(ref this.desiredConnectionStrength, "desiredConnectionStrength", 0.5f, false);
			Scribe_Values.Look<float>(ref this.connectionStrength, "connectionStrength", 0f, false);
			Scribe_References.Look<Pawn>(ref this.connectedPawn, "connectedPawn", false);
			Scribe_References.Look<Thing>(ref this.gaumakerPod, "gaumakerPod", false);
			Scribe_Collections.Look<Pawn>(ref this.dryads, "dryads", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.dryads.RemoveAll((Pawn x) => x == null || x.Dead);
			}
		}

		// Token: 0x040061E6 RID: 25062
		private Pawn connectedPawn;

		// Token: 0x040061E7 RID: 25063
		private int nextUntornTick = -1;

		// Token: 0x040061E8 RID: 25064
		private int spawnTick = -1;

		// Token: 0x040061E9 RID: 25065
		private float connectionStrength;

		// Token: 0x040061EA RID: 25066
		private List<Pawn> dryads = new List<Pawn>();

		// Token: 0x040061EB RID: 25067
		private int lastPrunedTick;

		// Token: 0x040061EC RID: 25068
		private float desiredConnectionStrength = 0.5f;

		// Token: 0x040061ED RID: 25069
		private GauranlenTreeModeDef currentMode;

		// Token: 0x040061EE RID: 25070
		public Thing gaumakerPod;

		// Token: 0x040061EF RID: 25071
		public GauranlenTreeModeDef desiredMode;

		// Token: 0x040061F0 RID: 25072
		private Material cachedPodMat;

		// Token: 0x040061F1 RID: 25073
		private Effecter leafEffecter;

		// Token: 0x040061F2 RID: 25074
		private Gizmo_PruningConfig pruningGizmo;

		// Token: 0x040061F3 RID: 25075
		private const int ConnectionTornDurationTicks = 1800000;

		// Token: 0x040061F4 RID: 25076
		private const int CheckPodSpawnInterval = 300;

		// Token: 0x040061F5 RID: 25077
		public const int DryadsToCreatePod = 3;

		// Token: 0x040061F6 RID: 25078
		private float TimeBetweenAutoPruning = 10000f;

		// Token: 0x040061F7 RID: 25079
		private const float PruningConnectionStrengthDithering = 0.03f;

		// Token: 0x040061F8 RID: 25080
		private const float PruningSpeedFactor_DisabledSkill = 0.75f;

		// Token: 0x040061F9 RID: 25081
		private List<Pawn> tmpDryads = new List<Pawn>();
	}
}
