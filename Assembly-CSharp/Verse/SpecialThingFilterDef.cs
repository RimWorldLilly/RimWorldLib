using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200072D RID: 1837
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x00171C5A File Offset: 0x0016FE5A
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x00171C80 File Offset: 0x0016FE80
		public override IEnumerable<string> ConfigErrors()
		{
			SpecialThingFilterDef.<ConfigErrors>d__8 <ConfigErrors>d__ = new SpecialThingFilterDef.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00171C90 File Offset: 0x0016FE90
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x0400264A RID: 9802
		public ThingCategoryDef parentCategory;

		// Token: 0x0400264B RID: 9803
		public string saveKey;

		// Token: 0x0400264C RID: 9804
		public bool allowedByDefault;

		// Token: 0x0400264D RID: 9805
		public bool configurable = true;

		// Token: 0x0400264E RID: 9806
		public Type workerClass;

		// Token: 0x0400264F RID: 9807
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
