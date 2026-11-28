using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C0 RID: 6080
	public abstract class ScenPart_ThingCount : ScenPart
	{
		// Token: 0x060097A3 RID: 38819 RVA: 0x0036FD38 File Offset: 0x0036DF38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x060097A4 RID: 38820 RVA: 0x0036FD98 File Offset: 0x0036DF98
		public override void Randomize()
		{
			this.thingDef = this.PossibleThingDefs().RandomElement<ThingDef>();
			this.stuff = GenStuff.RandomStuffFor(this.thingDef);
			if (this.thingDef.statBases.StatListContains(StatDefOf.MarketValue))
			{
				float num = (float)Rand.Range(200, 2000);
				float statValueAbstract = this.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, this.stuff);
				this.count = Mathf.CeilToInt(num / statValueAbstract);
				return;
			}
			this.count = Rand.RangeInclusive(1, 100);
		}

		// Token: 0x060097A5 RID: 38821 RVA: 0x0036FE24 File Offset: 0x0036E024
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 4f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 4f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 4f, scenPartRect.width, scenPartRect.height / 4f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 4f, scenPartRect.width, scenPartRect.height / 4f);
			Rect rect4 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 3f / 4f, scenPartRect.width, scenPartRect.height / 4f);
			if (Widgets.ButtonText(rect, this.thingDef.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleThingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.thingDef = localTd;
						this.stuff = GenStuff.DefaultStuffFor(localTd);
						this.quality = null;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (this.thingDef.MadeFromStuff && Widgets.ButtonText(rect2, this.stuff.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (ThingDef localSd2 in from t in GenStuff.AllowedStuffsFor(this.thingDef, TechLevel.Undefined, false)
				orderby t.label
				select t)
				{
					ThingDef localSd = localSd2;
					list2.Add(new FloatMenuOption(localSd.LabelCap, delegate()
					{
						this.stuff = localSd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (this.thingDef.HasComp(typeof(CompQuality)))
			{
				string str = (this.quality != null) ? this.quality.Value.GetLabel() : "Default".Translate().ToString();
				if (Widgets.ButtonText(rect3, str.CapitalizeFirst(), true, true, true, null))
				{
					List<FloatMenuOption> list3 = new List<FloatMenuOption>
					{
						new FloatMenuOption("Default".Translate().CapitalizeFirst(), delegate()
						{
							this.quality = null;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
					};
					foreach (QualityCategory qualityCategory in QualityUtility.AllQualityCategories)
					{
						QualityCategory localQ = qualityCategory;
						list3.Add(new FloatMenuOption(qualityCategory.GetLabel().CapitalizeFirst(), delegate()
						{
							this.quality = new QualityCategory?(localQ);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list3));
				}
			}
			Widgets.TextFieldNumeric<int>(rect4, ref this.count, ref this.countBuf, 1f, 1E+09f);
		}

		// Token: 0x060097A6 RID: 38822 RVA: 0x00370278 File Offset: 0x0036E478
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ThingCount scenPart_ThingCount = other as ScenPart_ThingCount;
			if (scenPart_ThingCount != null && base.GetType() == scenPart_ThingCount.GetType() && this.thingDef == scenPart_ThingCount.thingDef && this.stuff == scenPart_ThingCount.stuff)
			{
				QualityCategory? qualityCategory = this.quality;
				QualityCategory? qualityCategory2 = scenPart_ThingCount.quality;
				if ((qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null)) && this.count >= 0 && scenPart_ThingCount.count >= 0)
				{
					this.count += scenPart_ThingCount.count;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060097A7 RID: 38823 RVA: 0x0037031A File Offset: 0x0036E51A
		protected virtual IEnumerable<ThingDef> PossibleThingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop) || (d.category == ThingCategory.Building && d.Minifiable)
			select d;
		}

		// Token: 0x060097A8 RID: 38824 RVA: 0x00370345 File Offset: 0x0036E545
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.thingDef == null || (this.thingDef.MadeFromStuff && this.stuff == null);
		}

		// Token: 0x060097A9 RID: 38825 RVA: 0x00370374 File Offset: 0x0036E574
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.thingDef != null)
			{
				num ^= this.thingDef.GetHashCode();
			}
			if (this.stuff != null)
			{
				num ^= this.stuff.GetHashCode();
			}
			num ^= this.count;
			return num ^ this.quality.GetHashCode();
		}

		// Token: 0x04005AC3 RID: 23235
		protected ThingDef thingDef;

		// Token: 0x04005AC4 RID: 23236
		protected ThingDef stuff;

		// Token: 0x04005AC5 RID: 23237
		protected int count = 1;

		// Token: 0x04005AC6 RID: 23238
		protected QualityCategory? quality;

		// Token: 0x04005AC7 RID: 23239
		private string countBuf;
	}
}
