using System;

namespace RimWorld
{
	// Token: 0x0200182F RID: 6191
	public class ITab_BiosculpterNutritionStorage : ITab_Storage
	{
		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x06009B26 RID: 39718 RVA: 0x0037F3BE File Offset: 0x0037D5BE
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009B27 RID: 39719 RVA: 0x0037F3C1 File Offset: 0x0037D5C1
		public ITab_BiosculpterNutritionStorage()
		{
			this.labelKey = "Nutrition";
		}
	}
}
