using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A5F RID: 6751
	[StaticConstructorOnStartup]
	public class MapPortal : Building, IThingHolder
	{
		// Token: 0x17001DD2 RID: 7634
		// (get) Token: 0x0600A982 RID: 43394 RVA: 0x003C17CC File Offset: 0x003BF9CC
		public Map PocketMap
		{
			get
			{
				Map map = this.pocketMap;
				bool flag;
				if (map == null)
				{
					flag = false;
				}
				else
				{
					MapParent parent = map.Parent;
					bool? flag2 = (parent != null) ? new bool?(parent.HasMap) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					this.pocketMap = null;
				}
				return this.pocketMap;
			}
		}

		// Token: 0x17001DD3 RID: 7635
		// (get) Token: 0x0600A983 RID: 43395 RVA: 0x003C1828 File Offset: 0x003BFA28
		public bool PocketMapExists
		{
			get
			{
				return this.PocketMap != null;
			}
		}

		// Token: 0x17001DD4 RID: 7636
		// (get) Token: 0x0600A984 RID: 43396 RVA: 0x003C1833 File Offset: 0x003BFA33
		public virtual bool AutoDraftOnEnter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DD5 RID: 7637
		// (get) Token: 0x0600A985 RID: 43397 RVA: 0x003C1836 File Offset: 0x003BFA36
		protected virtual Texture2D EnterTex
		{
			get
			{
				return MapPortal.DefaultEnterTex;
			}
		}

		// Token: 0x17001DD6 RID: 7638
		// (get) Token: 0x0600A986 RID: 43398 RVA: 0x003C183D File Offset: 0x003BFA3D
		public virtual string EnterString
		{
			get
			{
				return "EnterPortal".Translate(this.Label);
			}
		}

		// Token: 0x17001DD7 RID: 7639
		// (get) Token: 0x0600A987 RID: 43399 RVA: 0x003C1859 File Offset: 0x003BFA59
		public virtual string CancelEnterString
		{
			get
			{
				return "CommandCancelEnterPortal".Translate();
			}
		}

		// Token: 0x17001DD8 RID: 7640
		// (get) Token: 0x0600A988 RID: 43400 RVA: 0x003C186A File Offset: 0x003BFA6A
		public virtual string EnteringString
		{
			get
			{
				return "EnteringPortal".Translate(this.Label);
			}
		}

		// Token: 0x17001DD9 RID: 7641
		// (get) Token: 0x0600A989 RID: 43401 RVA: 0x003C1886 File Offset: 0x003BFA86
		public bool LoadInProgress
		{
			get
			{
				return this.leftToLoad != null && this.leftToLoad.Any<TransferableOneWay>();
			}
		}

		// Token: 0x17001DDA RID: 7642
		// (get) Token: 0x0600A98A RID: 43402 RVA: 0x003C18A0 File Offset: 0x003BFAA0
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.LoadInProgress)
				{
					return false;
				}
				if (!base.Spawned)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToPortal && ((JobDriver_HaulToPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
					{
						return true;
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterPortal && ((JobDriver_EnterPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
					{
						return true;
					}
				}
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					Pawn_MindState mindState = allPawnsSpawned[j].mindState;
					Thing thing;
					if (mindState == null)
					{
						thing = null;
					}
					else
					{
						PawnDuty duty = mindState.duty;
						thing = ((duty != null) ? duty.focus.Thing : null);
					}
					Thing thing2 = thing;
					if (thing2 != null && thing2 == this && allPawnsSpawned[j].CanReach(thing2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						return true;
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist && EnterPortalUtility.HasJobOnPortal(allPawnsSpawned[k], this))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600A98B RID: 43403 RVA: 0x003C19E0 File Offset: 0x003BFBE0
		public override void ExposeData()
		{
			base.ExposeData();
			Map map = this.pocketMap;
			bool flag;
			if (map == null)
			{
				flag = false;
			}
			else
			{
				MapParent parent = map.Parent;
				bool? flag2 = (parent != null) ? new bool?(parent.HasMap) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				this.pocketMap = null;
			}
			Scribe_References.Look<Map>(ref this.pocketMap, "pocketMap", false);
			Scribe_References.Look<PocketMapExit>(ref this.exit, "exit", false);
			Scribe_Values.Look<bool>(ref this.beenEntered, "beenEntered", false, false);
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<TransferableOneWay> list = this.leftToLoad;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((TransferableOneWay x) => x.AnyThing == null);
			}
		}

		// Token: 0x0600A98C RID: 43404 RVA: 0x003C1ABE File Offset: 0x003BFCBE
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.containerProxy = new PortalContainerProxy
			{
				portal = this
			};
		}

		// Token: 0x0600A98D RID: 43405 RVA: 0x003C1ADC File Offset: 0x003BFCDC
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(60) && base.Spawned && this.LoadInProgress && !this.notifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow)
			{
				TransferableOneWay transferableOneWay = this.leftToLoad[0];
				if (((transferableOneWay != null) ? transferableOneWay.AnyThing : null) != null)
				{
					this.notifiedCantLoadMore = true;
					Messages.Message("MessageCantLoadMoreIntoPortal".Translate(this.Label, Faction.OfPlayer.def.pawnsPlural, this.leftToLoad[0].AnyThing), this, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600A98E RID: 43406 RVA: 0x003C1B98 File Offset: 0x003BFD98
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
		}

		// Token: 0x0600A98F RID: 43407 RVA: 0x003C1B9A File Offset: 0x003BFD9A
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.containerProxy;
		}

		// Token: 0x0600A990 RID: 43408 RVA: 0x003C1BA2 File Offset: 0x003BFDA2
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount);
		}

		// Token: 0x0600A991 RID: 43409 RVA: 0x003C1BB4 File Offset: 0x003BFDB4
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

		// Token: 0x0600A992 RID: 43410 RVA: 0x003C1C80 File Offset: 0x003BFE80
		public int SubtractFromToLoadList(Thing t, int count)
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
			transferableOneWay.things.Remove(t);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			return num;
		}

		// Token: 0x0600A993 RID: 43411 RVA: 0x003C1CF0 File Offset: 0x003BFEF0
		public void CancelLoad()
		{
			Lord lord = base.Map.lordManager.lords.FirstOrDefault(delegate(Lord l)
			{
				LordJob_LoadAndEnterPortal lordJob_LoadAndEnterPortal = l.LordJob as LordJob_LoadAndEnterPortal;
				return lordJob_LoadAndEnterPortal != null && lordJob_LoadAndEnterPortal.portal == this;
			});
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			this.leftToLoad.Clear();
		}

		// Token: 0x0600A994 RID: 43412 RVA: 0x003C1D40 File Offset: 0x003BFF40
		public virtual bool IsEnterable(out string reason)
		{
			foreach (ThingComp thingComp in base.AllComps)
			{
				AcceptanceReport acceptanceReport = thingComp.CanEnterPortal();
				if (!acceptanceReport.Accepted)
				{
					reason = acceptanceReport.Reason;
					return false;
				}
			}
			reason = "";
			return true;
		}

		// Token: 0x0600A995 RID: 43413 RVA: 0x003C1DB4 File Offset: 0x003BFFB4
		public virtual Map GetOtherMap()
		{
			if (this.PocketMap == null)
			{
				this.GeneratePocketMap();
			}
			return this.PocketMap;
		}

		// Token: 0x0600A996 RID: 43414 RVA: 0x003C1DCA File Offset: 0x003BFFCA
		public virtual IntVec3 GetDestinationLocation()
		{
			PocketMapExit pocketMapExit = this.exit;
			if (pocketMapExit == null)
			{
				return IntVec3.Invalid;
			}
			return pocketMapExit.Position;
		}

		// Token: 0x0600A997 RID: 43415 RVA: 0x003C1DE4 File Offset: 0x003BFFE4
		public virtual void OnEntered(Pawn pawn)
		{
			this.Notify_ThingAdded(pawn);
			if (!this.beenEntered)
			{
				this.beenEntered = true;
				if (!this.def.portal.enteredLetterLabel.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.def.portal.enteredLetterLabel, this.def.portal.enteredLetterText.Formatted(pawn.Named("PAWN")), this.def.portal.enteredLetterDef, this.exit, null, null, null, null, 0, true);
				}
			}
			if (Find.CurrentMap != base.Map)
			{
				if (Find.CurrentMap == this.exit.Map)
				{
					SoundDef traverseSound = this.def.portal.traverseSound;
					if (traverseSound == null)
					{
						return;
					}
					traverseSound.PlayOneShot(this.exit);
				}
				return;
			}
			SoundDef traverseSound2 = this.def.portal.traverseSound;
			if (traverseSound2 == null)
			{
				return;
			}
			traverseSound2.PlayOneShot(this);
		}

		// Token: 0x0600A998 RID: 43416 RVA: 0x003C1EE3 File Offset: 0x003C00E3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapPortal.<GetGizmos>d__40 <GetGizmos>d__ = new MapPortal.<GetGizmos>d__40(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A999 RID: 43417 RVA: 0x003C1EF3 File Offset: 0x003C00F3
		private void GeneratePocketMap()
		{
			PocketMapUtility.currentlyGeneratingPortal = this;
			this.pocketMap = this.GeneratePocketMapInt();
			PocketMapUtility.currentlyGeneratingPortal = null;
		}

		// Token: 0x0600A99A RID: 43418 RVA: 0x003C1F10 File Offset: 0x003C0110
		protected virtual Map GeneratePocketMapInt()
		{
			return PocketMapUtility.GeneratePocketMap(new IntVec3(this.def.portal.pocketMapSize, 1, this.def.portal.pocketMapSize), this.def.portal.pocketMapGenerator, this.GetExtraGenSteps(), base.Map);
		}

		// Token: 0x0600A99B RID: 43419 RVA: 0x003C1F64 File Offset: 0x003C0164
		protected virtual IEnumerable<GenStepWithParams> GetExtraGenSteps()
		{
			return Enumerable.Empty<GenStepWithParams>();
		}

		// Token: 0x04006329 RID: 25385
		private static readonly Texture2D ViewPocketMapTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewCave", true);

		// Token: 0x0400632A RID: 25386
		private static readonly Texture2D CancelEnterTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400632B RID: 25387
		private static readonly Texture2D DefaultEnterTex = ContentFinder<Texture2D>.Get("UI/Commands/EnterCave", true);

		// Token: 0x0400632C RID: 25388
		protected Map pocketMap;

		// Token: 0x0400632D RID: 25389
		public PocketMapExit exit;

		// Token: 0x0400632E RID: 25390
		protected bool beenEntered;

		// Token: 0x0400632F RID: 25391
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04006330 RID: 25392
		public PortalContainerProxy containerProxy;

		// Token: 0x04006331 RID: 25393
		public bool notifiedCantLoadMore;
	}
}
