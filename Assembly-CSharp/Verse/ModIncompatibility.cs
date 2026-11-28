using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000346 RID: 838
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x0009B48E File Offset: 0x0009968E
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0009B4A9 File Offset: 0x000996A9
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId, false) == null;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0009B4BC File Offset: 0x000996BC
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null && modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0009B528 File Offset: 0x00099728
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier != null && modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}
	}
}
