using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE6 RID: 8166
	public class Verb_EntitySkip : Verb_CastAbility
	{
		// Token: 0x0600CF0F RID: 53007 RVA: 0x004C44CE File Offset: 0x004C26CE
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return ModLister.AnomalyInstalled && base.IsApplicableTo(target, showMessages);
		}

		// Token: 0x0600CF10 RID: 53008 RVA: 0x004C44E6 File Offset: 0x004C26E6
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x0600CF11 RID: 53009 RVA: 0x004C44F5 File Offset: 0x004C26F5
		public override void OnGUI(LocalTargetInfo target)
		{
			base.DrawAttachmentExtraLabel(target);
		}
	}
}
