using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182A RID: 6186
	public class CompBiocodable : ThingComp
	{
		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x06009AB6 RID: 39606 RVA: 0x0037CF05 File Offset: 0x0037B105
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x06009AB7 RID: 39607 RVA: 0x0037CF0D File Offset: 0x0037B10D
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x06009AB8 RID: 39608 RVA: 0x0037CF15 File Offset: 0x0037B115
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x06009AB9 RID: 39609 RVA: 0x0037CF1D File Offset: 0x0037B11D
		public CompProperties_Biocodable Props
		{
			get
			{
				return (CompProperties_Biocodable)this.props;
			}
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x06009ABA RID: 39610 RVA: 0x0037CF2A File Offset: 0x0037B12A
		public virtual bool Biocodable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009ABB RID: 39611 RVA: 0x0037CF30 File Offset: 0x0037B130
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		// Token: 0x06009ABC RID: 39612 RVA: 0x0037CF50 File Offset: 0x0037B150
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		// Token: 0x06009ABD RID: 39613 RVA: 0x0037CF72 File Offset: 0x0037B172
		public virtual void CodeFor(Pawn p)
		{
			if (!this.Biocodable)
			{
				return;
			}
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
			this.OnCodedFor(p);
		}

		// Token: 0x06009ABE RID: 39614 RVA: 0x0037CFA3 File Offset: 0x0037B1A3
		protected virtual void OnCodedFor(Pawn p)
		{
		}

		// Token: 0x06009ABF RID: 39615 RVA: 0x0037CFA5 File Offset: 0x0037B1A5
		public virtual void UnCode()
		{
			this.biocoded = false;
			this.codedPawn = null;
			this.codedPawnLabel = null;
		}

		// Token: 0x06009AC0 RID: 39616 RVA: 0x0037CFBC File Offset: 0x0037B1BC
		public override void Notify_Equipped(Pawn pawn)
		{
			if (this.Biocodable && this.Props.biocodeOnEquip)
			{
				this.CodeFor(pawn);
			}
		}

		// Token: 0x06009AC1 RID: 39617 RVA: 0x0037CFDC File Offset: 0x0037B1DC
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		// Token: 0x06009AC2 RID: 39618 RVA: 0x0037D01C File Offset: 0x0037B21C
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x06009AC3 RID: 39619 RVA: 0x0037D05B File Offset: 0x0037B25B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompBiocodable.<SpecialDisplayStats>d__21 <SpecialDisplayStats>d__ = new CompBiocodable.<SpecialDisplayStats>d__21(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06009AC4 RID: 39620 RVA: 0x0037D06C File Offset: 0x0037B26C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.codedPawn != null && this.codedPawn.Discarded)
			{
				this.codedPawn = null;
			}
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		// Token: 0x04005C05 RID: 23557
		protected bool biocoded;

		// Token: 0x04005C06 RID: 23558
		protected string codedPawnLabel;

		// Token: 0x04005C07 RID: 23559
		protected Pawn codedPawn;
	}
}
