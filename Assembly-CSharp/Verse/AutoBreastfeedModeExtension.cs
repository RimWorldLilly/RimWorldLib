using System;

namespace Verse
{
	// Token: 0x020004DD RID: 1245
	public static class AutoBreastfeedModeExtension
	{
		// Token: 0x06002458 RID: 9304 RVA: 0x000DB90A File Offset: 0x000D9B0A
		public static TaggedString Translate(this AutofeedMode mode)
		{
			switch (mode)
			{
			case AutofeedMode.Never:
				return "AutofeedModeNever".Translate();
			case AutofeedMode.Childcare:
				return "AutofeedModeChildcare".Translate();
			case AutofeedMode.Urgent:
				return "AutofeedModeUrgent".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000DB948 File Offset: 0x000D9B48
		public static TaggedString GetTooltip(this AutofeedMode mode, Pawn baby, Pawn feeder)
		{
			string key;
			switch (mode)
			{
			case AutofeedMode.Never:
				key = "AutofeedModeTooltipNever";
				break;
			case AutofeedMode.Childcare:
				key = "AutofeedModeTooltipChildcare";
				break;
			case AutofeedMode.Urgent:
				key = "AutofeedModeTooltipUrgent";
				break;
			default:
				throw new NotImplementedException();
			}
			return key.Translate(baby.Named("BABY"), feeder.Named("FEEDER"));
		}
	}
}
