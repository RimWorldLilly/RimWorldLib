using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C6 RID: 6342
	public class CompInspectString : ThingComp
	{
		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x06009F02 RID: 40706 RVA: 0x00390EC6 File Offset: 0x0038F0C6
		public CompProperties_InspectString Props
		{
			get
			{
				return (CompProperties_InspectString)this.props;
			}
		}

		// Token: 0x06009F03 RID: 40707 RVA: 0x00390ED4 File Offset: 0x0038F0D4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + this.Props.inspectString.Formatted(this.parent.Named("PARENT")).Resolve();
		}
	}
}
