using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000674 RID: 1652
	public class Dialog_RenameAnimalPen : Dialog_Rename<CompAnimalPenMarker>
	{
		// Token: 0x06003424 RID: 13348 RVA: 0x001504AB File Offset: 0x0014E6AB
		public Dialog_RenameAnimalPen(Map map, CompAnimalPenMarker marker) : base(marker)
		{
			this.map = map;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x001504BC File Offset: 0x0014E6BC
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (name != this.renaming.label && this.map.animalPenManager.GetPenNamed(name) != null)
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x04002213 RID: 8723
		private readonly Map map;
	}
}
