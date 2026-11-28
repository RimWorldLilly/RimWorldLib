using System;
using System.Text;

namespace RimWorld
{
	// Token: 0x020018C9 RID: 6345
	public class CompInspectStringGeneProcessor : CompInspectString
	{
		// Token: 0x06009F09 RID: 40713 RVA: 0x00390FC4 File Offset: 0x0038F1C4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Props.inspectString);
			stringBuilder.Append(": +");
			stringBuilder.Append(this.parent.GetStatValue(StatDefOf.GeneticComplexityIncrease, true, -1));
			return stringBuilder.ToString();
		}
	}
}
