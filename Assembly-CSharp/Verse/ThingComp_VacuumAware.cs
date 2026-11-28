using System;

namespace Verse
{
	// Token: 0x02000548 RID: 1352
	public abstract class ThingComp_VacuumAware : ThingComp
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000F705B File Offset: 0x000F525B
		protected bool InVacuum
		{
			get
			{
				return this.parent.PositionHeld.GetVacuum(this.parent.MapHeld) > 0f;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060027F9 RID: 10233
		protected abstract bool FunctionsInVacuum { get; }

		// Token: 0x060027FA RID: 10234 RVA: 0x000F7080 File Offset: 0x000F5280
		public override string CompInspectStringExtra()
		{
			if (this.InVacuum && !this.FunctionsInVacuum)
			{
				return "CannotFunctionInVacuum".Translate().CapitalizeFirst().Colorize(ColoredText.WarningColor);
			}
			return null;
		}
	}
}
