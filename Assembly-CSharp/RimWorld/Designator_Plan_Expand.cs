using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C79 RID: 7289
	public class Designator_Plan_Expand : Designator_Plan_Add
	{
		// Token: 0x170021C1 RID: 8641
		// (get) Token: 0x0600BA30 RID: 47664 RVA: 0x0041A5A6 File Offset: 0x004187A6
		protected override bool CanSelectColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021C2 RID: 8642
		// (get) Token: 0x0600BA31 RID: 47665 RVA: 0x0041A5A9 File Offset: 0x004187A9
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA32 RID: 47666 RVA: 0x0041A5AC File Offset: 0x004187AC
		public Designator_Plan_Expand()
		{
			this.defaultLabel = "DesignatorExpandPlan".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x0600BA33 RID: 47667 RVA: 0x0041A5D4 File Offset: 0x004187D4
		public void Initialize(ColorDef color)
		{
			this.colorDef = color;
		}
	}
}
