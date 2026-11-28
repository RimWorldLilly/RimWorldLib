using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC6 RID: 6854
	public class Building_ResearchBench : Building
	{
		// Token: 0x0600AE73 RID: 44659 RVA: 0x003D6F4C File Offset: 0x003D514C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project != null)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 6);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CurrentProject".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(project.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ResearchProgress".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(project.ProgressApparent, "F0");
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(project.CostApparent, "F0");
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(project.ProgressPercent.ToStringPercent("0.#"));
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CurrentProject".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("None".Translate());
				text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			return text;
		}
	}
}
