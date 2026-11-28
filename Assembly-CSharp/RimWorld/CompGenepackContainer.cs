using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A0 RID: 6304
	public class CompGenepackContainer : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x06009DDB RID: 40411 RVA: 0x0038B1D4 File Offset: 0x003893D4
		public CompProperties_GenepackContainer Props
		{
			get
			{
				return (CompProperties_GenepackContainer)this.props;
			}
		}

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x06009DDC RID: 40412 RVA: 0x0038B1E1 File Offset: 0x003893E1
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x06009DDD RID: 40413 RVA: 0x0038B1E4 File Offset: 0x003893E4
		public bool PowerOn
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x06009DDE RID: 40414 RVA: 0x0038B1F6 File Offset: 0x003893F6
		public bool Full
		{
			get
			{
				return this.ContainedGenepacks.Count >= this.Props.maxCapacity;
			}
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x06009DDF RID: 40415 RVA: 0x0038B213 File Offset: 0x00389413
		public bool CanLoadMore
		{
			get
			{
				return !this.Full && this.ContainedGenepacks.Count + this.leftToLoad.Count < this.Props.maxCapacity;
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x06009DE0 RID: 40416 RVA: 0x0038B243 File Offset: 0x00389443
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x06009DE1 RID: 40417 RVA: 0x0038B24C File Offset: 0x0038944C
		public List<Genepack> ContainedGenepacks
		{
			get
			{
				this.tmpGenepacks.Clear();
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Genepack genepack = this.innerContainer[i] as Genepack;
					if (genepack != null && genepack.GeneSet.GenesListForReading.Any<GeneDef>())
					{
						this.tmpGenepacks.Add(genepack);
					}
				}
				return this.tmpGenepacks;
			}
		}

		// Token: 0x06009DE2 RID: 40418 RVA: 0x0038B2B3 File Offset: 0x003894B3
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Genepack container"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06009DE3 RID: 40419 RVA: 0x0038B2E0 File Offset: 0x003894E0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06009DE4 RID: 40420 RVA: 0x0038B2EE File Offset: 0x003894EE
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06009DE5 RID: 40421 RVA: 0x0038B2F6 File Offset: 0x003894F6
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x06009DE6 RID: 40422 RVA: 0x0038B30C File Offset: 0x0038950C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.EjectContents(map);
			}
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				((Genepack)this.leftToLoad[i]).targetContainer = null;
			}
			this.leftToLoad.Clear();
		}

		// Token: 0x06009DE7 RID: 40423 RVA: 0x0038B35C File Offset: 0x0038955C
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				if (this.leftToLoad[i].Map == this.parent.Map)
				{
					GenDraw.DrawLineBetween(this.parent.DrawPos, this.leftToLoad[i].DrawPos);
				}
			}
		}

		// Token: 0x06009DE8 RID: 40424 RVA: 0x0038B3C0 File Offset: 0x003895C0
		public void EjectContents(Map destMap = null)
		{
			if (destMap == null)
			{
				destMap = this.parent.Map;
			}
			IntVec3 dropLoc = this.parent.def.hasInteractionCell ? this.parent.InteractionCell : this.parent.Position;
			this.innerContainer.TryDropAll(dropLoc, destMap, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x06009DE9 RID: 40425 RVA: 0x0038B41C File Offset: 0x0038961C
		public override void CompTickRare()
		{
			if (this.innerContainer != null)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					this.innerContainer[i].TickRare();
				}
			}
		}

		// Token: 0x06009DEA RID: 40426 RVA: 0x0038B458 File Offset: 0x00389658
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGenepackContainer.<CompGetGizmosExtra>d__27 <CompGetGizmosExtra>d__ = new CompGenepackContainer.<CompGetGizmosExtra>d__27(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009DEB RID: 40427 RVA: 0x0038B468 File Offset: 0x00389668
		public override string CompInspectStringExtra()
		{
			TaggedString t = "GenepacksStored".Translate();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.innerContainer.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Props.maxCapacity);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			return t + defaultInterpolatedStringHandler.ToStringAndClear() + "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
		}

		// Token: 0x06009DEC RID: 40428 RVA: 0x0038B510 File Offset: 0x00389710
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<Thing>(ref this.leftToLoad, "leftToLoad", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
		}

		// Token: 0x04005D6D RID: 23917
		public ThingOwner innerContainer;

		// Token: 0x04005D6E RID: 23918
		public List<Thing> leftToLoad = new List<Thing>();

		// Token: 0x04005D6F RID: 23919
		public bool autoLoad = true;

		// Token: 0x04005D70 RID: 23920
		[Unsaved(false)]
		private List<Genepack> tmpGenepacks = new List<Genepack>();

		// Token: 0x04005D71 RID: 23921
		private static readonly CachedTexture EjectTex = new CachedTexture("UI/Gizmos/EjectAll");
	}
}
