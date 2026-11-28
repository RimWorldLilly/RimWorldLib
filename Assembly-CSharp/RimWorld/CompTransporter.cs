using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019C6 RID: 6598
	[StaticConstructorOnStartup]
	public class CompTransporter : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17001CDB RID: 7387
		// (get) Token: 0x0600A59B RID: 42395 RVA: 0x003B0A36 File Offset: 0x003AEC36
		public CompProperties_Transporter Props
		{
			get
			{
				return (CompProperties_Transporter)this.props;
			}
		}

		// Token: 0x17001CDC RID: 7388
		// (get) Token: 0x0600A59C RID: 42396 RVA: 0x003B0A43 File Offset: 0x003AEC43
		public Map Map
		{
			get
			{
				return this.parent.MapHeld;
			}
		}

		// Token: 0x17001CDD RID: 7389
		// (get) Token: 0x0600A59D RID: 42397 RVA: 0x003B0A50 File Offset: 0x003AEC50
		public bool AnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoad != null;
			}
		}

		// Token: 0x17001CDE RID: 7390
		// (get) Token: 0x0600A59E RID: 42398 RVA: 0x003B0A5B File Offset: 0x003AEC5B
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.groupID >= 0 || this.parent.IsInCaravan();
			}
		}

		// Token: 0x17001CDF RID: 7391
		// (get) Token: 0x0600A59F RID: 42399 RVA: 0x003B0A73 File Offset: 0x003AEC73
		public bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.FirstThingLeftToLoadInGroup != null;
			}
		}

		// Token: 0x17001CE0 RID: 7392
		// (get) Token: 0x0600A5A0 RID: 42400 RVA: 0x003B0A7E File Offset: 0x003AEC7E
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityOverride > 0f)
				{
					return this.massCapacityOverride;
				}
				return this.Props.massCapacity;
			}
		}

		// Token: 0x17001CE1 RID: 7393
		// (get) Token: 0x0600A5A1 RID: 42401 RVA: 0x003B0A9F File Offset: 0x003AEC9F
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001CE2 RID: 7394
		// (get) Token: 0x0600A5A2 RID: 42402 RVA: 0x003B0AA7 File Offset: 0x003AECA7
		public bool Groupable
		{
			get
			{
				return !this.Props.max1PerGroup;
			}
		}

		// Token: 0x17001CE3 RID: 7395
		// (get) Token: 0x0600A5A3 RID: 42403 RVA: 0x003B0AB7 File Offset: 0x003AECB7
		public bool OverMassCapacity
		{
			get
			{
				return this.MassUsage > this.MassCapacity;
			}
		}

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x0600A5A4 RID: 42404 RVA: 0x003B0AC7 File Offset: 0x003AECC7
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsage(this.innerContainer, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x0600A5A5 RID: 42405 RVA: 0x003B0AF4 File Offset: 0x003AECF4
		public CompLaunchable Launchable
		{
			get
			{
				CompLaunchable result;
				if ((result = this.cachedCompLaunchable) == null)
				{
					result = (this.cachedCompLaunchable = this.parent.GetComp<CompLaunchable>());
				}
				return result;
			}
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x0600A5A6 RID: 42406 RVA: 0x003B0B20 File Offset: 0x003AED20
		public CompShuttle Shuttle
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedCompShuttle) == null)
				{
					result = (this.cachedCompShuttle = this.parent.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x0600A5A7 RID: 42407 RVA: 0x003B0B4B File Offset: 0x003AED4B
		public virtual bool RequiresFuelingPort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x0600A5A8 RID: 42408 RVA: 0x003B0B50 File Offset: 0x003AED50
		public Thing FirstThingLeftToLoad
		{
			get
			{
				if (this.leftToLoad == null)
				{
					return null;
				}
				for (int i = 0; i < this.leftToLoad.Count; i++)
				{
					if (this.leftToLoad[i].CountToTransfer != 0 && this.leftToLoad[i].HasAnyThing)
					{
						return this.leftToLoad[i].AnyThing;
					}
				}
				return null;
			}
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x0600A5A9 RID: 42409 RVA: 0x003B0BB8 File Offset: 0x003AEDB8
		public Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return null;
				}
				for (int i = 0; i < list.Count; i++)
				{
					Thing firstThingLeftToLoad = list[i].FirstThingLeftToLoad;
					if (firstThingLeftToLoad != null)
					{
						return firstThingLeftToLoad;
					}
				}
				return null;
			}
		}

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x0600A5AA RID: 42410 RVA: 0x003B0C00 File Offset: 0x003AEE00
		public bool AnyInGroupNotifiedCantLoadMore
		{
			get
			{
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return false;
				}
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].notifiedCantLoadMore)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001CEB RID: 7403
		// (get) Token: 0x0600A5AB RID: 42411 RVA: 0x003B0C48 File Offset: 0x003AEE48
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.AnythingLeftToLoad)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = this.parent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						CompTransporter transporter = ((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter != null && transporter.groupID == this.groupID)
						{
							return true;
						}
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterTransporter)
					{
						CompTransporter transporter2 = ((JobDriver_EnterTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter;
						if (transporter2 != null && transporter2.groupID == this.groupID)
						{
							return true;
						}
					}
				}
				List<CompTransporter> list = this.TransportersInGroup(this.parent.Map);
				if (list == null)
				{
					return false;
				}
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].mindState.duty != null && allPawnsSpawned[j].mindState.duty.transportersGroup == this.groupID)
					{
						CompTransporter compTransporter = JobGiver_EnterTransporter.FindMyTransporter(list, allPawnsSpawned[j]);
						if (compTransporter != null && allPawnsSpawned[j].CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return true;
						}
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (LoadTransportersJobUtility.HasJobOnTransporter(allPawnsSpawned[k], list[l]))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600A5AC RID: 42412 RVA: 0x003B0E0C File Offset: 0x003AF00C
		public CompTransporter()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600A5AD RID: 42413 RVA: 0x003B0E5C File Offset: 0x003AF05C
		public override void PostExposeData()
		{
			base.PostExposeData();
			bool flag = !this.parent.SpawnedOrAnyParentSpawned;
			if (flag && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
						if (!pawn.IsWorldPawn())
						{
							string str = "Trying to save a non-world pawn (";
							Pawn pawn2 = pawn;
							Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null) + ") as a reference in a transporter.");
						}
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.notifiedCantLoadMore, "notifiedCantLoadMore", false, false);
			Scribe_Values.Look<float>(ref this.massCapacityOverride, "massCapacityOverride", 0f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				List<TransferableOneWay> list = this.leftToLoad;
				if (list != null)
				{
					list.RemoveWhere((TransferableOneWay t) => t == null);
				}
			}
			if (flag && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x0600A5AE RID: 42414 RVA: 0x003B1033 File Offset: 0x003AF233
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600A5AF RID: 42415 RVA: 0x003B103B File Offset: 0x003AF23B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600A5B0 RID: 42416 RVA: 0x003B104C File Offset: 0x003AF24C
		public override void CompTick()
		{
			if (this.Props.shouldTickContents)
			{
				this.innerContainer.DoTick();
			}
			else if (this.Props.restEffectiveness != 0f)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null && !pawn.Dead && pawn.needs.rest != null)
					{
						pawn.needs.rest.TickResting(this.Props.restEffectiveness);
					}
				}
			}
			if (this.parent.IsHashIntervalTick(60) && this.parent.Spawned)
			{
				CompShuttle shuttle = this.Shuttle;
				if ((shuttle == null || !shuttle.Autoload) && this.LoadingInProgressOrReadyToLaunch && this.AnyInGroupHasAnythingLeftToLoad && !this.AnyInGroupNotifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow)
				{
					this.notifiedCantLoadMore = true;
					if (this.Shuttle != null)
					{
						Messages.Message("MessageCantLoadMoreIntoShuttle".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
						return;
					}
					Messages.Message("MessageCantLoadMoreIntoTransporters".Translate(this.FirstThingLeftToLoadInGroup.LabelNoCount, Faction.OfPlayer.def.pawnsPlural, this.FirstThingLeftToLoadInGroup), this.parent, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600A5B1 RID: 42417 RVA: 0x003B11FC File Offset: 0x003AF3FC
		public List<CompTransporter> TransportersInGroup(Map map)
		{
			CompTransporter.tmpTransportersInGroup.Clear();
			if (!this.parent.Spawned)
			{
				CompTransporter.tmpTransportersInGroup.Add(this);
				return CompTransporter.tmpTransportersInGroup;
			}
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			TransporterUtility.GetTransportersInGroup(this.groupID, map, CompTransporter.tmpTransportersInGroup);
			return CompTransporter.tmpTransportersInGroup;
		}

		// Token: 0x0600A5B2 RID: 42418 RVA: 0x003B1251 File Offset: 0x003AF451
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTransporter.<CompGetGizmosExtra>d__57 <CompGetGizmosExtra>d__ = new CompTransporter.<CompGetGizmosExtra>d__57(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A5B3 RID: 42419 RVA: 0x003B1264 File Offset: 0x003AF464
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.Launchable != null && this.Launchable.lastLaunchTick == Find.TickManager.TicksGame)
			{
				return;
			}
			if (this.CancelLoad(map) && this.Shuttle == null)
			{
				if (!this.Groupable)
				{
					Messages.Message("MessageTransporterSingleLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
				else
				{
					Messages.Message("MessageTransportersLoadCanceled_TransporterDestroyed".Translate(), MessageTypeDefOf.NegativeEvent, true);
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x0600A5B4 RID: 42420 RVA: 0x003B1318 File Offset: 0x003AF518
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Contents".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst());
			if (this.Props.showMassInInspectString)
			{
				TaggedString taggedString = "Mass".Translate() + ": " + this.MassUsage.ToString("F0") + " / " + this.MassCapacity.ToString("F0") + " kg";
				stringBuilder.AppendLine().Append((this.MassUsage > this.MassCapacity) ? taggedString.Colorize(ColorLibrary.RedReadable) : taggedString);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A5B5 RID: 42421 RVA: 0x003B13FC File Offset: 0x003AF5FC
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || count <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null)
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					if (!transferableOneWay.things.Contains(t.things[i]))
					{
						transferableOneWay.things.Add(t.things[i]);
					}
				}
				if (transferableOneWay.CanAdjustBy(count).Accepted)
				{
					transferableOneWay.AdjustBy(count);
					return;
				}
			}
			else
			{
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				this.leftToLoad.Add(transferableOneWay2);
				transferableOneWay2.things.AddRange(t.things);
				transferableOneWay2.AdjustTo(count);
			}
		}

		// Token: 0x0600A5B6 RID: 42422 RVA: 0x003B14C8 File Offset: 0x003AF6C8
		public bool LeftToLoadContains(Thing thing)
		{
			if (this.leftToLoad == null)
			{
				return false;
			}
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				for (int j = 0; j < this.leftToLoad[i].things.Count; j++)
				{
					if (this.leftToLoad[i].things[j] == thing)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A5B7 RID: 42423 RVA: 0x003B1534 File Offset: 0x003AF734
		public void Notify_ThingAdded(Thing t)
		{
			try
			{
				this.SubtractFromToLoadList(t, t.stackCount, true);
				if (this.parent.Spawned && this.Props.pawnLoadedSound != null && t is Pawn)
				{
					this.Props.pawnLoadedSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				Pawn pawn = t as Pawn;
				if (pawn != null && pawn.IsFormingCaravan())
				{
					pawn.GetLord().Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
				}
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "ThingAdded", t.Named("SUBJECT"));
				this.massUsageDirty = true;
			}
			catch (Exception ex)
			{
				string str = "Exception in Notify_ThingAdded: ";
				Exception ex2 = ex;
				Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x0600A5B8 RID: 42424 RVA: 0x003B1624 File Offset: 0x003AF824
		public void Notify_ThingRemoved(Thing t)
		{
			if (this.Props.pawnExitSound != null && t is Pawn)
			{
				this.Props.pawnExitSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
			this.massUsageDirty = true;
		}

		// Token: 0x0600A5B9 RID: 42425 RVA: 0x003B167E File Offset: 0x003AF87E
		public void Notify_ThingAddedAndMergedWith(Thing t, int mergedCount)
		{
			this.SubtractFromToLoadList(t, mergedCount, true);
			this.massUsageDirty = true;
		}

		// Token: 0x0600A5BA RID: 42426 RVA: 0x003B1691 File Offset: 0x003AF891
		public bool CancelLoad()
		{
			CompShuttle shuttle = this.Shuttle;
			if (((shuttle != null) ? shuttle.shipParent : null) != null)
			{
				this.Shuttle.shipParent.ForceJob_DelayCurrent(ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload));
			}
			return this.CancelLoad(this.Map);
		}

		// Token: 0x0600A5BB RID: 42427 RVA: 0x003B16D0 File Offset: 0x003AF8D0
		public bool CancelLoad(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return false;
			}
			this.TryRemoveLord(map);
			List<CompTransporter> list = this.TransportersInGroup(map);
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CleanUpLoadingVars(map);
			}
			this.CleanUpLoadingVars(map);
			return true;
		}

		// Token: 0x0600A5BC RID: 42428 RVA: 0x003B1724 File Offset: 0x003AF924
		public void TryRemoveLord(Map map)
		{
			if (!this.LoadingInProgressOrReadyToLaunch)
			{
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, map);
			if (lord != null)
			{
				foreach (Pawn pawn in lord.ownedPawns)
				{
					if (pawn.IsColonist && map.IsPlayerHome)
					{
						pawn.inventory.UnloadEverything = true;
					}
				}
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x0600A5BD RID: 42429 RVA: 0x003B17B4 File Offset: 0x003AF9B4
		public void CleanUpLoadingVars(Map map)
		{
			this.groupID = -1;
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			List<TransferableOneWay> list = this.leftToLoad;
			if (list != null)
			{
				list.Clear();
			}
			CompShuttle shuttle = this.Shuttle;
			if (shuttle != null)
			{
				shuttle.CleanUpLoadingVars();
			}
			this.notifiedCantLoadMore = false;
			this.massUsageDirty = true;
		}

		// Token: 0x0600A5BE RID: 42430 RVA: 0x003B1814 File Offset: 0x003AFA14
		public int SubtractFromToLoadList(Thing t, int count, bool sendMessageOnFinished = true)
		{
			if (this.leftToLoad == null)
			{
				return 0;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				return 0;
			}
			if (transferableOneWay.CountToTransfer <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
			transferableOneWay.AdjustBy(-num);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			if (sendMessageOnFinished && !this.AnyInGroupHasAnythingLeftToLoad)
			{
				CompShuttle comp = this.parent.GetComp<CompShuttle>();
				if (comp == null || comp.AllRequiredThingsLoaded)
				{
					if (comp != null)
					{
						Messages.Message("MessageFinishedLoadingShuttle".Translate(this.parent.Named("SHUTTLE")), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
					else
					{
						Messages.Message("MessageFinishedLoadingTransporters".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					}
				}
			}
			return num;
		}

		// Token: 0x0600A5BF RID: 42431 RVA: 0x003B18F8 File Offset: 0x003AFAF8
		private void SelectPreviousInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list != null)
			{
				int num = list.IndexOf(this);
				CameraJumper.TryJumpAndSelect(list[GenMath.PositiveMod(num - 1, list.Count)].parent, CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x0600A5C0 RID: 42432 RVA: 0x003B1944 File Offset: 0x003AFB44
		private void SelectAllInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list == null)
			{
				return;
			}
			Selector selector = Find.Selector;
			selector.ClearSelection();
			for (int i = 0; i < list.Count; i++)
			{
				selector.Select(list[i].parent, true, true);
			}
		}

		// Token: 0x0600A5C1 RID: 42433 RVA: 0x003B1994 File Offset: 0x003AFB94
		private void SelectNextInGroup()
		{
			List<CompTransporter> list = this.TransportersInGroup(this.Map);
			if (list != null)
			{
				int num = list.IndexOf(this);
				CameraJumper.TryJumpAndSelect(list[(num + 1) % list.Count].parent, CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x040061C5 RID: 25029
		public int groupID = -1;

		// Token: 0x040061C6 RID: 25030
		public ThingOwner innerContainer;

		// Token: 0x040061C7 RID: 25031
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x040061C8 RID: 25032
		private bool notifiedCantLoadMore;

		// Token: 0x040061C9 RID: 25033
		public float massCapacityOverride = -1f;

		// Token: 0x040061CA RID: 25034
		private CompLaunchable cachedCompLaunchable;

		// Token: 0x040061CB RID: 25035
		private CompShuttle cachedCompShuttle;

		// Token: 0x040061CC RID: 25036
		private bool massUsageDirty = true;

		// Token: 0x040061CD RID: 25037
		private float cachedMassUsage;

		// Token: 0x040061CE RID: 25038
		public static readonly Texture2D CancelLoadCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x040061CF RID: 25039
		private static readonly Texture2D LoadCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LoadTransporter", true);

		// Token: 0x040061D0 RID: 25040
		private static readonly Texture2D SelectPreviousInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectPreviousTransporter", true);

		// Token: 0x040061D1 RID: 25041
		private static readonly Texture2D SelectAllInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllTransporters", true);

		// Token: 0x040061D2 RID: 25042
		private static readonly Texture2D SelectNextInGroupCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectNextTransporter", true);

		// Token: 0x040061D3 RID: 25043
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x040061D4 RID: 25044
		private List<Pawn> tmpSavedPawns = new List<Pawn>();

		// Token: 0x040061D5 RID: 25045
		private static List<CompTransporter> tmpTransportersInGroup = new List<CompTransporter>();
	}
}
