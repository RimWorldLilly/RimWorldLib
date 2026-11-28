using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200164D RID: 5709
	public class Precept_ThingStyle : Precept_ThingDef
	{
		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x06008F63 RID: 36707 RVA: 0x0034B2B5 File Offset: 0x003494B5
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x06008F64 RID: 36708 RVA: 0x0034B2C7 File Offset: 0x003494C7
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x06008F65 RID: 36709 RVA: 0x0034B2CF File Offset: 0x003494CF
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x06008F66 RID: 36710 RVA: 0x0034B2D2 File Offset: 0x003494D2
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x06008F67 RID: 36711 RVA: 0x0034B2D5 File Offset: 0x003494D5
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x06008F68 RID: 36712 RVA: 0x0034B2D8 File Offset: 0x003494D8
		protected virtual string NameRootSymbol
		{
			get
			{
				return "root";
			}
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x06008F69 RID: 36713 RVA: 0x0034B2DF File Offset: 0x003494DF
		protected virtual string ThingLabelCap
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x06008F6A RID: 36714 RVA: 0x0034B2F4 File Offset: 0x003494F4
		public override string GenerateNameRaw()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.def.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (base.ThingDef.ideoBuildingNamerBase != null)
			{
				request.Includes.Add(base.ThingDef.ideoBuildingNamerBase);
			}
			string thingLabelCap = this.ThingLabelCap;
			if (thingLabelCap != null)
			{
				request.Rules.Add(new Rule_String("thingLabel", thingLabelCap));
			}
			string text = null;
			int i = 100;
			while (i > 0)
			{
				text = GrammarResolver.Resolve(this.NameRootSymbol, request, null, false, null, null, null, false);
				i--;
				if (text.Length <= this.def.nameMaxLength)
				{
					break;
				}
			}
			return GenText.CapitalizeAsTitle(text);
		}

		// Token: 0x06008F6B RID: 36715 RVA: 0x0034B3A8 File Offset: 0x003495A8
		public virtual void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x0034B3AA File Offset: 0x003495AA
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x0034B3B1 File Offset: 0x003495B1
		public virtual string TransformThingLabel(string label)
		{
			return label;
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x0034B3B4 File Offset: 0x003495B4
		public virtual string InspectStringExtra(Thing thing)
		{
			return null;
		}
	}
}
