using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200164C RID: 5708
	public class Precept_ThingDef : Precept
	{
		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x06008F57 RID: 36695 RVA: 0x0034AE85 File Offset: 0x00349085
		public override string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.ThingDef.LabelCap);
			}
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x06008F58 RID: 36696 RVA: 0x0034AEC5 File Offset: 0x003490C5
		// (set) Token: 0x06008F59 RID: 36697 RVA: 0x0034AECD File Offset: 0x003490CD
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
			set
			{
				if (this.thingDef != value)
				{
					this.thingDef = value;
					this.Notify_ThingDefSet();
				}
			}
		}

		// Token: 0x06008F5A RID: 36698 RVA: 0x0034AEE8 File Offset: 0x003490E8
		public static List<StyleCategoryPair> AllPossibleStylesForBuilding(ThingDef building)
		{
			Precept_ThingDef.tmpStylesForBuilding.Clear();
			foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefsListForReading)
			{
				foreach (ThingDefStyle thingDefStyle in styleCategoryDef.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == building)
					{
						Precept_ThingDef.tmpStylesForBuilding.Add(new StyleCategoryPair
						{
							category = styleCategoryDef,
							styleDef = thingDefStyle.StyleDef
						});
					}
				}
			}
			return Precept_ThingDef.tmpStylesForBuilding;
		}

		// Token: 0x06008F5B RID: 36699 RVA: 0x0034AFA8 File Offset: 0x003491A8
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_ThingDef.usedThingDefsTmp.Clear();
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					if (precept != this)
					{
						Precept_ThingStyle precept_ThingStyle = precept as Precept_ThingStyle;
						if (precept_ThingStyle != null)
						{
							Precept_ThingDef.usedThingDefsTmp.Add(precept_ThingStyle.ThingDef);
						}
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo, generatingFor)
				where !Precept_ThingDef.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor);
			}
			if (this.ThingDef == null)
			{
				if (source.Any<PreceptThingChance>())
				{
					PreceptThingChance preceptThingChance;
					if (source.TryRandomElementByWeight((PreceptThingChance d) => d.chance, out preceptThingChance))
					{
						this.ThingDef = preceptThingChance.def;
						goto IL_16E;
					}
				}
				this.ThingDef = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
				Log.Warning("Failed to generate a unique building for " + ideo.name + " for precept " + this.def.defName);
			}
			IL_16E:
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.Notify_ThingDefSet();
		}

		// Token: 0x06008F5C RID: 36700 RVA: 0x0034B148 File Offset: 0x00349348
		protected virtual void Notify_ThingDefSet()
		{
			this.ideo.style.ResetStyleForThing(this.ThingDef);
			if (this.ThingDef.canEditAnyStyle && this.ideo.GetStyleAndCategoryFor(this.ThingDef) == null)
			{
				StyleCategoryPair styleAndCat = Precept_ThingDef.AllPossibleStylesForBuilding(this.ThingDef).RandomElement<StyleCategoryPair>();
				this.ideo.style.SetStyleForThingDef(this.ThingDef, styleAndCat);
			}
		}

		// Token: 0x06008F5D RID: 36701 RVA: 0x0034B1B3 File Offset: 0x003493B3
		public override string GenerateNameRaw()
		{
			return this.name;
		}

		// Token: 0x06008F5E RID: 36702 RVA: 0x0034B1BC File Offset: 0x003493BC
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, this.ThingDef, GenStuff.DefaultStuffFor(this.ThingDef), 1f, this.ideo.GetStyleFor(this.ThingDef), false, null, null, null, 1f);
		}

		// Token: 0x06008F5F RID: 36703 RVA: 0x0034B210 File Offset: 0x00349410
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ThingDef == null)
			{
				Log.Error(base.GetType().Name + " had null thingDef after loading.");
				this.ThingDef = this.def.Worker.ThingDefs.RandomElement<PreceptThingChance>().def;
			}
		}

		// Token: 0x06008F60 RID: 36704 RVA: 0x0034B27D File Offset: 0x0034947D
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			((Precept_ThingDef)other).thingDef = this.thingDef;
		}

		// Token: 0x04005794 RID: 22420
		private ThingDef thingDef;

		// Token: 0x04005795 RID: 22421
		private static List<StyleCategoryPair> tmpStylesForBuilding = new List<StyleCategoryPair>();

		// Token: 0x04005796 RID: 22422
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
