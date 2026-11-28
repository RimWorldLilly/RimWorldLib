using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EA5 RID: 3749
	public abstract class BookOutcomeDoer : ReadingOutcomeDoer
	{
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060062BD RID: 25277 RVA: 0x00239050 File Offset: 0x00237250
		public new BookOutcomeProperties Props
		{
			get
			{
				return (BookOutcomeProperties)this.props;
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060062BE RID: 25278 RVA: 0x0023905D File Offset: 0x0023725D
		public Book Book
		{
			get
			{
				return (Book)base.Parent;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060062BF RID: 25279 RVA: 0x0023906C File Offset: 0x0023726C
		private CompQuality CompQuality
		{
			get
			{
				CompQuality result;
				if ((result = this.compQualityCached) == null)
				{
					result = (this.compQualityCached = base.Parent.GetComp<CompQuality>());
				}
				return result;
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060062C0 RID: 25280 RVA: 0x00239097 File Offset: 0x00237297
		public QualityCategory Quality
		{
			get
			{
				return this.CompQuality.Quality;
			}
		}

		// Token: 0x060062C1 RID: 25281
		public abstract bool DoesProvidesOutcome(Pawn reader);

		// Token: 0x060062C2 RID: 25282 RVA: 0x002390A4 File Offset: 0x002372A4
		public virtual void OnBookGenerated(Pawn author = null)
		{
		}

		// Token: 0x060062C3 RID: 25283 RVA: 0x002390A6 File Offset: 0x002372A6
		public virtual string GetBenefitsString(Pawn reader = null)
		{
			return "";
		}

		// Token: 0x060062C4 RID: 25284 RVA: 0x002390AD File Offset: 0x002372AD
		public virtual bool BenefitDetailsCanChange(Pawn reader = null)
		{
			return false;
		}

		// Token: 0x060062C5 RID: 25285 RVA: 0x002390B0 File Offset: 0x002372B0
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			return new BookOutcomeDoer.<GetHyperlinks>d__13(-2);
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x002390B9 File Offset: 0x002372B9
		public virtual IEnumerable<RulePack> GetTopicRulePacks()
		{
			return null;
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x002390BC File Offset: 0x002372BC
		public virtual IEnumerable<Rule_String> GetTopicRuleStrings()
		{
			return null;
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x002390BF File Offset: 0x002372BF
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x002390C6 File Offset: 0x002372C6
		public virtual void Reset()
		{
		}

		// Token: 0x0400367F RID: 13951
		private CompQuality compQualityCached;
	}
}
