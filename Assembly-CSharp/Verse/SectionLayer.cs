using System;

namespace Verse
{
	// Token: 0x020002AB RID: 683
	public abstract class SectionLayer : MapDrawLayer
	{
		// Token: 0x060014FC RID: 5372 RVA: 0x000799C1 File Offset: 0x00077BC1
		public SectionLayer(Section section) : base(section.map)
		{
			this.section = section;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000799D6 File Offset: 0x00077BD6
		public override CellRect GetBoundaryRect()
		{
			return this.section.CellRect;
		}

		// Token: 0x04001274 RID: 4724
		protected Section section;
	}
}
