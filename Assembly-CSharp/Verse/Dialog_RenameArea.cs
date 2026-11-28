using System;

namespace Verse
{
	// Token: 0x02000673 RID: 1651
	public class Dialog_RenameArea : Dialog_Rename<Area>
	{
		// Token: 0x06003422 RID: 13346 RVA: 0x00150428 File Offset: 0x0014E628
		public Dialog_RenameArea(Area area) : base(area)
		{
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x00150434 File Offset: 0x0014E634
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.renaming.Map.areaManager.AllAreas.Any((Area a) => a != this.renaming && a.Label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}
	}
}
