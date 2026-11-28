using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A7 RID: 6567
	public class CompStyleable : ThingComp
	{
		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x0600A4DD RID: 42205 RVA: 0x003AE412 File Offset: 0x003AC612
		public Ideo Ideo
		{
			get
			{
				Precept_ThingStyle precept_ThingStyle = this.SourcePrecept;
				if (precept_ThingStyle == null)
				{
					return null;
				}
				return precept_ThingStyle.ideo;
			}
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x0600A4DE RID: 42206 RVA: 0x003AE428 File Offset: 0x003AC628
		public StyleCategoryDef StyleCategoryDef
		{
			get
			{
				if (this.cachedStyleCategoryDef == null)
				{
					bool flag = false;
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						for (int i = 0; i < styleCategoryDef.thingDefStyles.Count; i++)
						{
							if (styleCategoryDef.thingDefStyles[i].StyleDef == this.styleDef && styleCategoryDef.thingDefStyles[i].ThingDef == this.parent.def)
							{
								this.cachedStyleCategoryDef = styleCategoryDef;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				return this.cachedStyleCategoryDef;
			}
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x0600A4DF RID: 42207 RVA: 0x003AE4E0 File Offset: 0x003AC6E0
		// (set) Token: 0x0600A4E0 RID: 42208 RVA: 0x003AE4E8 File Offset: 0x003AC6E8
		public Precept_ThingStyle SourcePrecept
		{
			get
			{
				return this.sourcePrecept;
			}
			set
			{
				if (this.sourcePrecept == value)
				{
					return;
				}
				this.sourcePrecept = value;
				if (this.parent.def.randomStyleChance > 0f)
				{
					return;
				}
				BuildableDef buildableDef = this.parent.def.IsBlueprint ? this.parent.def.entityDefToBuild : this.parent.def;
				Precept_ThingStyle precept_ThingStyle = this.sourcePrecept;
				StyleCategoryPair styleCategoryPair = (precept_ThingStyle != null) ? precept_ThingStyle.ideo.style.StyleForThingDef((ThingDef)buildableDef, this.sourcePrecept) : null;
				this.cachedStyleCategoryDef = ((styleCategoryPair != null) ? styleCategoryPair.category : null);
				if (this.cachedStyleCategoryDef != null)
				{
					this.styleDef = styleCategoryPair.styleDef;
				}
			}
		}

		// Token: 0x0600A4E1 RID: 42209 RVA: 0x003AE59D File Offset: 0x003AC79D
		public override string TransformLabel(string label)
		{
			if (this.sourcePrecept != null)
			{
				label = this.sourcePrecept.TransformThingLabel(label);
			}
			return label;
		}

		// Token: 0x0600A4E2 RID: 42210 RVA: 0x003AE5B8 File Offset: 0x003AC7B8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.sourcePrecept == null)
			{
				return text;
			}
			string text2 = this.sourcePrecept.InspectStringExtra(this.parent);
			if (text2.NullOrEmpty())
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + text2;
		}

		// Token: 0x0600A4E3 RID: 42211 RVA: 0x003AE610 File Offset: 0x003AC810
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			StyleCategoryPair styleCategoryPair;
			if (forFaction == null)
			{
				styleCategoryPair = null;
			}
			else
			{
				FactionIdeosTracker ideos = forFaction.ideos;
				if (ideos == null)
				{
					styleCategoryPair = null;
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					if (primaryIdeo == null)
					{
						styleCategoryPair = null;
					}
					else
					{
						IdeoStyleTracker style = primaryIdeo.style;
						styleCategoryPair = ((style != null) ? style.StyleForThingDef(this.parent.def, null) : null);
					}
				}
			}
			StyleCategoryPair styleCategoryPair2 = styleCategoryPair;
			if (styleCategoryPair2 != null)
			{
				this.parent.SetStyleDef(styleCategoryPair2.styleDef);
			}
		}

		// Token: 0x0600A4E4 RID: 42212 RVA: 0x003AE66E File Offset: 0x003AC86E
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompStyleable.<SpecialDisplayStats>d__14 <SpecialDisplayStats>d__ = new CompStyleable.<SpecialDisplayStats>d__14(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A4E5 RID: 42213 RVA: 0x003AE680 File Offset: 0x003AC880
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.sourcePrecept != null && (this.sourcePrecept.ideo == null || !Find.IdeoManager.IdeosListForReading.Contains(this.sourcePrecept.ideo)))
			{
				this.sourcePrecept = null;
			}
			Scribe_References.Look<Precept_ThingStyle>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", false, false);
		}

		// Token: 0x04006171 RID: 24945
		private Precept_ThingStyle sourcePrecept;

		// Token: 0x04006172 RID: 24946
		public ThingStyleDef styleDef;

		// Token: 0x04006173 RID: 24947
		public bool everSeenByPlayer;

		// Token: 0x04006174 RID: 24948
		[Unsaved(false)]
		public StyleCategoryDef cachedStyleCategoryDef;
	}
}
