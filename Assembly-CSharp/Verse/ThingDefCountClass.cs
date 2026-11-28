using System;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006B2 RID: 1714
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x0015E369 File Offset: 0x0015C569
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x0015E37D File Offset: 0x0015C57D
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x0015E390 File Offset: 0x0015C590
		public string Summary
		{
			get
			{
				return this.count.ToString() + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x0015E3C1 File Offset: 0x0015C5C1
		public float DropChance
		{
			get
			{
				return this.chance.GetValueOrDefault(1f);
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x0015E3D3 File Offset: 0x0015C5D3
		public bool IsChanceBased
		{
			get
			{
				return this.chance != null;
			}
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x0015E3E0 File Offset: 0x0015C5E0
		public ThingDefCountClass()
		{
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x0015E3F8 File Offset: 0x0015C5F8
		public ThingDefCountClass(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning("Tried to set ThingDefCountClass count to " + count.ToString() + ". thingDef=" + ((thingDef != null) ? thingDef.ToString() : null));
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0015E458 File Offset: 0x0015C658
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<float?>(ref this.chance, "chance", null, false);
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x0015E4DD File Offset: 0x0015C6DD
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "thingDef", "count");
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x0015E4F0 File Offset: 0x0015C6F0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.count);
			defaultInterpolatedStringHandler.AppendLiteral("x ");
			defaultInterpolatedStringHandler.AppendFormatted((this.thingDef != null) ? this.thingDef.defName : "null");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x0015E55F File Offset: 0x0015C75F
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x0015E576 File Offset: 0x0015C776
		public IngredientCount ToIngredientCount()
		{
			IngredientCount ingredientCount = new IngredientCount();
			ingredientCount.SetBaseCount((float)this.count);
			ingredientCount.filter.SetAllow(this.thingDef, true);
			return ingredientCount;
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x0015E59C File Offset: 0x0015C79C
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x04002339 RID: 9017
		public ThingDef thingDef;

		// Token: 0x0400233A RID: 9018
		public int count = 1;

		// Token: 0x0400233B RID: 9019
		public Color? color;

		// Token: 0x0400233C RID: 9020
		public float? chance;

		// Token: 0x0400233D RID: 9021
		public ThingDef stuff;

		// Token: 0x0400233E RID: 9022
		public QualityCategory quality = QualityCategory.Normal;
	}
}
