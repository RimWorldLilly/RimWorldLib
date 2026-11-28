using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CC RID: 6348
	public class CompInspectStringStatValue : CompInspectString
	{
		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x06009F0F RID: 40719 RVA: 0x00391094 File Offset: 0x0038F294
		private new CompProperties_InspectStringStatValue Props
		{
			get
			{
				return (CompProperties_InspectStringStatValue)this.props;
			}
		}

		// Token: 0x06009F10 RID: 40720 RVA: 0x003910A4 File Offset: 0x0038F2A4
		public override string CompInspectStringExtra()
		{
			if (this.Props.stat == null || this.Props.stat.Worker.IsDisabledFor(this.parent))
			{
				return null;
			}
			return this.Props.inspectString.Formatted(this.Props.stat.LabelCap, this.Props.stat.Worker.ValueToString(this.parent.GetStatValue(this.Props.stat, true, -1), true, this.Props.numberSense ?? this.Props.stat.toStringNumberSense));
		}
	}
}
