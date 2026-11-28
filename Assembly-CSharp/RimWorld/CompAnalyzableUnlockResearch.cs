using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F3 RID: 6131
	public class CompAnalyzableUnlockResearch : CompAnalyzable
	{
		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x060098FD RID: 39165 RVA: 0x00376533 File Offset: 0x00374733
		public new CompProperties_CompAnalyzableUnlockResearch Props
		{
			get
			{
				return (CompProperties_CompAnalyzableUnlockResearch)this.props;
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x060098FE RID: 39166 RVA: 0x00376540 File Offset: 0x00374740
		public List<ResearchProjectDef> ResearchUnlocked
		{
			get
			{
				if (this.researchUnlocked == null)
				{
					this.researchUnlocked = new List<ResearchProjectDef>();
					foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
					{
						if (!researchProjectDef.requiredAnalyzed.NullOrEmpty<ThingDef>() && researchProjectDef.requiredAnalyzed.Contains(this.parent.def))
						{
							this.researchUnlocked.Add(researchProjectDef);
						}
					}
				}
				return this.researchUnlocked;
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x060098FF RID: 39167 RVA: 0x003765D0 File Offset: 0x003747D0
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?((from r in this.ResearchUnlocked
				select r.label).ToCommaList(true, false).Named("RESEARCH"));
			}
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x06009900 RID: 39168 RVA: 0x0037661D File Offset: 0x0037481D
		public override int AnalysisID
		{
			get
			{
				return this.Props.analysisID;
			}
		}

		// Token: 0x06009901 RID: 39169 RVA: 0x0037662C File Offset: 0x0037482C
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null && this.Props.requiresMechanitor && !MechanitorUtility.IsMechanitor(activateBy))
			{
				return "RequiresMechanitor".Translate();
			}
			return true;
		}

		// Token: 0x04005B6C RID: 23404
		private List<ResearchProjectDef> researchUnlocked;
	}
}
