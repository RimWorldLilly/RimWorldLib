using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001816 RID: 6166
	public class CompArt : ThingComp
	{
		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06009A11 RID: 39441 RVA: 0x0037A6F0 File Offset: 0x003788F0
		public TaggedString AuthorName
		{
			get
			{
				if (this.authorNameInt.NullOrEmpty())
				{
					return "UnknownLower".Translate().CapitalizeFirst();
				}
				return this.authorNameInt.Resolve();
			}
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06009A12 RID: 39442 RVA: 0x0037A730 File Offset: 0x00378930
		// (set) Token: 0x06009A13 RID: 39443 RVA: 0x0037A78D File Offset: 0x0037898D
		public string Title
		{
			get
			{
				if (this.parent.StyleSourcePrecept != null)
				{
					return this.parent.StyleSourcePrecept.LabelCap;
				}
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.");
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
			set
			{
				this.titleInt = value;
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x06009A14 RID: 39444 RVA: 0x0037A79B File Offset: 0x0037899B
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x06009A15 RID: 39445 RVA: 0x0037A7A4 File Offset: 0x003789A4
		public bool CanShowArt
		{
			get
			{
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = this.parent as Building_Grave;
					if (building_Grave == null || !building_Grave.HasCorpse)
					{
						return false;
					}
				}
				QualityCategory qualityCategory;
				return !this.parent.TryGetQuality(out qualityCategory) || qualityCategory >= this.Props.minQualityForArtistic;
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x06009A16 RID: 39446 RVA: 0x0037A7F9 File Offset: 0x003789F9
		public virtual bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x06009A17 RID: 39447 RVA: 0x0037A804 File Offset: 0x00378A04
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x06009A18 RID: 39448 RVA: 0x0037A811 File Offset: 0x00378A11
		public override string TransformLabel(string label)
		{
			if (this.Active && this.parent.StyleSourcePrecept != null)
			{
				return this.Title;
			}
			return base.TransformLabel(label);
		}

		// Token: 0x06009A19 RID: 39449 RVA: 0x0037A836 File Offset: 0x00378A36
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArtInternal(null, source);
		}

		// Token: 0x06009A1A RID: 39450 RVA: 0x0037A840 File Offset: 0x00378A40
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArtInternal(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x06009A1B RID: 39451 RVA: 0x0037A84C File Offset: 0x00378A4C
		protected virtual void InitializeArtInternal(Thing relatedThing, ArtGenerationContext source)
		{
			if (!this.titleInt.NullOrEmpty())
			{
				return;
			}
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
			if (this.CanShowArt)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (relatedThing != null)
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(relatedThing);
					}
					else
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArt(source);
					}
				}
				else
				{
					this.taleRef = TaleReference.Taleless;
				}
				this.titleInt = this.GenerateTitle(source);
				return;
			}
			this.titleInt = null;
			this.taleRef = null;
		}

		// Token: 0x06009A1C RID: 39452 RVA: 0x0037A8EB File Offset: 0x00378AEB
		public virtual void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.NameFullColored;
			}
		}

		// Token: 0x06009A1D RID: 39453 RVA: 0x0037A901 File Offset: 0x00378B01
		public void Clear()
		{
			this.authorNameInt = null;
			this.titleInt = null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06009A1E RID: 39454 RVA: 0x0037A938 File Offset: 0x00378B38
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<TaggedString>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<TaggedString>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", Array.Empty<object>());
		}

		// Token: 0x06009A1F RID: 39455 RVA: 0x0037A990 File Offset: 0x00378B90
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "Author".Translate() + ": " + this.AuthorName + ("\n" + "Title".Translate() + ": " + this.Title);
		}

		// Token: 0x06009A20 RID: 39456 RVA: 0x0037A9FE File Offset: 0x00378BFE
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06009A21 RID: 39457 RVA: 0x0037AA24 File Offset: 0x00378C24
		public override string GetDescriptionPart()
		{
			if (!this.Active)
			{
				return null;
			}
			return "" + this.Title + "\n\n" + this.GenerateImageDescription() + "\n\n" + ("Author".Translate() + ": " + this.AuthorName);
		}

		// Token: 0x06009A22 RID: 39458 RVA: 0x0037AA98 File Offset: 0x00378C98
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x06009A23 RID: 39459 RVA: 0x0037AAA8 File Offset: 0x00378CA8
		public virtual TaggedString GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				string str = "Did CompArt.GenerateImageDescription without initializing art: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker, null, null);
		}

		// Token: 0x06009A24 RID: 39460 RVA: 0x0037AB00 File Offset: 0x00378D00
		protected virtual string GenerateTitle(ArtGenerationContext context)
		{
			if (this.taleRef == null)
			{
				string str = "Did CompArt.GenerateTitle without initializing art: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker, null, null));
		}

		// Token: 0x04005BC4 RID: 23492
		private TaggedString authorNameInt = null;

		// Token: 0x04005BC5 RID: 23493
		protected TaggedString titleInt = null;

		// Token: 0x04005BC6 RID: 23494
		private TaleReference taleRef;
	}
}
