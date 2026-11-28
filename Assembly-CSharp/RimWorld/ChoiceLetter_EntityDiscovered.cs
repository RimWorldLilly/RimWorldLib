using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D55 RID: 7509
	public class ChoiceLetter_EntityDiscovered : ChoiceLetter
	{
		// Token: 0x170023E4 RID: 9188
		// (get) Token: 0x0600C25A RID: 49754 RVA: 0x0046D24D File Offset: 0x0046B44D
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_EntityDiscovered.<get_Choices>d__2 <get_Choices>d__ = new ChoiceLetter_EntityDiscovered.<get_Choices>d__2(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C25B RID: 49755 RVA: 0x0046D25D File Offset: 0x0046B45D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<EntityCodexEntryDef>(ref this.codexEntry, "codexEntry");
		}

		// Token: 0x170023E5 RID: 9189
		// (get) Token: 0x0600C25C RID: 49756 RVA: 0x0046D275 File Offset: 0x0046B475
		protected DiaOption Option_OpenEntityCodex
		{
			get
			{
				return new DiaOption("ViewEntityCodex".Translate())
				{
					action = delegate()
					{
						if (this.codexEntry != null)
						{
							Find.WindowStack.Add(new Dialog_EntityCodex(this.codexEntry));
						}
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x04006F1C RID: 28444
		public EntityCodexEntryDef codexEntry;
	}
}
