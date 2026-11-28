using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A3F RID: 6719
	public class CompUseEffect_InstallImplantMechlink : CompUseEffect_InstallImplant
	{
		// Token: 0x0600A87A RID: 43130 RVA: 0x003BDBBB File Offset: 0x003BBDBB
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!ModLister.CheckBiotech("install implant mechlink"))
			{
				return false;
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0600A87B RID: 43131 RVA: 0x003BDBD7 File Offset: 0x003BBDD7
		public override TaggedString ConfirmMessage(Pawn p)
		{
			if (p.WorkTypeIsDisabled(WorkTypeDefOf.Smithing))
			{
				return "ConfirmInstallMechlink_Smithing".Translate();
			}
			if (p.WorkTagIsDisabled(WorkTags.Intellectual))
			{
				return "ConfirmInstallMechlink_Intellectual".Translate();
			}
			return null;
		}
	}
}
